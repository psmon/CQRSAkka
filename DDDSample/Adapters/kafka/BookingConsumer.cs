using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDSample.Adapters.kafka
{

    public class BookingConsumer : IBookingConsumer
    {
        public void Listen(Action<string> message)
        {
            var config = new Dictionary<string, object>
            {
                {"group.id","booking_consumer" },
                {"bootstrap.servers", "192.168.99.100:9092" },
                { "enable.auto.commit", "false" }
            };

            using (var consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.Subscribe("timemanagement_booking");
                consumer.OnMessage += (_, msg) => {
                    message(msg.Value);
                };

                while (true)
                {
                    consumer.Poll(100);
                }
            }
        }
    }
}
