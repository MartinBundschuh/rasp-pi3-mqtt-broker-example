using Okra.Navigation;
using RaspPi3.MqttBrokerPiConsumer.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace RaspPi3.MqttBrokerPiConsumer.ViewModel
{
    [ViewModelExport(SpecialPageNames.Home)]
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly MqttConnector mqttConnector = new MqttConnector();
        private string errorMessage = string.Empty;

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
                TryConnectOrDisconnectAndSetError(value);
                IsReadOnly = mqttConnector.IsConnected;
                // Event seems not to be triggered.
                OnPropertyChanged();
            }
        }

        private void TryConnectOrDisconnectAndSetError(bool value)
        {
            errorMessage = string.Empty;
            IsVisible = IsVisible;
            try
            {
                if (value)
                    mqttConnector.Connect();
                else
                    mqttConnector.DisConnect();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                IsVisible = IsVisible;
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
            private set { OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            private set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsVisible
        {
            get { return string.IsNullOrEmpty(errorMessage) ? Visibility.Collapsed : Visibility.Visible; }
            private set { OnPropertyChanged(); }
        }
    }
}
