using System;
using uPLibrary.Networking.M2Mqtt;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

[assembly: CLSCompliant(false)]
namespace RaspPi3.MqttBrokerHost
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral backgroundTraskDeferral;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            if (taskInstance == null)
                throw new ArgumentNullException(nameof(taskInstance), "Task is not allowed to be null");

            backgroundTraskDeferral = taskInstance.GetDeferral();

            var mqttBroker = new MqttBroker();

            // Maybe use User Authentification
            //mqttBroker.UserAuth("Username", "Password");

            mqttBroker.Start();

            // Do not stop the backgroundtask unless it's cancled
            //backgroundTraskDeferral.Complete();

            taskInstance.Canceled += (s, e) =>
            {
                backgroundTraskDeferral.Complete();
            };
        }
    }
}
