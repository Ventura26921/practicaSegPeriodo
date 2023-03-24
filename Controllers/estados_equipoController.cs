using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica01.Models;

namespace practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public estados_equipoController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<estados_equipo> listadoEstadoEquipo = (from c in _equiposContexto.estados_equipo select c).ToList();
            if (listadoEstadoEquipo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEstadoEquipo);
        }
        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            estados_equipo? ListadoEstadoEquipos = (from e in _equiposContexto.estados_equipo
                                                    where e.id_estados_equipo == id
                                                    select e).FirstOrDefault();
            if (ListadoEstadoEquipos == null)
            {
                return NotFound();
            }

            return Ok(ListadoEstadoEquipos);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarEstado_equipo([FromBody] estados_equipo estados_equipo)
        {
            try
            {
                _equiposContexto.estados_equipo.Add(estados_equipo);
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
        public IActionResult ActualizarEstado_equipo(int id, [FromBody] estados_equipo estados_Equipo)
        {
            estados_equipo? estado_quipoActual = (from e in _equiposContexto.estados_equipo
                                                  where e.id_estados_equipo == id
                                                  select e).FirstOrDefault();
            if (estados_Equipo == null)
            {
                return NotFound();
            }
            estado_quipoActual.descripcion = estados_Equipo.descripcion;

            _equiposContexto.Entry(estados_Equipo).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(estados_Equipo);
        }

        [HttpPut]
        [Route("actulizarEstado/{id}")]

        public IActionResult actulizarEstados_equiposEstado(int id, char estado)
        {
            estados_equipo? estados_Equipo = (from e in _equiposContexto.estados_equipo
                                              where e.id_estados_equipo == id
                                              select e).FirstOrDefault();
            if (estados_Equipo == null)
            {
                return NotFound();
            }
            estados_Equipo.estado = estado;
            _equiposContexto.Entry(estado).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(estado);
        }

    }
}
