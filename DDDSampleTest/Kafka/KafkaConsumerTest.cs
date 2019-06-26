using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Akka.Routing;
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
                Thread.Sleep(500);
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

            //Test Probe
            probe = this.CreateTestProbe();

            //Test Actor
            myActor = this.ActorOf<MyActor>("myactor");


            //Test Router
            var w1 = this.ActorOf<MyActor>("w1");
            var w2 = this.ActorOf<MyActor>("w2");
            var w3 = this.ActorOf<MyActor>("w3");
            var w4 = this.ActorOf<MyActor>("w4");
            var w5 = this.ActorOf<MyActor>("w5");            
            var router = this.ActorOf(Props.Empty.WithRouter(new RoundRobinGroup(new[] { w1, w2, w3,w4,w5 })), "some-group");

            // Select probe or myActor or router
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
