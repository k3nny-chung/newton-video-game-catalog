using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoGamesCatalog.Core.Services;
using VideoGamesCatalog.Server.Dto;

namespace VideoGamesCatalog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlatformService _platformService;

        public PlatformController(IPlatformService platformService, IMapper mapper)
        {
            _mapper = mapper;
            _platformService = platformService;
        }

        [HttpGet]
        public async Task<List<PlatformDto>> Get()
        {
            var platforms = await _platformService.GetAllPlatforms();
            return _mapper.Map<List<PlatformDto>>(platforms);
        }
    }
}
