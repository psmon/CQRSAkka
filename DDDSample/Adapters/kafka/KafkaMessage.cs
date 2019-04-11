using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDSample.Adapters.kafka
{
    public class KafkaMessage
    {
        public string topic { get; private  set; }
        public string message { get; private set; }

        public KafkaMessage(string _topic,string _message)
        {
            topic = _topic;
            message = _message;
        }
    }
}
