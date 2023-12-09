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
    public class UsuariosController: ControllerBase
    {
        private readonly DominioGenerico<EncuestasContext> _dominioGenerico;
        private readonly IMapper _mapper;
        public UsuariosController(EncuestasContext encuestasContext, IMapper mapper)
        {
            _dominioGenerico = new DominioGenerico<EncuestasContext>(encuestasContext);
            _mapper = mapper;
        }

        [HttpGet("ObtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var resultados = this._dominioGenerico.GetRepositorio<Usuario>().ObtenerTodos();
            var resultadosMapeados = _mapper.Map<IEnumerable<EntidadRegistroUsuario>>(resultados);
            var response = new EntidadRespuesta<IEnumerable<EntidadRegistroUsuario>>() { Entidad = resultadosMapeados, Mensaje = "Usuarios Obtenidos correctamente" };
            return Ok(response);
        }

        [HttpGet("ObtenerUsuario/{id}")]
        public async Task<IActionResult> ObtenerUsuario(int id)
        {
            var resultado = this._dominioGenerico.GetRepositorio<Usuario>().Buscar(id);
            var resultadoMapeado = _mapper.Map<EntidadRegistroUsuario>(resultado);
            var response = new EntidadRespuesta<EntidadRegistroUsuario>() { Entidad = resultadoMapeado, Mensaje = "Usuario Obtenido correctamente" };
            return Ok(response);
        }

        [HttpPost("AgregarUsuario")]
        public async Task<IActionResult> AgregarUsuario(EntidadRegistroUsuario usuario)
        {
            var valor = _mapper.Map<Usuario>(usuario);
            this._dominioGenerico.GetRepositorio<Usuario>().Insertar(valor);
            this._dominioGenerico.GuardarTransacciones();
            return Ok("Insertado Correctamente");
        }

        [HttpPut("ActualizarUsuario")]
        public async Task<IActionResult> ActualizarUsuario(EntidadRegistroUsuario usuario)
        {
            var valor = _mapper.Map<Usuario>(usuario);
            this._dominioGenerico.GetRepositorio<Usuario>().Actualizar(valor);
            this._dominioGenerico.GuardarTransacciones();
            return Ok("Actualizado Correctamente");
        }

        [HttpDelete("EliminarUsuario/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            this._dominioGenerico.GetRepositorio<Usuario>().Eliminar(id);
            this._dominioGenerico.GuardarTransacciones();
            return Ok("Eliminado Correctamente");
        }
    }
}
