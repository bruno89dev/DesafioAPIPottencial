using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context) {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id) {
            var tarefaPorId = _context.Tarefas.Where(t => t.Id == id).ToList();
            if (tarefaPorId.Count == 0) {
                Response.StatusCode = 404;
                return new ObjectResult(new { info = "Tarefa não encontrada!" });
            } else {
                return Ok(tarefaPorId);
            }
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos() {
            var listaTarefas = _context.Tarefas.ToList();
            if (listaTarefas.Count == 0) {
                Response.StatusCode = 404;
                return new ObjectResult(new { info = "Nenhuma tarefa encontrada!" });
            } else {
                return Ok(listaTarefas);
            }
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo) {
            var tarefaPorTitulo = _context.Tarefas.Where(t => t.Titulo.Contains(titulo)).ToList();
            if (tarefaPorTitulo.Count == 0) {
                Response.StatusCode = 404;
                return new ObjectResult(new { info = "Tarefa não encontrada!" });
            } else {
                return Ok(tarefaPorTitulo);
            }
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data) {
            var tarefaPorData = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();
            if (tarefaPorData.Count == 0) {
                Response.StatusCode = 404;
                return new ObjectResult(new { info = "Nenhuma tarefa encontrada na data selecionada!" });
            } else {
                return Ok(tarefaPorData);
            }
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status) {
            var tarefaPorStatus = _context.Tarefas.Where(x => x.Status == status).ToList();
            if (tarefaPorStatus.Count == 0) {
                Response.StatusCode = 404;
                return new ObjectResult(new { info = "Nenhuma tarefa encontrada com o status selecionado!" });
            } else {
                return Ok(tarefaPorStatus);
            }
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa) {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa) {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;
            _context.SaveChanges();
            new ObjectResult(new { info = "Tarefa atualizada!"});
            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id) {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return Ok("Tarefa excluída com sucesso!");
        }
    }
}
