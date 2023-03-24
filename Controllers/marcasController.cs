using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica01.Models;

namespace practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public marcasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<marcas> listadoMarcas = (from m in _equiposContexto.marcas select m).ToList();
            if (listadoMarcas.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoMarcas);
        }
        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            marcas? ListadoMarcas = (from m in _equiposContexto.marcas
                                         where m.id_marcas == id
                                         select m).FirstOrDefault();
            if (ListadoMarcas == null)
            {
                return NotFound();
            }

            return Ok(ListadoMarcas);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarCarrera([FromBody] marcas marcas)
        {
            try
            {
                _equiposContexto.marcas.Add(marcas);
                _equiposContexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarCarrera(int id, [FromBody] marcas marcas)
        {
            marcas? marcaActual = (from m in _equiposContexto.marcas
                                       where m.id_marcas == id
                                       select m).FirstOrDefault();
            if (marcaActual == null)
            {
                return NotFound();
            }
            marcaActual.nombre_marca= marcas.nombre_marca;
            _equiposContexto.Entry(marcaActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(marcas);
        }
        [HttpPut]
        [Route("actulizarEstado/{id}")]

        public IActionResult actualizaCarrera(int id, char estado)
        {
            marcas? marca = (from m in _equiposContexto.marcas
                                  where m.id_marcas == id
                                  select m).FirstOrDefault();
            if (marca == null)
            {
                return NotFound();
            }
            marca.estados = estado;
            _equiposContexto.Entry(marca).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(marca);
        }
    }
}
