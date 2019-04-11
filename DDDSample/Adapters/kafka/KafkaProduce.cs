using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace DDDSample.Adapters.kafka
{
    public class KafkaProduce
    {
        private readonly Dictionary<string, object> config;
        private readonly Producer<Null, string> producer;
        private readonly String topic;

        public KafkaProduce(string _topic)
        {
            config = new Dictionary<string, object>
            {
                { "bootstrap.servers", "kafka:9092" }
            };

            topic = _topic;


            producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));
        }

        public void Produce(string data)
        {
            producer.ProduceAsync(topic, null, data).Wait();
            producer.Flush(100);
        }

    }
}
