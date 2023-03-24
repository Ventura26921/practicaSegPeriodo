using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica01.Models;

namespace practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public usuariosController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult get()
        {
            var listaUsuarios = (from usu in _equiposContext.usuarios
                                 join
                                 car in _equiposContext.Carreras on usu.carrera_id equals car.carrera_id
                                 select new
                                 {
                                     usu.usuario_id,
                                     usu.nombre,
                                     usu.documento,
                                     usu.tipo,
                                     usu.carnet,
                                     usu.carrera_id,
                                     car.nombre_carrera,
                                     usu.estado
                                 }).ToList();
            if (listaUsuarios.Count == 0)
            {
                return NotFound();
            }
            return Ok(listaUsuarios);
        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            usuarios? usuario = (from usu in _equiposContext.usuarios
                                 where usu.usuario_id == id
                                 select usu).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarUsuario([FromBody] usuarios usuario)
        {
            try
            {
                _equiposContext.usuarios.Add(usuario);
                _equiposContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] usuarios usuario)
        {
            usuarios? usuarioActual = (from usu in _equiposContext.usuarios
                                       where usu.usuario_id == id
                                       select usu).FirstOrDefault();
            if (usuarioActual == null)
            {
                return NotFound();
            }

            usuarioActual.nombre = usuario.nombre;
            usuarioActual.documento = usuario.documento;
            usuarioActual.tipo = usuario.tipo;
            usuarioActual.carnet = usuario.carnet;
            usuarioActual.carrera_id = usuario.carrera_id;

            _equiposContext.Entry(usuario).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(usuario);
        }

        [HttpPut]
        [Route("actualizarEstado/{id}")]

        public IActionResult ActualizarEstadoUsuario(int id, char estado)
        {
            usuarios? usuario = (from usu in _equiposContext.usuarios
                                 where usu.usuario_id == id
                                 select usu).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }
            usuario.estado = estado;
            _equiposContext.Entry(usuario).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(usuario);
        }
    }
}
