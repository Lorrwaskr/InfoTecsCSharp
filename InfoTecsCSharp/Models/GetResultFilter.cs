using System.ComponentModel.DataAnnotations;

namespace InfoTecsCSharp.Models
{
    public class GetResultFilter
    {
        [RegularExpression(@"^[\w,\s-]+\.csv")]
        [StringLength(255, MinimumLength = 4)]
        public string? Filename { get; set; }
        public DateTime? FirstOperationFrom { get; set; }
        public DateTime? FirstOperationTo { get; set; }
        [Range(0, float.MaxValue)]
        public float? AverageOperationTimeFrom { get; set; }
        [Range(0, float.MaxValue)]
        public float? AverageOperationTimeTo { get; set; }
        [Range(0, float.MaxValue)]
        public float? AverageValueFrom { get; set; }
        [Range(0, float.MaxValue)]
        public float? AverageValueTo { get; set; }
    }
}
