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
        public async Task<bool> CreateReport(Report report)
        {
            try
            {
                await _reportCollection.InsertOneAsync(report);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //delete a report
        public async Task<bool> DeleteReport(string id)
        {
            try
            {
                var existingReport = await _reportCollection.Find(report => report.ReportId == id).FirstOrDefaultAsync();
                if (existingReport == null)
                {
                    Console.WriteLine("Report not found");
                }

                var status = await _reportCollection.DeleteOneAsync(report => report.ReportId == id);
                if (status.DeletedCount > 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Report not found"); // we need to work on edge cases
                }
            }
            catch
            {
                return false;
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

        public async Task<bool> UpdateReport(string id, Report report)
        {
            try
            {
                report.ReportId = id;
                var status = await _reportCollection.ReplaceOneAsync(report => report.ReportId == id, report);
                if (status.ModifiedCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}