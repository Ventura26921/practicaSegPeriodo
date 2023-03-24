using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica01.Models;

namespace practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public carrerasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoCarreras = (from car in _equiposContexto.Carreras
                                 join
                                fac in _equiposContexto.facultades on car.facultad_id equals fac.facultad_id
                                 select new
                                 {
                                     car.carrera_id,
                                     car.nombre_carrera,
                                     car.facultad_id,
                                     fac.nombre_facultad,
                                     car.estado
                                 }).ToList();
            if (listadoCarreras.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoCarreras);
        }
        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            carreras? ListadoCarreras = (from e in _equiposContexto.Carreras
                               where e.carrera_id == id
                               select e).FirstOrDefault();
            if (ListadoCarreras == null)
            {
                return NotFound();
            }

            return Ok(ListadoCarreras);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarCarrera([FromBody] carreras carreras)
        {
            try
            {
                _equiposContexto.Carreras.Add(carreras);
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

        public IActionResult ActualizarCarrera(int id, [FromBody] carreras carreras)
        {
            carreras? carreraActual = (from e in _equiposContexto.Carreras
                                     where e.carrera_id == id
                                     select e).FirstOrDefault();
            if (carreraActual == null)
            {
                return NotFound();
            }
            carreraActual.nombre_carrera = carreras.nombre_carrera;
            carreraActual.facultad_id = carreras.facultad_id;

            _equiposContexto.Entry(carreraActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(carreras);
        }
        [HttpPut]
        [Route("actulizarEstado/{id}")]

        public IActionResult actualizaCarrera(int id, char estado)
        {
            carreras? carreras = (from e in _equiposContexto.Carreras
                               where e.carrera_id == id
                               select e).FirstOrDefault();
            if (carreras == null)
            {
                return NotFound();
            }
            carreras.estado = estado;
            _equiposContexto.Entry(carreras).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(carreras);
        }
    }
}
