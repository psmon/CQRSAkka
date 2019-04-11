using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDSample.Adapters.kafka
{
    public interface IBookingConsumer
    {
        void Listen(Action<string> message);
    }
}
