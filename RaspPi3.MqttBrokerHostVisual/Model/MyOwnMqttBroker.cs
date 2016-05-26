using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.System;

namespace RaspPi3.MqttBrokerHostVisual.Model
{
    class MyOwnMqttBroker
    {
        private readonly ProcessLauncherOptions processLauncherOptions;
        private readonly InMemoryRandomAccessStream standardOutput;
        private readonly InMemoryRandomAccessStream standardError;

        public MyOwnMqttBroker()
        {
            standardOutput = new InMemoryRandomAccessStream();
            standardError = new InMemoryRandomAccessStream();
            processLauncherOptions = new ProcessLauncherOptions
            {
                StandardOutput = standardOutput,
                StandardError = standardError
            };
        }

        public bool IsHosting { get; private set; }
        public async void StartAsync()
        {
            if (IsHosting)
                return;

            // System cannot find Path? Somehow not registered. Cmd itself works fine.
            await RunCmdAsync(@"/K cd ""c:\program files (x86)\mosquitto"""); // \mosquitto -c mosquitto.conf
            //await RunCmdAsync(string.Empty);

            IsHosting = true;
        }

        public async void StopAsync()
        {
            if (!IsHosting)
                return;

            await RunCmdAsync(@"/IM ""c:\program files (x86)\mosquitto\mosquitto""");

            IsHosting = false;
        }

        private async Task RunCmdAsync(string args)
        {
            var result = await Task.Run(() => ProcessLauncher.RunToCompletionAsync(@"c:\windows\system32\cmd.exe", args, processLauncherOptions));

            var outputTextDebug = await ReadStreamAsync(standardOutput);
            var errorText = await ReadStreamAsync(standardError);

            if (result.ErrorCode != null)
                throw new Exception(result.ErrorCode.Message);
            if (!string.IsNullOrEmpty(errorText))
                throw new Exception(errorText);
        }

        private async static Task<string> ReadStreamAsync(InMemoryRandomAccessStream RandomAccessstream)
        {
            using (var stream = RandomAccessstream.GetInputStreamAt(0))
            {
                var size = RandomAccessstream.Size;
                var output = new StringBuilder((int)size);
                using (var dataReader = new DataReader(stream))
                {
                    var bytesLoaded = await dataReader.LoadAsync((uint)size);
                    output.Append(dataReader.ReadString(bytesLoaded));
                }
                return output.ToString();
            }
        }
    }
}
