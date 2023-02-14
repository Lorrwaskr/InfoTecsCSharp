using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text.RegularExpressions;
using InfoTecsCSharp.Models;
using Microsoft.AspNetCore.Mvc;
using InfoTecsCSharp.Controllers;
using InfoTecsCSharp.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace InfoTecsCSharp.Services
{
    public class HomeService
    {
        private readonly DateTime START_DATE = new DateTime(2000, 1, 1);
        private InfoTecsDbContext _dbContext;

        public HomeService(InfoTecsDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public async Task<IActionResult> ParseCsv(IFormFile file, HomeController homeController) {
            var oldFileEntity = await _dbContext.Files.FirstOrDefaultAsync(x => x.Name == file.FileName);

            var newFileEntity = new FileDescription { Name = file.FileName };
            await _dbContext.AddAsync(newFileEntity);

            var tableEntries = new List<TableEntry>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    Delimiter = ";"
                };
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    Regex regex = new Regex(@"[0-9]{4}-[0-9]{2}-[0-9]{2}_[0-9]{2}-[0-9]{2}-[0-9]{2}");
                    var records = csv.GetRecordsAsync<dynamic>();
                    var skippedLines = 0;
                    await foreach (var record in records)
                    {
                        int operationSeconds = -1;
                        float value = -1;
                        if (!regex.IsMatch(record.Field1) || !int.TryParse(record.Field2, out operationSeconds) || !float.TryParse(record.Field3, out value))
                        {
                            skippedLines++;
                            continue;
                        }
                        DateTime operationDateTime = DateTime.ParseExact(record.Field1, "yyyy-MM-dd_HH-mm-ss", System.Globalization.CultureInfo.InvariantCulture);
                        if (operationDateTime < START_DATE || operationSeconds < 0 || value < 0)
                        {
                            skippedLines++;
                            continue;
                        }
                        var newTableEntry = new TableEntry()
                        {
                            OperationDateTime = operationDateTime,
                            OperationSeconds = operationSeconds,
                            Value = value,
                            FileDescription = newFileEntity
                        };
                        tableEntries.Add(newTableEntry);
                    }
                    if (tableEntries.Count < 1 || tableEntries.Count > 10000)
                    {
                        return homeController.ValidationProblem("The number of lines in the file is less than 1 or greater than 10000");
                    }
                    if (oldFileEntity != default)
                    {
                        _dbContext.Files.Remove(oldFileEntity);
                    }
                    await _dbContext.AddRangeAsync(tableEntries);
                }
            }

            await CalculateResult(newFileEntity, tableEntries, homeController);
            await _dbContext.SaveChangesAsync();
            return homeController.CreatedAtAction("ParseCsv", newFileEntity);
        }

        public async Task<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ResultModel>> CalculateResult(FileDescription file, IEnumerable<TableEntry> tableEntries, HomeController homeController)
        {
            var tableEntriesSortedByValue = tableEntries.OrderBy(x => x.Value);
            var results = new ResultModel
            {
                FileDescription = file,
                AllTimeSeconds = (int)(tableEntries.Max(x => x.OperationDateTime) - tableEntries.Min(x => x.OperationDateTime)).TotalSeconds,
                FirstOperation = tableEntries.Min(x => x.OperationDateTime),
                AverageOperationTime = (float)tableEntries.Average(x => x.OperationSeconds),
                AverageValue = (float)tableEntries.Average(x => x.Value),
                MedianValue = (float)tableEntries.Count() == 0 ?
                    ((tableEntriesSortedByValue.ElementAt(tableEntriesSortedByValue.Count() / 2).Value + tableEntriesSortedByValue.ElementAt((tableEntriesSortedByValue.Count() / 2) - 1).Value)) / 2 :
                    tableEntriesSortedByValue.ElementAt(tableEntriesSortedByValue.Count() / 2).Value,
                MaximumValue = (float)tableEntries.Max(x => x.Value),
                MinimumValue = (float)tableEntries.Min(x => x.Value),
                RowsCount = tableEntries.Count()
            };

            return await _dbContext.AddAsync(results);
        }

        public async Task<IActionResult> GetResults(GetResultFilter filter, HomeController homeController)
        {
            var results = await _dbContext.Results.ToListAsync();
            if (filter.Filename != default)
            {
                var file = _dbContext.Files.FirstOrDefaultAsync(x => x.Name == filter.Filename);
                if (file == default)
                {
                    return homeController.BadRequest("There is no such filename in the database");
                }
                results = results.Where(x => x.FileDescriptionId == file.Id).ToList();
            }

            if (filter.FirstOperationFrom != default && filter.FirstOperationTo != default)
            {
                results = results.Where(x =>
                    x.FirstOperation > filter.FirstOperationFrom &&
                    x.FirstOperation < filter.FirstOperationTo
                ).ToList();
            }

            if (filter.AverageValueFrom != default && filter.AverageValueTo != default)
            {
                results = results.Where(x =>
                    x.AverageValue > filter.AverageValueFrom &&
                    x.AverageValue < filter.AverageValueTo
                ).ToList();
            }

            if (filter.AverageOperationTimeFrom != default && filter.AverageOperationTimeTo != default)
            {
                results = results.Where(x => 
                x.AverageOperationTime > filter.AverageOperationTimeFrom &&
                x.AverageOperationTime < filter.AverageOperationTimeTo
                ).ToList();
            }

            return homeController.Json(results);
        }

        public async Task<IActionResult> GetValuesByFilename(string filename, HomeController homeController)
        {
            var file = await _dbContext.Files.Include(x => x.TableEntries).FirstOrDefaultAsync(x => x.Name == filename);
            if (file == default)
            {
                 return homeController.BadRequest("There is no such filename in the database");
            }
            return homeController.Json(file.TableEntries);
        }
    }
}
