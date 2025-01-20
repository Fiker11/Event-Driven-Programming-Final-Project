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

        //CREATE NEW REPORT
        public async Task CreateReport(Report report)
        {
            try
            {   //Set the report date and time to the current UTC time
                report.ReportDateAndTime = DateTime.UtcNow;
                await _reportCollection.InsertOneAsync(report);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create report.", ex);
            }
        }

        //DELETE REPORT
        public async Task DeleteReport(string id)
        {
            //Check if the report with the id exists
            var existingReport = await _reportCollection.Find(report => report.ReportId == id).FirstOrDefaultAsync();

            //if the report does not exist, throw an exception
            if (existingReport == null)
            {
                throw new KeyNotFoundException($"Report with ID {id} not found.");
            }

            //if the report exists, delete it
            var result = await _reportCollection.DeleteOneAsync(report => report.ReportId == id);

            //if something went wrong, throw an exception
            if (result.DeletedCount == 0)
            {
                throw new Exception($"Failed to delete the report with ID {id}.");
            }
        }

        //GET REPORT BY ID
        public async Task<Report> GetReportById(string id)
        {   //check if the report exixtes
            var report = await _reportCollection.Find(report => report.ReportId == id).FirstOrDefaultAsync();
            if (report == null)
            {
                throw new KeyNotFoundException($"Report with ID {id} not found.");
            }
            return report;
        }

        //GET ALL REPORTS
        public async Task<List<Report>> GetReports()
        {
            try
            {
                return await _reportCollection.Find(report => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve reports.", ex);
            }
        }

        //UPDATE REPORT
        public async Task UpdateReport(string id, Report report)
        {   
            //get the report with the id
            var existingReport = await GetReportById(id);

            report.ReportId = id; //so that the id will be consistent

            //update the report
            var result = await _reportCollection.ReplaceOneAsync(r => r.ReportId == id, report);

            //if the report was not updated, throw an exception
            if (result.ModifiedCount == 0)
            {
                throw new Exception($"Failed to update the report with ID {id}.");
            }
        }
    }
}