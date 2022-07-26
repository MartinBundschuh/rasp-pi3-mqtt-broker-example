using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaspPi3.WebApi.Models
{
    public class SaveMqttMessageBindingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id of the message.")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Json string of message body.")]
        public string ObjectSendJson { get; set; }

        [Required]
        [Display(Name = "Username the message came from.")]
        public string UserFrom { get; set; }

        [Required]
        [Display(Name = "Username the message came from.")]
        public string Topic { get; set; }

        [Required]
        [Display(Name = "Mqtt Broker and Port.")]
        public string BrokerAndPort { get; set; }

        [Required]
        [Display(Name = "Timestamp from when message has been send.")]
        public DateTime TimeStampSend { get; set; }
    }
}
