using System.Data.Entity.Migrations;

namespace RaspPi3.WebApi.Models;

public class MqttDbConfiguration : DbMigrationsConfiguration<MqttDbContext>
{
    public MqttDbConfiguration()
    {
        AutomaticMigrationsEnabled = true;
        AutomaticMigrationDataLossAllowed = false;
    }
}