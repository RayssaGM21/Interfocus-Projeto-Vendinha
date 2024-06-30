using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VendinhasConsole.Entidades;
using VendinhasConsole.Services;


namespace Vendinhas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DividaController : ControllerBase
    {
        private readonly DividaService service;

        public DividaController(DividaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult GetDivida(string busca = null)
        {
            var dados = busca == null ?
                            service.Listar() :
                            service.Listar(busca);
            return Ok(dados);
        }

       

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var dados = service.Retorna(id);
            return Ok(dados);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Divida divida, [FromServices] ClienteService clienteService, [FromQuery] int clienteId)
        {
            var valido = service.Criar(divida, clienteId, clienteService, out List<ValidationResult> erros);
            return valido ? Ok(divida) : UnprocessableEntity(erros);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Divida divida, [FromServices] ClienteService clienteService, [FromQuery] int clienteId)
        {
            var valido = service.Editar(divida, clienteId, clienteService, out List<ValidationResult> erros);
            return valido ? Ok(divida) : UnprocessableEntity(erros);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int id)
        {
            var valido = service.Excluir(id, out List<ValidationResult> erros);
            return valido ? Ok(valido) : UnprocessableEntity(erros);
        }

        /*
        [HttpPut("{id}/pagar")]
        public IActionResult Pagar(int id)
        {
            var sucesso = service.MarcarComoPaga(id);
            if (sucesso)
            {
                return Ok();
            }
            else
            {
                return BadRequest(); // Implemente uma lógica apropriada para tratamento de erros
            }
        }*/
    }
}

