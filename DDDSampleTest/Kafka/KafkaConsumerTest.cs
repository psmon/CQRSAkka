using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DDDSample.Adapters.kafka;
using NUnit.Framework;


namespace DDDSampleTest.Kafka
{
    public class KafkaConsumerTest
    {
        KafkaProduce kafkaProduce;
        KafkaConsumer kafkaConsumer; 

        [SetUp]
        public void Setup()
        {
            kafkaProduce = new KafkaProduce("kafka:9092", "test_consumer");
            kafkaConsumer = new KafkaConsumer("kafka:9092", "test_consumer");
        }

        [Test]
        public void TestIt()
        {
            kafkaConsumer.CreateConsumer().Start();
            
            for(int i=0;i<10;i++)
                kafkaProduce.Produce("SomeMessage");
        }
    }
}
