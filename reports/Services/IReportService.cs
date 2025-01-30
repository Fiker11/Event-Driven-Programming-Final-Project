using Reports.Models;

namespace Reports.Services
{
    //Interface for the ReportService
    public interface IReportService
    {
        //Method to create a report
        public Task CreateReport(Report report);

        //Method to get all reports
        public Task<List<Report>> GetReports();

        //Method to get a report by id
        public Task<Report> GetReportById(string id);

        //Method to update a report by id
        public Task UpdateReport(string id, Report report);

        //Method to remove a report by id
        public Task DeleteReport(string id);

        //Method to get reports paginated
        Task<List<Report>> GetReportsPaginated(int pageNumber, int pageSize);
        
        // Method to search reports by type, status and location
        Task<List<Report>> SearchReports(string? type, string? status, string? location);
    }
}