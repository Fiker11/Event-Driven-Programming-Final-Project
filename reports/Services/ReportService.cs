using MongoDB.Driver;
using Reports.Enums;
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

        //create a report
        public async Task CreateReport(Report report)
        {
            try
            {   
                await _reportCollection.InsertOneAsync(report);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //get all reports
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

        //get a report by id
        public async Task<Report> GetReportById(string id)
        {
            //check if the report exixtes
            var report = await _reportCollection.Find(report => report.ReportId == id).FirstOrDefaultAsync();

            if (report == null)
            {
                throw new KeyNotFoundException($"Report with ID {id} not found.");
            }
            try
            {
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve the report.", ex);
            }
        }

        //update a report
        public async Task UpdateReport(string id, Report report)
        {
            //get the report with the id
            var existingReport = await GetReportById(id);

            report.ReportId = id; //so that the id will be consistent

            try
            {
                //update the report
                var result = await _reportCollection.ReplaceOneAsync(r => r.ReportId == id, report);

                //if the report was not updated, throw an exception
                if (result.ModifiedCount == 0)
                {
                    throw new Exception($"Failed to update the report with ID {id}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update the report.", ex);
            }
        }

        //delete a report
        public async Task DeleteReport(string id)
        {
            //Check if the report with the id exists
            var existingReport = await GetReportById(id);

            try
            {
                //if the report exists, delete it
                var result = await _reportCollection.DeleteOneAsync(report => report.ReportId == id);

                //if the report is not deleted, throw an exception
                if (result.DeletedCount == 0)
                {
                    throw new Exception($"Failed to delete the report with ID {id}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete the report.", ex);
            }
        }

        //get reports paginated
        public async Task<List<Report>> GetReportsPaginated(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and page size must be greater than 0.");
            }
            try
            {
                return await _reportCollection.Find(report => true)
                    .Skip((pageNumber - 1) * pageSize)
                    .Limit(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve reports.", ex);
            }
        }

        //search reports by type, status, and location
        public async Task<List<Report>> SearchReports(string? type, string? status, string? location)
        {
            try {
                var filters = Builders<Report>.Filter.Empty;

                if (!string.IsNullOrWhiteSpace(type))
                {
                    if (Enum.TryParse<ReportType>(type, true, out var reportType))
                    {
                        filters &= Builders<Report>.Filter.Eq(r => r.ReportType, reportType);
                    }
                }

                if (!string.IsNullOrWhiteSpace(status))
                {
                    if (Enum.TryParse<ReportStatus>(status, true, out var reportStatus))
                    {
                        filters &= Builders<Report>.Filter.Eq(r => r.ReportStatus, reportStatus);
                    }
                }

                if (!string.IsNullOrWhiteSpace(location))
                {
                    filters &= Builders<Report>.Filter.Regex(r => r.ReportLocation, new MongoDB.Bson.BsonRegularExpression(location, "i"));
                }

                return await _reportCollection.Find(filters).ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to search reports.", ex);
            }
        }

    }
}