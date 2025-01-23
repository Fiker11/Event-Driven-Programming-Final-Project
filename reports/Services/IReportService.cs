using Reports.Models;

namespace Reports.Services{
    public interface IReportService{

        //Method to create a report
        public Task CreateReport(Report report);

        //Method to get all reports
        public Task<List<Report>> GetReports();

        //Method to get a report by id
        public Task<Report> GetReportById(string id);
        
        //Method to update a report
        public Task UpdateReport(string id, Report report);
        
        //Method to remove a report
        public Task DeleteReport(string id);
        
        // Method to search reports
        Task<List<Report>> SearchReports(string? type, string? status, string? location, string? description);
    }
}