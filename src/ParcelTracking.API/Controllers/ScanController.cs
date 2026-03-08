using Microsoft.AspNetCore.Mvc;
using ParcelTracking.Application.Commands;
using ParcelTracking.Application.Interfaces;


namespace ParcelTracking.API.Controllers
{
    [ApiController]
    [Route("api/scans")]
    public class ScanController : ControllerBase
    {
        private readonly IParcelService _parcelService;

        public ScanController(IParcelService parcelService)
        {
            _parcelService = parcelService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitScan([FromBody] SubmitScanCommand command)
        {
            await _parcelService.ProcessScanAsync(command);

            return Ok(new { message = "Scan processed successfully" });
        }
    }
}
