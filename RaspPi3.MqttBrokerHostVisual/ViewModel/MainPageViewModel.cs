using Okra.Navigation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerHostVisual.ViewModel
{
    [ViewModelExport(SpecialPageNames.Home)]
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly MqttBroker mqttBroker; // MyOwnMqttBroker mqttBroker

        public MainPageViewModel()
        {
            mqttBroker = new MqttBroker(); // = new MyOwnMqttBroker();
            IsHosting = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isHosting;
        public bool IsHosting
        {
            get { return isHosting; }
            set
            {
                isHosting = value;
                if (isHosting)
                    mqttBroker.Start(); // .StartAsync();
                else
                    mqttBroker.Stop(); // .StopAsync();

                OnPropertyChanged();
            }
        }
    }
}
