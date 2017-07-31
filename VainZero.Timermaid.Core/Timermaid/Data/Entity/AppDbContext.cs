using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.CodeFirst;

namespace VainZero.Timermaid.Data.Entity
{
    public class AppDbContext
        : DbContext
    {
        static string ConnectionString { get; } =
            new SQLiteConnectionStringBuilder()
            {
                DataSource = "./data/database.sqlite",
                DateTimeKind = DateTimeKind.Utc,
            }.ToString();

        public AppDbContext()
            : base(new SQLiteConnection(ConnectionString), contextOwnsConnection: true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var initializer = new SqliteCreateDatabaseIfNotExists<AppDbContext>(modelBuilder);
            Database.SetInitializer(initializer);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Schedule> Schedules { get; set; }
    }
}
