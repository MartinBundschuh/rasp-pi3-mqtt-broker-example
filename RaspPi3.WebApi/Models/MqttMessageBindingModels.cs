using System.ComponentModel.DataAnnotations;

namespace RaspPi3.WebApi.Models
{
    public class SaveMqttMessageBindingModel
    {
        [Required]
        [Display(Name = "Json string of message body")]
        public string ObjectSendJson { get; set; }

        [Required]
        [Display(Name = "Username the message came from")]
        public string UserFrom { get; set; }

        [Required]
        [Display(Name = "Username the message came from")]
        public string Topic { get; set; }

        [Required]
        [Display(Name = "Mqtt Broker and Port")]
        public string BrokerAndPort { get; set; }
    }
}
