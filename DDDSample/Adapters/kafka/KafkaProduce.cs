using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace DDDSample.Adapters.kafka
{
    public class KafkaProduce
    {
        //private readonly ProducerConfig config;
        private readonly IProducer<Null, string> producer;
        private readonly String topic;

        public KafkaProduce(string server,string _topic)
        {
            var config = new ProducerConfig { BootstrapServers = server };
            
            topic = _topic;

            producer = new ProducerBuilder<Null, string>(config).Build();
            
        }

        public void Produce(string data)
        {
            producer.Produce(topic, new Message<Null, string> { Value = data });                   
        }

        public async Task ProduceAsync(string data)
        {
            await producer.ProduceAsync(topic, new Message<Null, string> { Value = data });            
        }

        public void Flush(int milisecondTimeOut)
        {
            producer.Flush(TimeSpan.FromMilliseconds(milisecondTimeOut));
        }

    }
}
