
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using practica01.Models;


namespace practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public equiposController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipos = (from equ in _equiposContexto.Equipos
                                  join esq in _equiposContexto.estados_equipo on
                                  equ.estado_equipo_id equals esq.id_estados_equipo
                                  join teq in _equiposContexto.tipo_equipo on
                                  equ.tipo_equipo_id equals teq.id_tipo_equipo
                                  join mar in _equiposContexto.marcas on
                                  equ.marca_id equals mar.id_marcas
                                  select new
                                  {
                                      equ.id_equipos,
                                      equ.nombre,
                                      equ.descripcion,
                                      equ.tipo_equipo_id,
                                      equipo_descripcion = teq.descripcion,
                                      equ.marca_id,
                                      mar.nombre_marca,
                                      equ.modelo,
                                      equ.anio_compra,
                                      equ.costo,
                                      equ.vida_util,
                                      equ.estado_equipo_id,
                                      esq.id_estados_equipo,
                                      descripcion_estado = esq.descripcion,
                                      equ.estado
                                  }).ToList();
            if (listadoEquipos.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);
        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            equipos? equipo = (from e in _equiposContexto.Equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }
        [HttpGet]
        [Route("find/{filtro}")]

        public IActionResult FinfByDescription(string filtro)
        {
            List<equipos> equipos = (from e in _equiposContexto.Equipos
                                     where e.descripcion.Contains(filtro)
                                     select e).ToList();
            if (equipos == null)
            {
                return NotFound();
            }

            return Ok(equipos);
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarEquipo([FromBody] equipos equipos)
        {
            try
            {
                _equiposContexto.Equipos.Add(equipos);
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

        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            equipos? equipoActual = (from e in _equiposContexto.Equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }
            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.marca_id = equipoModificar.marca_id;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(equipoModificar);
        }

        [HttpPut]
        [Route("actulizarEstado/{id}")]

        public IActionResult EliminarEquipo(int id, string estado)
        {
            equipos? equipo = (from e in _equiposContexto.Equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            equipo.estado = estado;
            _equiposContexto.Entry(equipo).State= EntityState.Modified;        
            _equiposContexto.SaveChanges();
            return Ok(equipo);
        }
    }
}
