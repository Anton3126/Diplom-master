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

            modelBuilder.Entity<Models.Task>()
                .Property(t => t.Hours)
                .HasDefaultValue(0);

            //Таблица Firms
            modelBuilder.Entity<Firm>().HasData(
            new Firm[]
            {
                 new Firm { FirmID = 1, FirmName = "ОАО Воробей", FirmAddress = "Казань, Колотушкина 13"},
                 new Firm { FirmID = 2, FirmName = "ООО Волна", FirmAddress = "Казань, Пушкина 43"},
                 new Firm { FirmID = 3, FirmName = "ИП Петров", FirmAddress = "Казань, Бызова 32"}
            });

            //Таблица Wallets
            modelBuilder.Entity<Wallet>().HasData(
            new Wallet[]
            {
                 new Wallet { WalletID = 1, WalletName = "Налоговая Рубль", Currency = "Рубль"},
                 new Wallet { WalletID = 2, WalletName = "Арендодатель Рубль", Currency = "Рубль"},
                 new Wallet { WalletID = 3, WalletName = "Управляющая компания(ЖКХ) Рубль", Currency = "Рубль"},
                 new Wallet { WalletID = 4, WalletName = "ОАО Воробей Рубль", FirmID = 1, Currency = "Рубль"},
                 new Wallet { WalletID = 5, WalletName = "ОАО Воробей Доллар", FirmID = 1, Currency = "Доллар"},
                 new Wallet { WalletID = 6, WalletName = "ОАО Воробей Евро", FirmID = 1, Currency = "Евро"},
                 new Wallet { WalletID = 7, WalletName = "ООО Волна Рубль", FirmID = 2, Currency = "Рубль"},
                 new Wallet { WalletID = 8, WalletName = "ООО Волна Доллар", FirmID = 2, Currency = "Доллар"},
                 new Wallet { WalletID = 9, WalletName = "ООО Волна Евро", FirmID = 2, Currency = "Евро"},
                 new Wallet { WalletID = 10, WalletName = "ИП Петров Рубль", FirmID = 3, Currency = "Рубль"},
                 new Wallet { WalletID = 11, WalletName = "ИП Петров Доллар", FirmID = 3, Currency = "Доллар"},
                 new Wallet { WalletID = 12, WalletName = "ИП Петров Евро", FirmID = 3, Currency = "Евро"}
            });

            //Таблица Projects
            modelBuilder.Entity<Project>().HasData(
           new Project[]
           {
                new Project { Id=1, Name="GLOBUS", Description="АИС управления персоналом",
                            Date = new DateTime(2008, 5, 1, 10, 40, 52), Cost = 100000, Status = "Активный"},
                new Project { Id=2, Name="Электронный инспектор", Description="АИС управления финансами",
                           Date = new DateTime(2008, 10, 10, 7, 30, 52), Cost = 50000, Status = "Активный"},
                new Project { Id=3, Name="Единое окно", Description="АИС управления call-центром",
                            Date = new DateTime(2009, 11, 3, 8, 35, 52), Cost = 93000, Status = "Активный"},
                new Project { Id=4, Name="Онлайн трекер", Description="Веб-приложения для отслеживания почтовых посылок",
                            Date = new DateTime(2009, 11, 3, 8, 35, 52), Cost = 85000, Status = "Завершенный"}
           });

            //Таблица Task
            modelBuilder.Entity<Models.Task>().HasData(
           new Models.Task[]
           {
                new Models.Task { Id=1, Name="Задача 1", Description="Спроектировать облачное хранилище данных", Date = new DateTime(2010, 2, 10, 10, 40, 52), ProjectId = 1},
                new Models.Task { Id=2, Name="Задача 2", Description="Разработать систему авторизации", Date = new DateTime(2010, 12, 10, 17, 30, 52), ProjectId = 2},
                new Models.Task { Id=3, Name="Задача 3", Description="Сверстать сайт", Date = new DateTime(2009, 12, 13, 10, 37, 52), ProjectId = 3}
           });

            //Таблица Invoices
            modelBuilder.Entity<Invoice>().HasData(
           new Invoice[]
           {
                new Invoice { InvoiceID=1, ClientName="Игорь Щербаков", DateCreate = new DateTime(2018, 10, 3, 10, 40, 52), AmountWait = 50000, AmountReal = 60000, DatePaymentWait =  new DateTime(2019, 1, 15, 10, 20, 32), DatePaymentReal = new DateTime(2019, 2, 2, 10, 1, 31), ProjectID = 1, Pay = "Оплачен"},
                new Invoice { InvoiceID=2, ClientName="Ян Кузнецов",  DateCreate = DateTime.Now, AmountWait = 90000, AmountReal = 95000, DatePaymentWait = new DateTime(2019, 2, 10, 10, 40, 52), ProjectID = 2, Pay = "Неоплачен"},
                new Invoice { InvoiceID=3, ClientName="Елисей Носов", DateCreate = DateTime.Now, AmountWait = 40000, AmountReal = 40000, DatePaymentWait = new DateTime(2019, 2, 10, 10, 40, 52), ProjectID = 3, Pay = "Неоплачен"}
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
