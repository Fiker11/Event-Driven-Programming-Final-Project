using Microsoft.AspNetCore.Mvc;
using Reports.Services;
using Reports.Models;
using Reports.Dtos;


namespace Reports.Controllers{
    [ApiController]
    [Route("api/[Controller]")]

    public class ReportController : ControllerBase{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService){
        _reportService = reportService;
    }

    [HttpGet]
    public async Task<ActionResult> GetReports(){
        try{
            return Ok(await _reportService.GetReports());
        }catch(Exception e){
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetRepotById(string id){
        try{
            return Ok(await _reportService.GetReportById(id));
        }catch(Exception e){
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateReport([FromBody] ReportDto reportDto)
        {
            
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Map DTO to Model

                var report = new Report
                {
                    ReportType = reportDto.ReportType,
                    ReportDescription = reportDto.ReportDescription,
                    ReportStatus = reportDto.ReportStatus, // Map the enum
                    ReportLocation = reportDto.ReportLocation,
                    ReportDateAndTime = DateTime.UtcNow // Automatically set the current time
                };
                var result = await _reportService.CreateReport(report);
                return Ok(result);
            

                //         return Ok(await _reportService.CreateReport(reportDto));
                // }catch(Exception e){
                //     return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                // }
        }
            

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateReport(string id, Report report) {
        try {
            return Ok(await _reportService.UpdateReport(id, report));
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteReport(string id) {
        try {
            return Ok(await _reportService.DeleteReport(id));
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    }
}