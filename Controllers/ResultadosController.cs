using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Unach.Codesi.Encuestas.Aplicacion.DTOs;
using Unach.Codesi.Encuestas.Dominio.Core;
using Unach.Codesi.Encuestas.Persistencia.Core.Models;

namespace Unach.Codesi.Encuestas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadosController: ControllerBase
    {
        private readonly DominioGenerico<EncuestasContext> _dominioGenerico;
        private readonly IMapper _mapper;
        public ResultadosController(EncuestasContext encuestasContext, IMapper mapper)
        {
            _dominioGenerico = new DominioGenerico<EncuestasContext>(encuestasContext);
            _mapper = mapper;
        }

        [HttpGet("prueba")]
        public async Task<IActionResult> prueba()
        {
            return Ok("listo");
        }

        [HttpGet("ObtenerResultados")]
        public async Task<IActionResult> ObtenerResultados()
        {
            var resultados = this._dominioGenerico.GetRepositorio<Resultado>().ObtenerTodos();
            var resultadosMapeados = _mapper.Map<IEnumerable<EntidadRegistroResultado>>(resultados);
            var response = new EntidadRespuesta<IEnumerable<EntidadRegistroResultado>>() { Entidad = resultadosMapeados, Mensaje = "Resultados Obtenidos correctamente" };
            return Ok(response);
        }

        [HttpGet("ObtenerResultado/{id}")]
        public async Task<IActionResult> ObtenerResultado(int id)
        {
            var resultado = this._dominioGenerico.GetRepositorio<Resultado>().Buscar(id);
            var resultadoMapeado = _mapper.Map<EntidadRegistroResultado>(resultado);
            var response = new EntidadRespuesta<EntidadRegistroResultado>() { Entidad = resultadoMapeado, Mensaje = "Resultado Obtenida correctamente" };
            return Ok(response);
        }

        [HttpGet("ObtenerListaResultados/{id}")]
        public async Task<IActionResult> ObtenerListaResultados(int id)
        {
            var resultados = this._dominioGenerico.GetRepositorio<Resultado>().BuscarPor((x=>x.IdEncuesta==id));
            var resultadosMapeados = _mapper.Map<IEnumerable<EntidadRegistroResultado>>(resultados);
            var response = new EntidadRespuesta<IEnumerable<EntidadRegistroResultado>>() { Entidad = resultadosMapeados, Mensaje = "Resultado Obtenida correctamente" };
            return Ok(response);
        }

        [HttpGet("ObtenerResultadoUsuario/{idE}/{idU}")]
        public async Task<IActionResult> ObtenerResultadoUsuario(int idE, int idU)
        {
            var respuestaUsuario = false;
            var resultados = this._dominioGenerico.GetRepositorio<Resultado>().BuscarPor((x => x.IdEncuesta == idE && x.IdUsuario == idU));
            if(resultados.Count() > 0)
            {
                respuestaUsuario = true;
            }
            //var resultadosMapeados = _mapper.Map<IEnumerable<EntidadRegistroResultado>>(resultados);
            var response = new EntidadRespuesta<bool>() {Entidad = respuestaUsuario, Mensaje= "Resultado Obtenido Correctamente" };
            return Ok(response);
        }

        [HttpPost("AgregarResultado")]
        public async Task<IActionResult> AgregarResultado(EntidadRegistroResultado resultado)
        {
            var valor = _mapper.Map<Resultado>(resultado);
            this._dominioGenerico.GetRepositorio<Resultado>().Insertar(valor);
            this._dominioGenerico.GuardarTransacciones();
            return Ok("Insertado Correctamente");
        }

        [HttpPut("ActualizarResultado")]
        public async Task<IActionResult> ActualizarResultado(EntidadRegistroResultado resultado)
        {
            var valor = _mapper.Map<Resultado>(resultado);
            this._dominioGenerico.GetRepositorio<Resultado>().Actualizar(valor);
            this._dominioGenerico.GuardarTransacciones();
            return Ok("Actualizado Correctamente");
        }

        [HttpDelete("EliminarResultado/{id}")]
        public async Task<IActionResult> EliminarResultado(int id)
        {
            this._dominioGenerico.GetRepositorio<Resultado>().Eliminar(id);
            this._dominioGenerico.GuardarTransacciones();
            return Ok("Eliminado Correctamente");
        }
    }
}
