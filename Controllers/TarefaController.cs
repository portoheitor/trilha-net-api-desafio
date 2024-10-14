using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Buscar o Id no banco utilizando o EF
            // Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            // caso contrário retornar OK com a tarefa encontrada

            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null) return NotFound();

            return Ok(tarefaBanco);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Buscar todas as tarefas no banco utilizando o EF
            var tarefaBanco = _context.Tarefas;

            if (tarefaBanco == null) return BadRequest();

            return Ok(tarefaBanco);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            //  Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            //  Usar como exemplo o endpoint ObterPorData
            var tarefaBanco = _context.Tarefas.Where( x => x.Titulo.Contains(titulo));

            if (tarefaBanco == null)
            {
                return NotFound();
            };

            return Ok(tarefaBanco);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Usar como exemplo o endpoint ObterPorData
            var tarefaBanco = _context.Tarefas.Where(x => x.Status == status);
            return Ok(tarefaBanco);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });


            //Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            //  Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            //  Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
