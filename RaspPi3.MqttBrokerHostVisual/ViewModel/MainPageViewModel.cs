using Okra.Navigation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerHostVisual.ViewModel
{
    [ViewModelExport(SpecialPageNames.Home)]
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly MqttBroker mqttBroker;
        public MainPageViewModel()
        {
            //startMqttBroker = new StartMqttBroker(this);
            //stopMqttBroker = new StopMqttBroker(this);
            mqttBroker = new MqttBroker();

            // Maybe use User Authentification
            //mqttBroker.UserAuth("Username", "Password");

            IsConnected = true;
            //startMqttBroker.Execute(startMqttBroker);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                if (isConnected)
                    mqttBroker.Start();
                else
                    mqttBroker.Stop();

                OnPropertyChanged();
            }
        }

    //    private readonly StartMqttBroker startMqttBroker;
    //    public StartMqttBroker StartMqtt { get { return StartMqtt; } }

    //    public class StartMqttBroker : ICommand
    //    {
    //        public event EventHandler CanExecuteChanged;
    //        private readonly MainPageViewModel mainPageViewModel;
    //        public StartMqttBroker(MainPageViewModel mainPageViewModel)
    //        {
    //            this.mainPageViewModel = mainPageViewModel;
    //            mainPageViewModel.PropertyChanged += (s, e) =>
    //            {
    //                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    //            };
    //        }

    //        public bool CanExecute(object parameter)
    //    {
    //        return mainPageViewModel.mqttBroker != null && !mainPageViewModel.IsConnected;
    //    }

    //    public void Execute(object parameter)
    //    {
    //        mainPageViewModel.mqttBroker.Start();
    //        mainPageViewModel.IsConnected = true;
    //    }
    //}

    //private readonly StopMqttBroker stopMqttBroker;
    //public StopMqttBroker stopMqtt { get { return stopMqttBroker; } }

    //public class StopMqttBroker : ICommand
    //{
    //    public event EventHandler CanExecuteChanged;
    //    private readonly MainPageViewModel mainPageViewModel;
    //    public StopMqttBroker(MainPageViewModel mainPageViewModel)
    //    {
    //        this.mainPageViewModel = mainPageViewModel;
    //        mainPageViewModel.PropertyChanged += (s, e) =>
    //        {
    //            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    //        };
    //    }

    //    public bool CanExecute(object parameter)
    //    {
    //        return mainPageViewModel.mqttBroker != null && mainPageViewModel.isConnected;
    //    }

    //    public void Execute(object parameter)
    //    {
    //        mainPageViewModel.mqttBroker.Stop();
    //        mainPageViewModel.IsConnected = false;
    //    }
    //}
}
}
