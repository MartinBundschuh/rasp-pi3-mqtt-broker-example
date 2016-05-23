using Okra;
using Okra.Navigation;

namespace RaspPi3.MqttBrokerHostVisual
{
    public class AppBootstrapper : OkraBootstrapper
    {
        /// <summary>
        /// Perform general initialization of application services.
        /// </summary>
        protected override void SetupServices()
        {
            // Configure the navigation manager to preseve application state in local storage
            NavigationManager.NavigationStorageType = NavigationStorageType.Local;
        }
    }
}