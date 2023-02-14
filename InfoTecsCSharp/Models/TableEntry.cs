using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InfoTecsCSharp.Models
{
    public class TableEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, JsonIgnore]
        public FileDescription? FileDescription { get; set; }

        [Required]
        public int FileDescriptionId { get; set; }

        [Required]
        [Display(Name = "Date and time of operation")]
        public DateTime OperationDateTime { get; set; }

        [Required]
        [Display(Name = "Operation time in seconds")]
        [Range(0, int.MaxValue)]
        public int OperationSeconds { get; set; }

        [Required]
        [Display(Name = "Value")]
        [Range(0, int.MaxValue)]
        public float Value { get; set; }
    }
}
