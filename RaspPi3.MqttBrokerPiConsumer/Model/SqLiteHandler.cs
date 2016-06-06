using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    public class SqLiteSaveableObject
    {
    }

    internal class SqLiteHandler : IDisposable
    {
        internal ObservableCollection<MqttUser> MqttUsers { get; set; }
        internal ObservableCollection<MqttTopic> MqttTopics { get; set; }
        internal ObservableCollection<MqttConnection> MqttConnections { get; set; }

        private readonly string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite");
        private SQLiteConnection connection;

        private readonly List<object> addedOrModifies = new List<object>();
        private readonly List<object> removed = new List<object>();

        internal SqLiteHandler()
        {
            ConnectIfNecessary();
            SetUpLists();
        }

        private void ConnectIfNecessary()
        {
            if (connection == null)
                connection = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
        }

        private void SetUpLists()
        {
            MqttUsers = GetInstantiatedList<MqttUser>(MqttUsers);
            MqttConnections = GetInstantiatedList<MqttConnection>(MqttConnections);
            MqttTopics = GetInstantiatedList<MqttTopic>(MqttTopics);
        }

        private ObservableCollection<T> GetInstantiatedList<T>(ObservableCollection<T> list) where T : SqLiteSaveableObject
        {
            list = new ObservableCollection<T>();
            list.CollectionChanged += OnCollectionChanged;
            return list;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                    removed.Add(item);
            }
            else
            {
                foreach (var item in e.NewItems)
                    addedOrModifies.Add(item);
            }
        }

        internal void DropAllTables()
        {
            foreach (var type in TypeAttribute.GetTypeAttributes())
            {
                ConnectIfNecessary();
                connection.DropTable(type);
            }
        }

        internal void CreateAllTables()
        {
            CreateTables(TypeAttribute.GetTypeAttributes());
        }

        internal void CreateTables(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                ConnectIfNecessary();
                connection.CreateTable(type);
            }
        }

        internal async Task SaveChangesAsync()
        {
            if (addedOrModifies.Count > 0)
            {
                await Task.Run(() =>
                {
                    foreach (var toAdd in addedOrModifies)
                    {
                        ConnectIfNecessary();
                        connection.InsertOrReplace(toAdd, toAdd.GetType());
                    }
                });
            }

            if (removed.Count > 0)
            {
                await Task.Run(() =>
                {
                    foreach (var toRemove in removed)
                    {
                        ConnectIfNecessary();
                        connection.Delete(toRemove);
                    }
                });
            }
        }

        internal async Task<TableQuery<T>> SelectAsync<T>() where T : SqLiteSaveableObject
        {
            ConnectIfNecessary();
            var returnQuery = await Task.Run(() => connection.Table<T>());
            return returnQuery;
        }

        internal TableQuery<T> Select<T>() where T : SqLiteSaveableObject
        {
            ConnectIfNecessary();
            return connection.Table<T>();
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
