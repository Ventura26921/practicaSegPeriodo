using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica01.Models;

namespace practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public reservasController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult get()
        {
            var listaReserva = (from res in _equiposContext.reservas
                                join
                                equ in _equiposContext.Equipos on res.equipo_id equals equ.id_equipos
                                join
                                usu in _equiposContext.usuarios on res.usuario_id equals usu.usuario_id
                                join
                                ere in _equiposContext.estados_reserva on res.estado_reserva_id equals ere.estado_res_id
                                select new
                                {
                                    res.reserva_id,
                                    res.equipo_id,
                                    equ.nombre,
                                    res.usuario_id,
                                    nombre_usuario = usu.nombre,
                                    res.fecha_salida,
                                    res.hora_salida,
                                    res.tiempo_reserva,
                                    res.estado_reserva_id,
                                    estado_reserva_descrip = ere.estado,
                                    res.fecha_retorno,
                                    res.hora_retorno,
                                    res.estado
                                }).ToList();
            if (listaReserva.Count == 0)
            {
                return NotFound();
            }
            return Ok(listaReserva);
        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            reservas? ListadoReservas = (from res in _equiposContext.reservas
                                     where res.reserva_id == id
                                     select res).FirstOrDefault();
            if (ListadoReservas == null)
            {
                return NotFound();
            }

            return Ok(ListadoReservas);
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarReserva([FromBody] reservas reserva)
        {
            try
            {
                _equiposContext.reservas.Add(reserva);
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
        public IActionResult actualizar(int id, [FromBody] reservas reservaModificar)
        {
            reservas? reservaActual = (from r in _equiposContext.reservas
                                       where r.reserva_id == id
                                       select r).FirstOrDefault();
            if (reservaActual == null)
            {
                return NotFound();
            }

            reservaActual.equipo_id = reservaModificar.equipo_id;
            reservaActual.usuario_id = reservaModificar.usuario_id;
            reservaActual.fecha_salida = reservaModificar.fecha_salida;
            reservaActual.hora_salida = reservaModificar.hora_salida;
            reservaActual.tiempo_reserva = reservaModificar.tiempo_reserva;
            reservaActual.estado_reserva_id = reservaModificar.estado_reserva_id;
            reservaActual.fecha_retorno = reservaModificar.fecha_retorno;
            reservaActual.hora_retorno = reservaModificar.hora_retorno;

            _equiposContext.Entry(reservaModificar).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(reservaModificar);
        }

        [HttpPut]
        [Route("actualizarEstadoReserva/{id}")]
        public IActionResult actualizarEstadoReserva(int id, char estado)
        {
            reservas? reserva = (from r in _equiposContext.reservas
                                 where r.reserva_id == id
                                 select r).FirstOrDefault();
            if (reserva == null)
            {
                return NotFound();
            }

            reserva.estado = estado;

            _equiposContext.Entry(reserva).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(reserva);
        }
    }
}
