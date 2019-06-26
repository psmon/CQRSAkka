using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace DDDSample.Adapters.kafka
{
    public class KafkaConsumer
    {
        private string server;
        private string topic;

        private CancellationToken ct;
        CancellationTokenSource tokenSource2;

        public Boolean HasMessage { get; set; }

        public KafkaConsumer(string _server,string _topic)
        {
            server = _server;
            topic = _topic;
            tokenSource2 = new CancellationTokenSource();
            ct = tokenSource2.Token;
        }

        public void Stop()
        {
            Console.WriteLine("try down KafkaConsumer.....");
            tokenSource2.Cancel();
        }

        public Task CreateConsumer(IActorRef consumeAoctor)
        {            
            var config = new Dictionary<string, object>
                {
                    {"group.id","kafka_consumer" },
                    {"bootstrap.servers", server },
                    { "enable.auto.commit", "false" }
                };

            Console.WriteLine("kafka StartConsumer ");

            var task = new Task(() => {
                
                using (var consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8)))
                {
                    consumer.Subscribe(topic);
                    consumer.OnMessage += (_, msg) => {
                        //message(msg.Value);                        
                        //Console.WriteLine(string.Format("kafka msg {0} === {1}",msg.Offset.Value,msg.Value));
                        if (consumeAoctor != null) consumeAoctor.Tell(new KafkaMessage(msg.Topic, msg.Value));
                        HasMessage = true;
                    };
                    
                    while (true)
                    {
                        if (ct.IsCancellationRequested)
                        {                       
                            Console.WriteLine("close done KafkaConsumer.....");
                            // Clean up here, then...
                            ct.ThrowIfCancellationRequested();
                        }
                        consumer.Poll(100);
                        //consumer.CommitAsync();
                    }

                }
                
            }, tokenSource2.Token);
            
            return task;
        }
    }
}
