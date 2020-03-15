using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace CronJobsMysql.Data.Entity
{
    public class CronJobsMysqlContext:DbContext
    {
        public DbSet<CronJob>  CronJob { get; set; }

        public readonly string ConnectionString;

        public CronJobsMysqlContext()
        {
            ConnectionString=SettingManager.GetValue("ConnectionString");
        }

        public CronJobsMysqlContext(DbContextOptions<CronJobsMysqlContext> options)
            : base(options)
        {
        }

        public CronJobsMysqlContext(string connectionString)
        {
            ConnectionString = connectionString;
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ConnectionString);
#if DEBUG
                ILoggerFactory logFactory =new LoggerFactory(new[] {new DebugLoggerProvider()});
                //ILoggerFactory logFactory = new LoggerFactory(new[] { new DebugLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information) });
                optionsBuilder.UseLoggerFactory(logFactory);
                optionsBuilder.EnableSensitiveDataLogging();
#endif
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {           
            base.OnModelCreating(builder);          

            builder.ApplyConfiguration(new CronJobConfiguration());
         
        }


    }
}
