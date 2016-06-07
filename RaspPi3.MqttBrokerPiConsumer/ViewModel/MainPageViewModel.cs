using Okra.Navigation;
using RaspPi3.MqttBrokerPiConsumer.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using static RaspPi3.MqttBrokerPiConsumer.Model.MqttConnection;

namespace RaspPi3.MqttBrokerPiConsumer.ViewModel
{
    [ViewModelExport(SpecialPageNames.Home)]
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly MqttConnector mqttConnector;
        private string errorMessage = string.Empty;

        public MainPageViewModel()
        {
            mqttConnector= new MqttConnector();
            IsConnected = true;

            StartPublishToTestTimerIntervall();
        }

        private void StartPublishToTestTimerIntervall()
        {
            var dispatchTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 10)
            };

            dispatchTimer.Tick += (s, e) =>
            {
                RefreshControls();
                mqttConnector.Publish(mqttConnector.mqttUser.TopicsToSubscribe
                    .FirstOrDefault(t => t.Name == "TestChannel"), mqttConnector.mqttUser);
            };

            dispatchTimer.Start();
        }

        private void RefreshControls()
        {
            IsConnected = mqttConnector.IsConnected;
            LatestPublishedMessage = mqttConnector.LatestPublishedMessage;
            LatestPublishedTopic = mqttConnector.LatestPublishedTopic;
            LatestReceivedMessage = mqttConnector.LatestReceivedMessage;
            LatestReceivedTopic = mqttConnector.LatestReceivedTopic;
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
                ErrorMessage = string.Empty;
                IsVisible = IsVisible;

                if (value != mqttConnector.IsConnected)
                    TryConnectOrDisconnectAndSetError(value);

                IsReadOnly = mqttConnector.IsConnected;
                // Event seems not to be triggered.
                OnPropertyChanged();
            }
        }

        private void TryConnectOrDisconnectAndSetError(bool value)
        {
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
            get { return mqttConnector.mqttUser.Connection.BrokerName; }
            set
            {
                mqttConnector.mqttUser.Connection.BrokerName = value;
                OnPropertyChanged();
            }
        }

        public string BrokerPort
        {
            get { return mqttConnector.mqttUser.Connection.BrokerPort.ToString("d"); }
            set
            {
                CloudMqttBrokerPort enumParse;
                if (!Enum.TryParse(value, out enumParse))
                    enumParse = CloudMqttBrokerPort.Default;
                mqttConnector.mqttUser.Connection.BrokerPort = enumParse;
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

        public string LatestPublishedTopic
        {
            get { return mqttConnector.LatestPublishedTopic; }
            private set { OnPropertyChanged(); }
        }

        public string LatestPublishedMessage
        {
            get { return mqttConnector.LatestPublishedMessage; }
            private set { OnPropertyChanged(); }
        }

        public string LatestReceivedTopic
        {
            get { return mqttConnector.LatestReceivedTopic; }
            private set { OnPropertyChanged(); }
        }

        public string LatestReceivedMessage
        {
            get { return mqttConnector.LatestReceivedMessage; }
            private set { OnPropertyChanged(); }
        }
    }
}
