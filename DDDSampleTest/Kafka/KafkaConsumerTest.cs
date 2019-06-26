using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using DDDSample.Adapters.kafka;
using NUnit.Framework;


namespace DDDSampleTest.Kafka
{
    public class MyActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();
        public MyActor()
        {
            Receive<KafkaMessage>(ka => {
                log.Info(string.Format("Receved:{0} ,{1}", ka.message, ka.offset.Offset));
            });
        }
    }

    public class KafkaConsumerTest : TestKit
    {
        KafkaProduce kafkaProduce;
        KafkaConsumer kafkaConsumer;
        TestProbe   probe;
        IActorRef myActor;

        [SetUp]
        public void Setup()
        {
            kafkaConsumer = new KafkaConsumer("kafka:9092", "test_consumer");

            probe = this.CreateTestProbe();
            myActor = this.ActorOf<MyActor>("myactor");

            kafkaConsumer.CreateConsumer(probe).Start();
            Thread.Sleep(1000);
            
            kafkaProduce = new KafkaProduce("kafka:9092", "test_consumer");
        }

        [TearDown]
        public void Down()
        {
            kafkaConsumer.Stop();
        }

        [Test]
        public void WaitforCleanConsumeQueue()
        {
            Thread.Sleep(5000);
        }

        [Test]
        public void MyActorConsumerTest()
        {
            int testCount = 20;
            Guid lastGuid = Guid.Empty;
            for (int i = 0; i < testCount; i++)
            {
                Guid guid = Guid.NewGuid();
                kafkaProduce.ProduceAsync(guid.ToString());

                if (i == testCount - 1) lastGuid = guid;
            }
            kafkaProduce.Flush(10000);
            WaitforCleanConsumeQueue();            
        }

        [Test]
        public void ProduceAndConsumerLoadTest()
        {
            int testCount = 50000;
            Guid lastGuid=Guid.Empty;
            for (int i = 0; i < testCount; i++)
            {
                Guid guid = Guid.NewGuid();
                kafkaProduce.ProduceAsync(guid.ToString());
            
                if (i == testCount - 1) lastGuid = guid;               
            }
            kafkaProduce.Flush(10000);
            
            Within(TimeSpan.FromSeconds(5), () => {
                AwaitCondition(() => probe.HasMessages);
                for (int i = 0; i < testCount; i++)
                {
                    probe.ExpectMsg<KafkaMessage>(TimeSpan.FromSeconds(2));
                    if ( (i % 1000 == 0) || i == testCount-1)
                    {
                        KafkaMessage curMessage = probe.LastMessage as KafkaMessage;
                        Console.WriteLine(string.Format("RecevedCnt:{0} ,{1}, {2}", i, curMessage.message,curMessage.offset.Offset ));
                    }

                }
                KafkaMessage lastMessage = probe.LastMessage as KafkaMessage;
                Assert.AreEqual(lastGuid.ToString(), lastMessage.message);
            });
            
        }

        [Test]
        public void ProduceAndConsumerRepeatTest()
        {
            for (int i = 0; i < 10; i++)
                ProduceAndConsumerTest();
        }

        private void ProduceAndConsumerTest()
        {
            Guid guid= Guid.NewGuid();
            
            kafkaProduce.Produce(guid.ToString());
            
            Within(TimeSpan.FromSeconds(3), () => {
                
                AwaitCondition(() => probe.HasMessages);
                
                probe.ExpectMsg<KafkaMessage>(TimeSpan.FromSeconds(0));

                KafkaMessage lastMessage = probe.LastMessage as KafkaMessage;

                Assert.AreEqual(guid.ToString(), lastMessage.message);

            });
        }

    }
}
