using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParcelTracking.Application.Interfaces;
using ParcelTracking.Domain.Interfaces;

namespace ParcelTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelController : ControllerBase
    {
        private readonly IParcelService _parcelService;
        private readonly IParcelEventRepository _eventRepository;
        private readonly ITrackingQueryRepository _queryRepository;
        public ParcelController(
            IParcelService parcelService,
            IParcelEventRepository eventRepository,
            ITrackingQueryRepository queryRepository)
        {
            _parcelService = parcelService;
            _eventRepository = eventRepository;
            _queryRepository = queryRepository;
        }

        [HttpGet("{trackingId}")]
        public async Task<IActionResult> GetParcel(string trackingId)
        {
            var parcel = await _parcelService.GetParcelAsync(trackingId);

            if (parcel == null)
                return NotFound();

            return Ok(parcel);
        }

        [HttpGet("{trackingId}/events")]
        public async Task<IActionResult> GetParcelEvents(string trackingId)
        {
            var events = await _eventRepository.GetEventsAsync(trackingId);

            return Ok(events);
        }

        [HttpGet("tracking/{trackingId}")]
        public async Task<IActionResult> GetTracking(string trackingId)
        {
            var result = await _queryRepository.GetAsync(trackingId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
