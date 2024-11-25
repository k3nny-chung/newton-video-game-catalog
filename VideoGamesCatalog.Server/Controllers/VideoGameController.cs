using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoGamesCatalog.Core.Data.Models;
using VideoGamesCatalog.Core.Services;
using VideoGamesCatalog.Server.Dto;

namespace VideoGamesCatalog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        private readonly IVideoGameService _videoGameService;
        private readonly IMapper _mapper;

        public VideoGameController(IVideoGameService videoGameService, IMapper mapper) 
        {
            _videoGameService = videoGameService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<VideoGameDto>>> Get([FromQuery] VideoGameSearchFilter filter)
        {
            var result = await _videoGameService.FindVideoGames(filter);
            return _mapper.Map<PagedResult<VideoGameDto>>(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoGameDto>> Get(int id)
        {
            var result = await _videoGameService.GetVideoGame(id);
            if (result == null)
            {
                return NotFound();
            }

            return _mapper.Map<VideoGameDto>(result);
        }

        [HttpPost]
        public async Task<ActionResult<VideoGameDto>> Post([FromBody] VideoGameDto videoGameDto)
        {
            var result = await _videoGameService.AddVideoGame(_mapper.Map<VideoGame>(videoGameDto));
            return CreatedAtAction("Post", _mapper.Map<VideoGameDto>(result));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] VideoGameDto videoGameDto)
        {
            var result = await _videoGameService.UpdateVideoGame(_mapper.Map<VideoGame>(videoGameDto));
            if (result == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _videoGameService.RemoveVideoGame(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
