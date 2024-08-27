using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WebApplication1.Custom;
using WebApplication1.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication1.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        private readonly CrudPruebaContext dbContext;
        private readonly Utilidades utilidades;

        public AnimeController(CrudPruebaContext _dbContext, Utilidades _utilidades)
        {
            this.dbContext = _dbContext;
            this.utilidades = _utilidades;
        }
        [HttpGet]
        [Route("ObtenerAnimes")]
        public async Task<IActionResult> Get()
        {
            var animes = await dbContext.Animes.ToListAsync();
            var animesConImagenBase64 = animes.Select(a => new {
                a.IdAnime,
                a.Nombre,
                a.Texto,
                Imagen = a.Imagen != null ? Convert.ToBase64String(a.Imagen) : null
            }).ToList();

            return StatusCode(StatusCodes.Status200OK, animesConImagenBase64);
        }
        //cambios

        [HttpGet]
        [Route("ObtenerAnimesConNombre/{nombre}")]
        public async Task<IActionResult> GetAnimesConNombre(string nombre)
        {
            var animes = await dbContext.Animes.Where(a =>  EF.Functions.Like(a.Nombre, "%"+nombre+"%")).ToListAsync();
            var animesConImagenBase64 = animes.Select(a => new {
                a.IdAnime,
                a.Nombre,
                a.Texto,
                Imagen = a.Imagen != null ? Convert.ToBase64String(a.Imagen) : null
            }).ToList();
            return StatusCode(StatusCodes.Status200OK, animesConImagenBase64);
        }
        [HttpGet]
        [Route("ObtenerAnime/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var anime = await dbContext.Animes.FindAsync(id);

            if (anime == null)
            {
                return NotFound(); // Devuelve 404 si no se encuentra el anime
            }

            var animeConImagenBase64 = new
            {
                anime.IdAnime,
                anime.Nombre,
                anime.Texto,
                Imagen = anime.Imagen != null ? Convert.ToBase64String(anime.Imagen) : null
            };

            return StatusCode(StatusCodes.Status200OK, animeConImagenBase64);
        }
    }
}