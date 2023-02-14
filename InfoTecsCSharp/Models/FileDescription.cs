using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoTecsCSharp.Models
{
    public class FileDescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[\w,\s-]+\.csv")]
        [StringLength(255, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        public List<TableEntry> TableEntries { get; set; } = new ();

        [Required]
        public ResultModel ResultModel { get; set; } = new();
    }
}
