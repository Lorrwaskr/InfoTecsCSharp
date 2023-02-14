using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InfoTecsCSharp.Models
{
    public class ResultModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, JsonIgnore]
        public FileDescription? FileDescription { get; set; }

        [Required]
        public int FileDescriptionId { get; set; }

        [Required]
        public int AllTimeSeconds { get; set; }

        [Required]
        public DateTime FirstOperation { get; set; }

        [Required]
        public float AverageOperationTime { get; set; }

        [Required]
        public float AverageValue { get; set; }

        [Required]
        public float MedianValue { get; set; }

        [Required]
        public float MaximumValue { get; set; }

        [Required]
        public float MinimumValue { get; set; }

        [Required]
        public int RowsCount { get; set; }
    }
}
