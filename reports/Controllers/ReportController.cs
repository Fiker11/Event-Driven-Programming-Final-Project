using Microsoft.AspNetCore.Mvc;
using Reports.Services;
using Reports.Models;
using Reports.Dtos;
using Reports.Enums;


namespace Reports.Controllers{
    [ApiController]
    [Route("api/[Controller]")]

    public class ReportController : ControllerBase{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService){
        _reportService = reportService;
    }


    //Create a new report that validates the input and set the default status to pending if its not set..
    [HttpPost]
    public async Task<ActionResult> CreateReport([FromBody] ReportDto reportDto)
    {
        if(!ModelState.IsValid){
            return BadRequest("Invalid model state");
        }else
        {
            try
            {
                var report = new Report
                {
                    ReportType = reportDto.ReportType,
                    ReportStatus = reportDto.ReportStatus != 0 ? reportDto.ReportStatus : ReportStatus.Pending, // Default to pending if not set
                    ReportDescription = reportDto.ReportDescription,
                    ReportLocation = reportDto.ReportLocation,
                    ReportDateAndTime = DateTime.UtcNow
                };
                return Ok(await _reportService.CreateReport(report));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
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