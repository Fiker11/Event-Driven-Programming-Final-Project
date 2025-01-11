using Microsoft.AspNetCore.Mvc;
using Reports.Services;
using Reports.Models;


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
    public async Task<ActionResult> CreateReport(Report report){
        try{
            return Ok(await _reportService.CreateReport(report));
        }catch(Exception e){
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
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