using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace DDDSample.Adapters.kafka
{
    public class KafkaConsumer
    {

        public KafkaConsumer()
        {

        }

        public Task StartConsumer()
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            var config = new Dictionary<string, object>
                {
                    {"group.id","kafka_consumer" },
                    {"bootstrap.servers", "kafka:9092" },
                    { "enable.auto.commit", "false" }
                };

            Console.WriteLine("kafka StartConsumer ");

            var task = new Task(() => {

                // Were we already canceled?
                ct.ThrowIfCancellationRequested();
             
                using (var consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8)))
                {
                    consumer.Subscribe("test_consumer");
                    consumer.OnMessage += (_, msg) => {
                        //message(msg.Value);
                        Console.WriteLine("kafka msg === " + msg.Value);
                    };
                    bool moreToDo = true;
                    while (moreToDo)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            ct.ThrowIfCancellationRequested();
                        }
                        consumer.Poll(100);
                    }
                }
                
            }, tokenSource2.Token);
            
            return task;
        }
    }
}
