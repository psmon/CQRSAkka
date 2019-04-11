using System;
using System.Collections.Generic;
using System.Text;
using DDDSample.Adapters.kafka;
using NUnit.Framework;


namespace DDDSampleTest.Kafka
{
    public class KafkaProduceTest
    {
        KafkaProduce kafkaProduce;

        [SetUp]
        public void Setup()
        {
            kafkaProduce = new KafkaProduce("kafka:9092","test_consumer");
           
        }

        [Test]
        public void ProduceTest()
        {
            kafkaProduce.Produce("test");
        }

    }
}
