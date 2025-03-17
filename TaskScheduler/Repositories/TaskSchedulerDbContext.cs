using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using TaskScheduler.Models;
using Task = TaskScheduler.Models.Task;

namespace TaskScheduler.Repositories;

public class TaskSchedulerDbContext(DbContextOptions<TaskSchedulerDbContext> options) : DbContext(options)
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Sop> Sops => Set<Sop>();
    public DbSet<Task> Tasks => Set<Task>();
    
    public DbSet<EmployeeTask> EmployeeTasks => Set<EmployeeTask>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeTask>()
            .HasKey(et => new { et.TaskId, et.EmployeeId }); // Composite Key

        modelBuilder.Entity<EmployeeTask>()
            .HasOne(et => et.Employee)
            .WithMany(e => e.EmployeeTasks)
            .HasForeignKey(et => et.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmployeeTask>()
            .HasOne(et => et.Task)
            .WithMany(t => t.EmployeeTasks)
            .HasForeignKey(et => et.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Sop>()
            .HasOne(s => s.Department)
            .WithMany(d => d.Sops)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Task>()
            .HasOne(t => t.Sop)
            .WithMany(s => s.Tasks)
            .HasForeignKey(t => t.SopId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}