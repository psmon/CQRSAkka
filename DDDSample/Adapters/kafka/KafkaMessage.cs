using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDSample.Adapters.kafka
{
    public class KafkaMessage
    {
        public string topic { get; private set; }
        public string message { get; private set; }
        public TopicPartitionOffset offset {get;set;}

        public KafkaMessage(string _topic,string _message, TopicPartitionOffset _offset)
        {
            topic = _topic;
            message = _message;
            offset = _offset;
        }
    }
}
