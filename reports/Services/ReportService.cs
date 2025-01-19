using MongoDB.Driver;
using Reports.Models;

namespace Reports.Services
{
    public class ReportService : IReportService
    {
        private readonly IMongoCollection<Report> _reportCollection;

        public ReportService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDbSettings:DatabaseName"]);
            _reportCollection = database.GetCollection<Report>(config["MongoDbSettings:CollectionName"]);
        }

        //create a new report
        public async Task<string> CreateReport(Report report)
        {
            try
            {
                report.ReportDateAndTime = DateTime.UtcNow; // Set to current UTC time
                await _reportCollection.InsertOneAsync(report);
                return "report created successfully!";
            }
            catch
            {
                return "Failed to create report";
            }
        }

        //delete a report
        public async Task<string> DeleteReport(string id)
        {
            try
            {
                //check if the report exists
                var existingReport = await _reportCollection.Find(report => report.ReportId == id).FirstOrDefaultAsync();
                if (existingReport == null)
                {
                    return $"Report with the id {id} not found.";
                }
                
                //if the report existes delete the report
                var status = await _reportCollection.DeleteOneAsync(report => report.ReportId == id);
                if (status.DeletedCount > 0)
                {
                    return $"Report with id {id} deleted successfully.";
                }
                else
                {
                    return "Failed to delete the report.";
                }
            }
            catch(Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        public async Task<Report> GetReportById(string id)
        {
            return await _reportCollection.Find(report => report.ReportId == id).FirstOrDefaultAsync();
        }

        public async Task<List<Report>> GetReports()
        {
            return await _reportCollection.Find(report => true).ToListAsync();
        }

        public async Task<string> UpdateReport(string id, Report report)
        {
            try
            {
                report.ReportId = id;
                var status = await _reportCollection.ReplaceOneAsync(report => report.ReportId == id, report);
                if (status.ModifiedCount > 0)
                {
                   return $"Report with id {id} updated successfully!";
                }
                else
                {
                    return "Failed to update the report.";
                }
            }
            catch
            {
                return "An error occurred.";
            }
        }
    }
}