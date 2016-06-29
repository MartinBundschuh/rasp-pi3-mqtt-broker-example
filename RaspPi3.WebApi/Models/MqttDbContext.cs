namespace RaspPi3.WebApi.Models
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    public class MqttDbContext : DbContext
    {
        public MqttDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MqttDbContext, MqttConfiguration>());
        }

         public virtual DbSet<SaveMqttMessageBindingModel> MqttMessages { get; set; }
    }

    public class MqttConfiguration : DbMigrationsConfiguration<MqttDbContext>
    {
        public MqttConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }
    }
}