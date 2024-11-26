using System;
using MeuTodo.Models;
using Microsoft.EntityFrameworkCore;


namespace MeuTodo.Data
{
	public class AppDbContext : DbContext // Responsável pelo CRUD.
	{
		public DbSet<Todo> Todos { get; set; } 

        protected override void OnConfiguring(
			DbContextOptionsBuilder optionsBuilder)
        	=> optionsBuilder.UseSqlite(connectionString:"DataSource=app.db;Cache=Shared"); 
		// Datasource é responsável por indicar em que banco de dados ele deve salvar, e cache shared otimiza a conexão pois varias conexões compartilha do mesmo cache.
        
    }
}
