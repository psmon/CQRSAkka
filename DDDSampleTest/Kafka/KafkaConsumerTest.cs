using System;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using DDDSample.Adapters.kafka;
using NUnit.Framework;


namespace DDDSampleTest.Kafka
{
    public class KafkaConsumerTest : TestKit
    {
        KafkaProduce kafkaProduce;
        KafkaConsumer kafkaConsumer;
        TestProbe probe;

        [SetUp]
        public void Setup()
        {
            kafkaConsumer = new KafkaConsumer("kafka:9092", "test_consumer");
            probe = this.CreateTestProbe();
            kafkaConsumer.CreateConsumer(probe).Start();
            
            kafkaProduce = new KafkaProduce("kafka:9092", "test_consumer");           
        }

        [Test]
        public void ProduceAndConsumerTest()
        {            
            kafkaProduce.Produce("SomeMessage");
            
            Within(TimeSpan.FromSeconds(3), () => {
                
                AwaitCondition(() => probe.HasMessages);
                
                probe.ExpectMsg<KafkaMessage>(TimeSpan.FromSeconds(0));

                KafkaMessage lastMessage = probe.LastMessage as KafkaMessage;

                Assert.AreEqual("SomeMessage", lastMessage.message);

            });
        }

    }
}
