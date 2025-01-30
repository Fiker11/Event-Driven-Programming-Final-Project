using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Reports.Enums;

namespace Reports.Dtos{
    //Dto class for the report
    public class ReportDto{

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))] // Converts enums to strings
        public ReportType ReportType{ get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))] // Converts enums to strings
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