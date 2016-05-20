# rasp-pi3-mqtt-broker-example

Raspberry Pi 3 Project using the MQTT protocoll.

This Project shows how to host an <b>MQTT broker</b> from <b>RaspberryPi 3</b>. The goal ist to controll Pi from Windows phone.
There are three different projects:

- RaspPi3.<b>MqttBrokerHost</b><br>
Here it is shown, how to host a local MQTT broker on an Raspberry Pi with <b>Windows IoT Core</b>. The broker is running on a background task. The project takes use of the <b>GnatMQ</b> Framework.

- RaspPi3.<b>MqttBrokerHostVisual</b><br>
In addition to the first project, you can see the broker hosting status. It's possbile to stop and rerun the broker. The project is written as an Universal Windows App. The <b>Okra</b> Framework is used to handle the MVVM pattern.

- RaspPi3.<b>MqttBrokerPiConsumer</b><br>
This project shows how to connect to an MQTT broker from an Raspberry Pi. Used Frameworks are the Okra Framework to hande MVVM pattern in the Universal Windows app. <b>M2Mqtt</b> provides the connection Framework to <b>CloudMQTT</b> broker where I use the free hosting service. To store data locally I use the <b>SQLite.Net</b> Framework.
I wrote a similar class to the DbContext, since EF is not usable in ARM runtime. Because of this i have kind of an simple and own EF. 
To send and receive messages via the broker I use Json serialization.
To be able to extend the poject easily for further classes, I wrote custom Attributes to extend functionallity almost only with DataAnnotations. Ther has only to be added an ObservableCollection to te SQLiteHandler.
There is also an visualization. You can disconnect and connect, change Broker or Port information and have a littel overview of the latest received and sent messages.
The Raspberry Pi will connect to an WiFi network if theres no lan connection detected.

  For first setup you need to add your WiFi an Broker User/Password Compinations. Also you neet to select an approprative MQTT Broker. Then run the setup method once. You can find the loc within the Task List.
  It also supports Barere Token authorized Http client. Saving the messages now on Azure using RESTful Api.

- RaspPi3.<b>RESTful Web.Api</b><br>
ASP.NET Web.Api ust hostet on Azure. It is connected to Azure DB. Ther is a basic API to select, insert (a.s.o) messages. The API is secured. You can use external logins or register. Either way, the email has to be confirmed to be able to use the API.
Authentification is realisied with Bearer tokens.

Comming soon:
- More use-cases
- Administration page
- Register devices on the fly
