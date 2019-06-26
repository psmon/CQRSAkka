﻿using System;
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

        public KafkaProduce(string server,string _topic)
        {
            config = new Dictionary<string, object>
            {
                { "bootstrap.servers", server },
                { "group.id","kafka_consumer" }
            };

            topic = _topic;


            producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));
        }

        public void Produce(string data)
        {
            producer.ProduceAsync(topic, null, data).Wait();           
        }

        public async Task ProduceAsync(string data)
        {
            await producer.ProduceAsync(topic, null, data);
        }

        public void Flush(int milisecondTimeOut)
        {
            producer.Flush(milisecondTimeOut);
        }

    }
}
