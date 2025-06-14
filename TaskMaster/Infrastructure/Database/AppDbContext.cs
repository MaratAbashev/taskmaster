﻿using Domain.Entities;
using Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<ToDoTask> Tasks { get; set; }
    public DbSet<TaskBoard> TaskBoards { get; set; }
    public DbSet<TelegramGroup> TelegramGroups { get; set; }
    public DbSet<BoardUser> BoardUsers { get; set; }
    public DbSet<TaskWorker> TaskWorkers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BoardUserConfiguration());
        modelBuilder.ApplyConfiguration(new TaskBoardConfiguration());
        modelBuilder.ApplyConfiguration(new TaskWorkerConfiguration());
        modelBuilder.ApplyConfiguration(new TelegramGroupConfiguration());
        modelBuilder.ApplyConfiguration(new ToDoTaskConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}