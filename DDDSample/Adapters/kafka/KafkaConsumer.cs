using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Confluent.Kafka;

namespace DDDSample.Adapters.kafka
{
    public class KafkaConsumer
    {
        private readonly string topic;
        private ConsumerConfig config;
        private readonly CancellationTokenSource tokenSource2;
        public Boolean HasMessage { get; set; }

        public KafkaConsumer(string _server,string _topic,string groupid="default-group")
        {
            config = new ConsumerConfig
            {
                GroupId = groupid,
                BootstrapServers = _server,
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            
            topic = _topic;
            tokenSource2 = new CancellationTokenSource();            
        }

        public void Stop()
        {
            Console.WriteLine("try down KafkaConsumer.....");
            tokenSource2.Cancel();
        }

        public Task CreateConsumer(IActorRef consumeAoctor)
        {
            Console.WriteLine($"kafka StartConsumer topic:{topic}");

            var task = new Task(() => {
                using (var c = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    c.Subscribe(topic);
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var cr = c.Consume(tokenSource2.Token);
                                consumeAoctor.Tell(new KafkaMessage(topic, cr.Value,cr.TopicPartitionOffset));
                                //Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                            }
                            catch (ConsumeException e)
                            {
                                Console.WriteLine($"Error occured: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Ensure the consumer leaves the group cleanly and final offsets are committed.
                        Console.WriteLine("safe close");
                        c.Close();
                    }
                }               
            }, tokenSource2.Token);
            
            return task;
        }
    }
}
