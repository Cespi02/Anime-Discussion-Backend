using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;
using WebApplication1.Custom;
using WebApplication1.Models;
using WebApplication1.Models.DTOS;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly CrudPruebaContext dbContext;
        private readonly Utilidades utilidades;

        public ClienteController(CrudPruebaContext _dbContext, Utilidades _utilidades)
        {
            this.dbContext = _dbContext;
            this.utilidades = _utilidades; 
        }

        [HttpPut]
        [Route("CambiarContra/{id:int}")]
        public async Task<IActionResult> CambiarContra(int id, [FromBody] CambiarContrasenaDto datos)
        {
            var usuario = await dbContext.Clientes.FindAsync(id);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            if (string.IsNullOrEmpty(datos.contrasenia))
            {
                return BadRequest("El campo 'Contrasenia' es requerido.");
            }

            // Encriptar la contraseña antes de almacenarla
            var contraEncriptada = utilidades.encriptarSHA256(datos.contrasenia);
            usuario.Contrasenia = contraEncriptada;

            await dbContext.SaveChangesAsync();

            return Ok(new { isSucess = true, estado = "Contraseña actualizada" });
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<IActionResult> Registro(UsuarioDTO objeto)
        {
            var modeloUsuario = new Cliente
            {
                Nombre = objeto.Nombre,
                Email = objeto.Email,
                Contrasenia = utilidades.encriptarSHA256(objeto.Contrasenia),
                NombreUsuario = objeto.NombreUsuario
            };
            await dbContext.Clientes.AddAsync(modeloUsuario);
            await dbContext.SaveChangesAsync();

            if(modeloUsuario.IdCliente != 0)
            return StatusCode(StatusCodes.Status200OK, new { isSucess = true });
            else
            return StatusCode(StatusCodes.Status200OK, new { isSucess = false });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            var usuarioEncontrado = await dbContext.Clientes.Where(u => u.Email == objeto.Email && u.Contrasenia == utilidades.encriptarSHA256(objeto.Contrasenia)).FirstOrDefaultAsync();
         
            if (usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status404NotFound, new { isSucess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSucess = true, token = utilidades.generarJWT(usuarioEncontrado) });
        }
    }
}
