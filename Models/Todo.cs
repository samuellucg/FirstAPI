using System;

namespace MeuTodo.Models
{

    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        // Esse é o campo required no view model que indica que esse campo não pode ser vazio. Será enviado pelo usuário, diferente das outras prop.
        public bool Done { get; set; }
        public DateTime Date { get; set; } = DateTime.Now; // Caso a data não seja especificada, será usado a data de quando ele enviou o dado pra API.
    }
}