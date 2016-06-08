using Okra.Navigation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerHostVisual.ViewModel
{
    [ViewModelExport(SpecialPageNames.Home)]
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly MqttBroker mqttBroker; // MyOwnMqttBroker mqttBroker
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isHosting;

        public MainPageViewModel()
        {
            mqttBroker = new MqttBroker(); // = new MyOwnMqttBroker();
            IsHosting = true;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
