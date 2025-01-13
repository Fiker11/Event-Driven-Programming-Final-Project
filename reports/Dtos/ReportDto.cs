using System.ComponentModel.DataAnnotations;
using Reports.Enums;

namespace Reports.Dtos{
    public class ReportDto{

        [Required]
        public ReportType ReportType{ get; set; }

        [Required]
        public ReportStatus ReportStatus{ get; set; }

        [Required]
        [StringLength(500)]
        public string? ReportDescription { get; set; }

        [Required]
        [StringLength(100)]
        public string? ReportLocation { get; set; }

        //ReportDateAndTIme is removed because the user doesnt provide the date and time

    }
}