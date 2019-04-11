# DDD Sample 프로젝트

도메인을 다루는 웹 어플리케이션의 레이아웃을 

## Application LayOut

	-Infra
	-db
	-kafka
	-network
	-Common
	-Domain
	-DDDSample : WebApp
	-Adapters:외부 Rest및 Kafka등을 사용하기위한 어댑터
	-Controllers:외부에서 접근하는 원격 포트가 여기로 연결되어 주로 서비스 객체를 호출 
	-Service:도메인에 접근하여 서비스를 제공,주로 도메인 이벤트를 통해 도메인에 접근가능
	-Domain:도메인 아이디어인 도메인 객체만을 다룸, Entity Repository는 도메인 모델을 수행하기위한 활용요소
	-Entity:데이터베이스 엔티티정의
	-DTO:원격으로 전송될때 변환되는 오브젝트 주로 Json직렬화처리됨
	-Repository:데이터베이스의 CRUD를 수행할수 있음
	-DDDSampleTest : 도메인을 검증할수 있는 유닛테스트 수행


## 로컬 구동환경 - 도커를 사용한 로컬 테스팅

### 네트워크 준비하기 - Infra/networkinit.cmd
	
	docker network create --driver=bridge --subnet=172.19.0.0/16 devnet

	docker network inspect devnet

### DB 준비하기
	
	/Infra/db/docker-compose up

### Kafka 준비하기

	/Infra/kafka/docker-compose up


## Kafka 활용
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

## Kafka + Actor
	consumer.OnMessage += (_, msg) => {
		if (consumeAoctor != null) consumeAoctor.Tell(new KafkaMessage(msg.Topic, msg.Value));
		HasMessage = true;
	};

