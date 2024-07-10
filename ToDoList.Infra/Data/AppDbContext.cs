using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.ToDoList.Entities;
using ToDoList.Infra.ToDoList.Mappings;

namespace ToDoList.Infra.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<ToDo> ToDos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new ToDoMap());
    }
}