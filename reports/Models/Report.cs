using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Reports.Enums;

namespace Reports.Models{
    public class Report{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ReportId{ get; set; } //unique identifier for the report

        [BsonRepresentation(BsonType.String)] // to store an enum as a string in mongodb
        public ReportType ReportType{ get; set; } //type of report only from the enum class

        public string? ReportDescription{ get; set; } //Detailed description of the report

        [BsonRepresentation(BsonType.String)]
        public ReportStatus ReportStatus{ get; set; }//status of the report
        
        public DateTime ReportDateAndTime { get; set; }//date and time of the report

        public string? ReportLocation{ get; set; } //location of the report
    }
}