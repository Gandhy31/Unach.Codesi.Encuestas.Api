using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Unach.Codesi.Encuestas.Aplicacion.DTOs;
using Unach.Codesi.Encuestas.Dominio.Core;
using Unach.Codesi.Encuestas.Persistencia.Core.Models;

namespace Unach.Codesi.Encuestas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncuestasController: ControllerBase
    {
        private readonly DominioGenerico<EncuestasContext> _dominioGenerico;
        private readonly IMapper _mapper;
        public EncuestasController(EncuestasContext encuestasContext, IMapper mapper)
        {
            _dominioGenerico = new DominioGenerico<EncuestasContext>(encuestasContext);
            _mapper = mapper;
        }

        [HttpGet("prueba")]
        public async Task<IActionResult> prueba()
        {
            return Ok("listo");
        }

        [HttpGet("ObtenerEncuestas")]
        public async Task<IActionResult> ObtenerEncuestas()
        {
            var resultados = this._dominioGenerico.GetRepositorio<Encuesta>().ObtenerTodos();
            var resultadosMapeados = _mapper.Map<IEnumerable<EntidadRegistroEncuesta>>(resultados);
            var response = new EntidadRespuesta<IEnumerable<EntidadRegistroEncuesta>>() { Entidad = resultadosMapeados, Mensaje = "Encuestas Obtenidas correctamente" };
            return Ok(response);
        }

        [HttpGet("ObtenerEncuesta/{id}")]
        public async Task<IActionResult> ObtenerEncuesta(int id)
        {
            var resultado = this._dominioGenerico.GetRepositorio<Encuesta>().Buscar(id);
            var resultadoMapeado = _mapper.Map<EntidadRegistroEncuesta>(resultado);
            var response = new EntidadRespuesta<EntidadRegistroEncuesta>() { Entidad = resultadoMapeado, Mensaje = "Encuesta Obtenida correctamente" };
            return Ok(response);
        }

        [HttpPost("AgregarEncuesta")]
        public async Task<IActionResult> AgregarEncuesta(EntidadRegistroEncuesta encuesta)
        {
            var valor = _mapper.Map<Encuesta>(encuesta);
            this._dominioGenerico.GetRepositorio<Encuesta>().Insertar(valor);
            this._dominioGenerico.GuardarTransacciones();
            return Ok("Insertado Correctamente");
        }

        [HttpPut("ActualizarEncuesta")]
        public async Task<IActionResult> ActualizarResultado(EntidadRegistroEncuesta encuesta)
        {
            var valor = _mapper.Map<Encuesta>(encuesta);
            this._dominioGenerico.GetRepositorio<Encuesta>().Actualizar(valor);
            this._dominioGenerico.GuardarTransacciones();
            return Ok("Actualizado Correctamente");
        }

        [HttpDelete("EliminarEncuesta/{id}")]
        public async Task<IActionResult> EliminarEncuesta(int id)
        {
            var resultados = this._dominioGenerico.GetRepositorio<Resultado>().BuscarPor((x => x.IdEncuesta == id));
            foreach (var resultado in resultados) 
            {
                this._dominioGenerico.GetRepositorio<Resultado>().Eliminar(resultado.Id);
            }
            this._dominioGenerico.GetRepositorio<Encuesta>().Eliminar(id);
            this._dominioGenerico.GuardarTransacciones();
            return Ok("Eliminado Correctamente");
        }
    }
}
