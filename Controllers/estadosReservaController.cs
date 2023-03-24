using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica01.Models;

namespace practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estadosReservaController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public estadosReservaController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<estados_reserva> listadoEstadosReserva = (from c in _equiposContexto.estados_reserva select c).ToList();
            if (listadoEstadosReserva.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEstadosReserva);
        }
        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            estados_reserva? listadoEstadoReserva = (from e in _equiposContexto.estados_reserva
                                                     where e.estado_res_id == id
                                                     select e).FirstOrDefault();
            if (listadoEstadoReserva == null)
            {
                return NotFound();
            }

            return Ok(listadoEstadoReserva);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarEstado_reserva([FromBody] estados_reserva estados_reserva)
        {
            try
            {
                _equiposContexto.estados_reserva.Add(estados_reserva);
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
        public IActionResult ActualizarEstado_reserva(int id, [FromBody] estados_equipo estados_reserva)
        {
            estados_reserva? estado_reservaActual = (from e in _equiposContexto.estados_reserva
                                                     where estados_reserva.estado == id
                                                     select e).FirstOrDefault();
            if (estados_reserva == null)
            {
                return NotFound();
            }
            estados_reserva.estado = estados_reserva.estado;

            _equiposContexto.Entry(estados_reserva).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(estados_reserva);
        }

        [HttpPut]
        [Route("actulizarEstado/{id}")]

        public IActionResult actulizarEstados_reservaEstado(int id, string estado)
        {
            estados_reserva? estados_reserva = (from e in _equiposContexto.estados_reserva
                                              where e.estado_res_id == id
                                              select e).FirstOrDefault();
            if (estados_reserva == null)
            {
                return NotFound();
            }
            estados_reserva.estado = estado ;
            _equiposContexto.Entry(estados_reserva).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(estados_reserva);
        }
    }
}
