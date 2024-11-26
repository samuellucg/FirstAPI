using System.Threading.Tasks;
using System.Collections.Generic;
using MeuTodo.Data;
using MeuTodo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeuTodo.ViewModels;
using System;

namespace MeuTodo.Controllers
{
    [ApiController] // Define que será o controller da api, pois é a responsável por responder requisições http
    [Route(template: "v1")] // Rota base
    public class ToDoController : ControllerBase
    {
        [HttpGet] // Get Geral
        [Route(template:"todos")]
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context) // Context será a "fonte de consulta aos dados."
        {
            var todos = await context.Todos.AsNoTracking().ToListAsync();
            // AsNoTracking pra otimizar a consulta pois é read only.
            return Ok(todos); // Retorna http 200, caso dê tudo certo.
        }

        [HttpGet] // Get por ID
        [Route(template:"todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context, [FromRoute] int id)
        {
            var todo = await context.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); // Faz a requisição do ID sem interromper processos posteriores.
            return todo == null 
            ? NotFound()  // Retorna 404, se não existir.
            : Ok(todo); // Retorna http 200, se o id existir.
        }

        [HttpPost(template:"Todos")] // Criar nova tarefa.
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context, [FromBody]CreateTodoViewModel model) // FromBody converterá o json enviado para o modelo da classe CreateTodoViewModel
        
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(); // Retorna 400, se o modelo não ser válido. Se não ser enviado um titulo.
            }

            var todo = new Todo
            {
                Date = DateTime.Now,
                Done = false,
                Title = model.Title
            };
            try
            {
                await context.Todos.AddAsync(todo); // Adiciona a tarefa no banco de dados.
                await context.SaveChangesAsync(); // Salva as mudanças no banco de dados.
                return Created(uri:$"v1/todos/{todo.Id}",todo); // Retorna 201 indicando que a tarefa foi criada com sucesso.
            }
            catch (Exception)
            {
                return BadRequest(); // Qualquer exceção retorna 400.
            }
            
        }

        [HttpPut(template: "todos/{id}")] // Put é responsável por atualizar um recurso já existente.
        public async Task<IActionResult> PitAsync(
            [FromServices] AppDbContext context, [FromBody]CreateTodoViewModel model,
            [FromRoute] int id) // FromRoute indica o id que será passado na hora de realizar o put.
        
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(); // Retorna 400, se o modelo não ser válido. Se não ser enviado um titulo.
            }

            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id); // Verifica se o id existe no banco de dados

            if (todo == null)
            {
                return NotFound(); // Retorna 404 se não existir
            }
            
            try
            {
                todo.Title = model.Title; // Irá fazer a atualizaçao do título.



                context.Todos.Update(todo); // Irá atualizar o banco de dados.
                await context.SaveChangesAsync(); // Irá salvar no banco de dados.

                return Ok(todo); // Retorna 200, indicando que a operação foi bem sucedida.
            }
            catch (Exception)
            {
                return BadRequest(); // Retorna 400, em caso de qualquer exceção.
            }
        }

        [HttpDelete(template:"todos/{id}")] // Rota para delete.
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        
        {
            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id); // Verifica se o id existe no banco de dados.

            try
            {
                context.Todos.Remove(todo); // Irá remover todo do banco de dados.
                await context.SaveChangesAsync(); // Irá salvar no banco de dados.
                return Ok(); // Retorna 200, indicando que a operação foi bem sucedida.
            }
            catch (System.Exception)
            {
                
                return BadRequest(); // Em caso de qualquer exceção retorna 400.
            }
            
        }
        
    }
}