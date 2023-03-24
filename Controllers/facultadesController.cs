using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica01.Models;

namespace practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public facultadesController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<facultades> listadoFacultades = (from c in _equiposContexto.facultades select c).ToList();
            if (listadoFacultades.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoFacultades);
        }
        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            facultades? listadoFacultades = (from e in _equiposContexto.facultades
                                                     where e.facultad_id == id
                                                     select e).FirstOrDefault();
            if (listadoFacultades == null)
            {
                return NotFound();
            }

            return Ok(listadoFacultades);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarFacultades([FromBody] facultades facultades)
        {
            try
            {
                _equiposContexto.facultades.Add(facultades);
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
        public IActionResult ActualizarFacultades(int id, [FromBody] facultades facultades)
        {
            facultades? facultad = (from e in _equiposContexto.facultades
                                                     where facultades.facultad_id == id
                                                     select e).FirstOrDefault();
            if (facultades == null)
            {
                return NotFound();
            }
            facultades.facultad_id = facultades.facultad_id;

            _equiposContexto.Entry(facultades).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(facultades);
        }

        [HttpPut]
        [Route("actulizarEstado/{id}")]

        public IActionResult actulizarFacultadEstado(int id, char estado)
        {
            facultades? facultades = (from e in _equiposContexto.facultades
                                                where e.facultad_id == id
                                                select e).FirstOrDefault();
            if (facultades == null)
            {
                return NotFound();
            }
            facultades.estado = estado;
            _equiposContexto.Entry(facultades).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(facultades);
        }
    }
}
