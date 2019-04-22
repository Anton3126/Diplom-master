using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class SchoolContext : IdentityDbContext<User>
    {

        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTask>()
            .HasKey(t => new { t.UserId, t.TaskId });

            modelBuilder.Entity<Firm>().HasData(
            new Firm[]
            {
                 new Firm { FirmID = 1, FirmName = "ОАО Воробей", FirmAddress = "Казань, Колотушкина 13"},
                 new Firm { FirmID = 2, FirmName = "ООО Волна", FirmAddress = "Казань, Пушкина 43"},
                 new Firm { FirmID = 3, FirmName = "ИП Петров", FirmAddress = "Казань, Бызова 32"}
            });
            modelBuilder.Entity<Wallet>().HasData(
            new Wallet[]
            {
                 new Wallet { WalletID = 1, WalletName = "Налоговая", Currency = "Рубль"},
                 new Wallet { WalletID = 2, WalletName = "Арендодатель", Currency = "Рубль"},
                 new Wallet { WalletID = 3, WalletName = "Управляющая компания(ЖКХ)", Currency = "Рубль"},
                 new Wallet { WalletID = 4, FirmID = 1, Currency = "Рубль"},
                 new Wallet { WalletID = 5, FirmID = 1, Currency = "Доллар"},
                 new Wallet { WalletID = 6, FirmID = 1, Currency = "Евро"},
                 new Wallet { WalletID = 7, FirmID = 2, Currency = "Рубль"},
                 new Wallet { WalletID = 8, FirmID = 2, Currency = "Доллар"},
                 new Wallet { WalletID = 9, FirmID = 2, Currency = "Евро"},
                 new Wallet { WalletID = 10, FirmID = 3, Currency = "Рубль"},
                 new Wallet { WalletID = 11, FirmID = 3, Currency = "Доллар"},
                 new Wallet { WalletID = 12, FirmID = 3, Currency = "Евро"}
            });
            //Таблица Projects
            modelBuilder.Entity<Project>().HasData(
           new Project[]
           {
                new Project { Id=1, Name="Проект 1", Description="АИС управления персоналом",
                            Date = new DateTime(2008, 5, 1, 10, 40, 52), Cost = 90000},
                new Project { Id=2, Name="Проект 3", Description="АИС управления финансами",
                           Date = new DateTime(2008, 10, 10, 7, 30, 52), Cost = 90000},
                new Project { Id=3, Name="Проект 2", Description="АИС управления call-центром",
                            Date = new DateTime(2009, 11, 3, 8, 35, 52), Cost = 90000}
           });

            //Таблица Task
            modelBuilder.Entity<Models.Task>().HasData(
           new Models.Task[]
           {
                new Models.Task { Id=1, Name="Задача 1", Description="Спроектировать облачное хранилище данных", Date = new DateTime(2010, 2, 10, 10, 40, 52), ProjectId = 1},
                new Models.Task { Id=2, Name="Задача 2", Description="Разработать систему авторизации", Date = new DateTime(2010, 12, 10, 17, 30, 52), ProjectId = 2},
                new Models.Task { Id=3, Name="Задача 3", Description="Сверстать сайт", Date = new DateTime(2009, 12, 13, 10, 37, 52), ProjectId = 3}
           });
        }

        public DbSet<WebApplication1.Models.Firm> Firm { get; set; }
        public DbSet<WebApplication1.Models.Wallet> Wallet { get; set; }
        public DbSet<WebApplication1.Models.Project> Projects { get; set; }
        public DbSet<WebApplication1.Models.Task> Tasks { get; set; }
        public DbSet<WebApplication1.Models.EmployeeType> EmployeeType { get; set; }
        public DbSet<WebApplication1.Models.Invoice> Invoice { get; set; }
        public DbSet<WebApplication1.Models.Transaction> Transaction { get; set; }
    }

}
