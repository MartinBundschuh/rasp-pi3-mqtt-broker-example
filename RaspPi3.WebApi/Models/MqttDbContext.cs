namespace RaspPi3.WebApi.Models
{
    using System.Data.Entity;

    public class MqttDbContext : DbContext
    {
        public MqttDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MqttDbContext, MqttDbConfiguration>());
        }

        public DbSet<SaveMqttMessageBindingModel> MqttMessages { get; set; }
    }
}