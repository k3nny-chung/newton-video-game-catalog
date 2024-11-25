using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoGamesCatalog.Core.Services;
using VideoGamesCatalog.Server.Dto;

namespace VideoGamesCatalog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgeRatingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAgeRatingService _ageRatingService;

        public AgeRatingController(IAgeRatingService ageRatingService, IMapper mapper)
        {
            _mapper = mapper;
            _ageRatingService = ageRatingService;
        }

        [HttpGet]
        public async Task<List<AgeRatingDto>> Get()
        {
            var ageRatings = await _ageRatingService.GetAllAgeRatings();
            return _mapper.Map<List<AgeRatingDto>>(ageRatings);
        }
    }
}
