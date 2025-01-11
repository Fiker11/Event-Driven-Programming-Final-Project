using Reports.Models;

namespace Reports.Services{
    public interface IReportService{
        //Method to get all reports
        public Task<List<Report>> GetReports();

        //Method to get a report by id
        public Task<Report> GetReportById(string id);
        
        //Method to create a report
        public Task<string> CreateReport(Report report);
        //Method to update a report
        public Task<string> UpdateReport(string id, Report report);
        
        //Method to remove a report
        public Task<string> DeleteReport(string id);
    }
}