using Microsoft.Data.Entity;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    class MqttContext : DbContext
    {
        public DbSet<MqttUser> MqttUsers { get; set; }
        public DbSet<MqttConnection> MqttConnections { get; set; }
        public DbSet<MqttTopic> MqttTopics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=MqttBroker.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MqttConnection>()
                .Property(c => c.BrokerName)
                .IsRequired();

            modelBuilder.Entity<MqttUser>()
                .Property(u => u.UserName)
                .IsRequired();

            modelBuilder.Entity<MqttTopic>()
                .Property(t => t.Id)
                .IsRequired();
        }
    }
}
