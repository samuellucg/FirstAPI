using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MeuTodo.ViewModels
{
    public class CreateTodoViewModel // Responsável por definir o que o usuário precisa mandar pra API ao criar um novo campo. Modelo a ser seguido pelo usuário.
    {
        [Required] // Indica que o campo não pode ser vazio
        public string Title { get; set; }
    }
}