using Microsoft.AspNetCore.Mvc;
using Reports.Services;
using Reports.Models;
using Reports.Dtos;
using Reports.Enums;

namespace Reports.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        //create a report
        [HttpPost]
        public async Task<ActionResult> CreateReport([FromBody] ReportDto reportDto)
        {  
            //check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //create a new report
            var report = new Report
            {
                ReportType = reportDto.ReportType,
                ReportStatus = reportDto.ReportStatus != 0 ? reportDto.ReportStatus : ReportStatus.Pending,
                ReportDescription = reportDto.ReportDescription,
                ReportLocation = reportDto.ReportLocation,
                ReportDateAndTime = DateTime.UtcNow //set the created at date
            };

            //try to create the report or else catch the exception
            try
            {
                await _reportService.CreateReport(report);
                return Ok("Report created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //get all reports
        [HttpGet]
        public async Task<ActionResult<List<Report>>> GetReports()
        {
            try
            {
                var reports = await _reportService.GetReports();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //get a report by id
        [HttpGet("search/{id}")]
        public async Task<ActionResult<Report>> GetReportById(string id)
        {
            try
            {
                var report = await _reportService.GetReportById(id);
                return Ok(report);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //update a report
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateReport(string id, ReportDto reportDto)
        {
            //check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            //try to update the report or else catch the exception
            try
            {
                var report = new Report
                {
                    ReportType = reportDto.ReportType,
                    ReportStatus = reportDto.ReportStatus,
                    ReportDescription = reportDto.ReportDescription,
                    ReportLocation = reportDto.ReportLocation
                };

                await _reportService.UpdateReport(id, report);
                return Ok("Report updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //Delete a report by its id
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteReport(string id)
        {
            try
            {
                await _reportService.DeleteReport(id);
                return Ok($"Report with ID {id} deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //get all reports paginated
        [HttpGet("paginated")]
        public async Task<ActionResult<List<Report>>> GetReportsPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var reports = await _reportService.GetReportsPaginated(pageNumber, pageSize);
                return Ok(reports);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //search reports by differnt filters 
        [HttpGet("search")]
        public async Task<ActionResult<List<Report>>> SearchReports(
            [FromQuery] string? type,
            [FromQuery] string? status,
            [FromQuery] string? location
            )
        {
            try
            {
                var reports = await _reportService.SearchReports(type, status, location);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}