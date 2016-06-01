using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    internal class SqLiteHandler : IDisposable
    {
        private readonly string path;
        private SQLiteConnection connection;

        internal SqLiteHandler()
        {
            path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
        }

        internal void CreateTables(IEnumerable<Type> types)
        {
            foreach (var type in types)
                connection.CreateTable(type);
        }

        public void Dispose()
        {
            if (connection != null)
            {
                try
                {
                    connection.Dispose();
                }
                finally
                {
                    connection = null;
                }
            }
            GC.SuppressFinalize(this);
        }
    }
}
