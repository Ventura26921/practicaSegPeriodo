using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica01.Models;

namespace practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoEquipoController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public tipoEquipoController(equiposContext equiposContext)
        {
            _equiposContexto = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult get()
        {
            List<tipo_equipo> listaTipoEquipo = (from teq in _equiposContexto.tipo_equipo
                                                 select teq).ToList();
            if (listaTipoEquipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(listaTipoEquipo);
        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            tipo_equipo? tipoEquipo = (from teq in _equiposContexto.tipo_equipo
                                         where teq.id_tipo_equipo == id
                                         select teq).FirstOrDefault();
            if (tipoEquipo == null)
            {
                return NotFound();
            }

            return Ok(tipoEquipo);
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarTipoEquipo([FromBody] tipo_equipo tipoE)
        {
            try
            {
                _equiposContexto.tipo_equipo.Add(tipoE);
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
        public IActionResult actualizar(int id, [FromBody] tipo_equipo equipoModificar)
        {
            tipo_equipo? tEquipo = (from te in _equiposContexto.tipo_equipo
                                    where te.id_tipo_equipo == id
                                    select te).FirstOrDefault();
            if (tEquipo == null)
            {
                return NotFound();
            }

            tEquipo.descripcion = equipoModificar.descripcion;

            _equiposContexto.Entry(equipoModificar).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(equipoModificar);
        }

        [HttpPut]
        [Route("actualizarEstadoEquipo/{id}")]

        public IActionResult ActualizarEstadoTEquipo(int id, char estado)
        {
            tipo_equipo? tEquipo = (from teq in _equiposContexto.tipo_equipo
                                    where teq.id_tipo_equipo == id
                                    select teq).FirstOrDefault();
            if (tEquipo == null)
            {
                return NotFound();
            }
            tEquipo.estado = estado;
            _equiposContexto.Entry(tEquipo).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(tEquipo);
        }
    }
}
