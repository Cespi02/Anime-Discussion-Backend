using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Custom;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly CrudPruebaContext dbContext;
        private readonly Utilidades utilidades;

        public ComentarioController(CrudPruebaContext _dbContext, Utilidades _utilidades)
        {
            this.dbContext = _dbContext;
            this.utilidades = _utilidades;
        }


        [HttpGet]
        [Route("ObtenerComentarios/{id:int}")]
        public async Task<IActionResult> Get(int id)      
        {
            var comentarios = await dbContext.Comentarios.Where(a => a.IdAnime == id)
                   .Join(dbContext.Clientes,
                   comentario => comentario.IdUsuario,
                   usuario => usuario.IdCliente,
                   (comentario, usuario) => new
                   {
                       comentarioId = comentario.IdComentario,
                       contenido = comentario.Contenido,
                       username = usuario.NombreUsuario,
                       fecha = utilidades.CalcularHorarioComentario(comentario.Fechayhora),
                       fechaOriginal = comentario.Fechayhora // Guardar la fecha original para ordenar
                   })
             .OrderByDescending(x => x.fechaOriginal) // Ordenar por fecha (descendente)
             .ToListAsync();
            if (comentarios == null)
            {
                return NotFound();
            }
            return StatusCode(StatusCodes.Status200OK, comentarios);
        }

        [HttpPost]
        [Route("NuevoComentario/IdAnime={idAnime:int}&IdUsuario={idUsuario:int}")]
        public async Task<IActionResult> Nuevo(int idAnime, int idUsuario, [FromBody] Dictionary<string, object> datos)
        {
            // Verificar si existen el Anime y Usuario
            var anime = await dbContext.Animes.FindAsync(idAnime);
            var usuario = await dbContext.Clientes.FindAsync(idUsuario);

            if (anime == null || usuario == null)
            {
                return NotFound("Anime o Usuario no encontrados");
            }

            // Verificar que el diccionario contiene el campo "Contenido"
            if (!datos.ContainsKey("Contenido") || string.IsNullOrEmpty(datos["Contenido"]?.ToString()))
            {
                return BadRequest("El campo 'Contenido' es requerido.");
            }

            // Crear una nueva instancia de Comentario
            var nuevoComentario = new Comentario
            {
                IdAnime = idAnime,
                IdUsuario = idUsuario,
                Fechayhora = DateTime.Now,
                Contenido = datos["Contenido"].ToString()
            };

            // Agregar el comentario a la base de datos
            dbContext.Comentarios.Add(nuevoComentario);

            try
            {
                // Guardar los cambios en la base de datos
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al guardar en la base de datos: {ex.Message}");
            }

            return Ok(new { message = "Comentario creado correctamente" });
        }
    }
}
