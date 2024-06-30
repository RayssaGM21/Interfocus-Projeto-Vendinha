using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using VendinhasConsole.Entidades;
using VendinhasConsole.Services;


namespace Vendinhas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService clienteService;
        private readonly IWebHostEnvironment env;

        public ClienteController(ClienteService clienteService, IWebHostEnvironment env)
        {
            this.clienteService = clienteService;
            this.env = env;                         
        }

        [HttpGet]
        public IActionResult Listar(int id, string pesquisa = null , int page = 0, int pageSize = 0)
        {
            var clientes = string.IsNullOrEmpty(pesquisa) ?
                clienteService.Listar(page, pageSize) :
                clienteService.Listar(pesquisa, page, pageSize);
            return Ok(clientes);
        }



        /*
        [HttpGet]
        public IActionResult ListarTodos()
        {
            var clientes = clienteService.ListarTodos();
            return Ok(clientes);
        
        }*/

        
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cliente = clienteService.Retorna(id);
            return Ok(cliente);
        }

        [HttpPost]
        public IActionResult Criar([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                return BadRequest(ModelState);
            }

            var sucesso = clienteService.Criar(cliente, out List<ValidationResult> erros);
            if (sucesso)
            {
                return Ok(cliente);
            }
            else
            {
                return UnprocessableEntity(erros);
            }
        }

        [HttpPut]
        public IActionResult Editar(int id, [FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                return BadRequest(ModelState);
            }

            var sucesso = clienteService.Editar(cliente, out List<ValidationResult> erros);
            if (sucesso)
            {
                return Ok(cliente);
            }
            else if (erros.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return UnprocessableEntity(erros);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Remover(int id)
        {
            var cliente = clienteService.Excluir(id, out List<ValidationResult> erros);
            if (cliente == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cliente);
            }
            
        }
    }
}
