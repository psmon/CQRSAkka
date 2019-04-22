# DDD Sample 프로젝트

도메인 이벤트를 다루는 웹 어플리케이션의 레이아웃정리편

추가목표 : CRUD에 탈피하여, Kafka/MesageQueue(AKKA)등을 활용하여 CQRS에 접근

## Application LayOut

	-Infra
	 -db
	 -kafka
	 -network
	-Common
	 -Domain : 도메인객체를 올바르게사용하기 위한 인터페이스
	-DDDSample : WebApp	 
	 -Controllers:외부에서 접근하는 원격 포트가 여기로 연결되어 주로 서비스 객체를 호출 
	 -Service:캡슐화된 상태가 없으며, 모델에 홀로 존재하는 인터페이스로 제공되는 연산
	 -Domain:도메인 로직을 책임지는 구현에 해당하는 부분
	 -Entity:근본적으로 속성이 아니라, 연속성과 식별성의 맥락에서 정의되는 객체
	 -DTO:원격으로 전송될때 변환되는 오브젝트 주로 Json직렬화처리됨
	 -Repository:객체 컬렉션을 흉내내며, 저장,조회,검색 행의를 캡슐화하는 메카니즘
	 -Adapters:외부 Rest및 Kafka등을 사용하기위한 어댑터
	-DDDSampleTest : 도메인을 검증할수 있는 유닛테스트 수행
	 -Repository:저장소 검증
	 -Kafka:카프카 메시지 검증
	 -Domain:도메인 검증


## 로컬 구동환경 - 도커를 사용한 로컬 테스팅

### 네트워크 준비하기 - Infra/networkinit.cmd
	
	docker network create --driver=bridge --subnet=172.19.0.0/16 devnet

	docker network inspect devnet

### DB 준비하기
	
	/Infra/db/docker-compose up

### .net core ㅐ그 마이그레이션
	dotnet ef migrations add InitialCreate
	dotnet ef database update
	dotnet ef migrations script
	dotnet ef migrations add AddProductReviews
	dotnet ef database update


### Kafka 준비하기

	/Infra/kafka/docker-compose up


## Kafka메시지 검증을, 로컬에 수행할수 있게끔 유닛테스트 구성
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

카프카의 수신메시지를, Actor를 사용한 분기 필터처리

	consumer.OnMessage += (_, msg) => {
		if (consumeAoctor != null) consumeAoctor.Tell(new KafkaMessage(msg.Topic, msg.Value));
	};