using Okra.Navigation;
using RaspPi3.MqttBrokerPiConsumer.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RaspPi3.MqttBrokerPiConsumer.ViewModel
{
    [ViewModelExport(SpecialPageNames.Home)]
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly MqttConnector mqttConnector = new MqttConnector();

        public MainPageViewModel()
        {
            IsConnected = true;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsConnected
        {
            get { return mqttConnector.IsConnected; }
            set
            {
                if (value)
                    mqttConnector.Connect();
                else
                    mqttConnector.DisConnect();

                value = mqttConnector.IsConnected;
                OnPropertyChanged();
                IsReadOnly = mqttConnector.IsConnected;
            }
        }

        public string BrokerName
        {
            get { return mqttConnector.BrokerName; }
            set
            {
                mqttConnector.BrokerName = value;
                OnPropertyChanged();
            }
        }

        public string BrokerPort
        {
            get { return mqttConnector.BrokerPort; }
            set
            {
                mqttConnector.BrokerPort = value;
                OnPropertyChanged();
            }
        }

        public bool IsReadOnly
        {
            get { return mqttConnector.IsConnected; }
            set { OnPropertyChanged(); }
        }
    }
}
