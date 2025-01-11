using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Reports.Models{
    public class Report{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ReportId{ get; set; } //unique identifier for the report

        public string? ReportType{ get; set; } //type of report

        public string? ReportDescription{ get; set; } //Detailed description of the report

        public string? ReportStatus{ get; set; }//status of the report
        
        public string? ReportDateAndTime{ get; set; }//date and time of the report

        public string? ReportLocation{ get; set; } //location of the report
    }
}