using RaspPi3.WebApi.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace RaspPi3.WebApi.Controllers
{
    public class MqttMessageController : ApiController
    {
        // GET api/mqttMessages
        /// <summary>
        /// Returns latest 100 saved messages.
        /// </summary>
        /// <returns>A list with up to 100 messages.</returns>
        public IEnumerable<SaveMqttMessageBindingModel> Get()
        {
            return new SaveMqttMessageBindingModel [] { null, null };
        }

        // GET api/mqttMessages/Topic
        /// <summary>
        /// Return latest 100 saved messages for a specific topic.
        /// </summary>
        /// <param name="topic">Name of topic.</param>
        /// <returns>A list with up to 100 messages for a topic.</returns>
        public IEnumerable<SaveMqttMessageBindingModel> Get(string topic)
        {
            return new SaveMqttMessageBindingModel[] { null, null };
        }

        // GET api/mqttMessages/TopicUser
        /// <summary>
        /// Return latest 100 saved messages for a specific topic and user.
        /// </summary>
        /// <param name="topic">Name of topic.</param>
        /// <param name="user">Name of user.</param>
        /// <returns>A list with up to 100 messages for a topic and user.</returns>
        public IEnumerable<SaveMqttMessageBindingModel> Get(string topic, string user)
        {
            return new SaveMqttMessageBindingModel[] { null, null };
        }

        // GET api/mqttMessages/5
        /// <summary>
        /// Returns the saves message with the given id.
        /// </summary>
        /// <param name="id">Id of message.</param>
        /// <returns>A single message with the given id.</returns>
        public SaveMqttMessageBindingModel Get(int id)
        {
            return null;
        }

        // POST api/mqttMessages
        /// <summary>
        /// Saves a new message. Id is autoincrement.
        /// </summary>
        /// <param name="mqttMessage">Message Json object.</param>
        public void Post([FromBody]SaveMqttMessageBindingModel mqttMessage)
        {
        }

        // DELETE api/mqttMessages/5
        /// <summary>
        /// Deletes the message with the given id.
        /// </summary>
        /// <param name="id">Id of message.</param>
        [Authorize]
        public void Delete(int id)
        {
        }

        // DELETE api/mqttMessages/Message
        /// <summary>
        /// Deletes the given message.
        /// </summary>
        /// <param name="mqttMessage">Message Json object.</param>
        [Authorize]
        public void Delete([FromBody]SaveMqttMessageBindingModel mqttMessage)
        {
        }
    }
}