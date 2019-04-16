using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample.Adapters.kafka;
using DDDSample.DTO;
using Microsoft.AspNetCore.Mvc;


namespace DDDSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaController
    {
        private readonly KafkaProduce kafkaProduce;
        static int seq = 0;

        public KafkaController(KafkaProduce _kafkaProduce)
        {
            kafkaProduce = _kafkaProduce;
        }

        [HttpGet]
        [Route("addPerson")]
        public ActionResult<IEnumerable<string>> Get()
        {
            Person person = new Person();
            person.NickName = "test"+seq;
            person.MyId = seq.ToString();
            string jsonStr = DTOUtils.WriteFromObject<Person>(person);
            kafkaProduce.Produce(jsonStr);
            seq++;
            return new string[] { "value1", "value2" };
        }

    }
}
