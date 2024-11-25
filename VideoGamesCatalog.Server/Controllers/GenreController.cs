using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoGamesCatalog.Core.Services;
using VideoGamesCatalog.Server.Dto;

namespace VideoGamesCatalog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;

        public GenreController(IGenreService genreService, IMapper mapper)
        {
            _genreService = genreService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<GenreDto>> Get()
        {
            var genres = await _genreService.GetAllGenres();
            return _mapper.Map<List<GenreDto>>(genres);
        }

    }
}
