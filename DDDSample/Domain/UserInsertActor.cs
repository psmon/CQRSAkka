using Akka.Actor;
using Akka.Event;
using DDDSample.Adapters.kafka;
using DDDSample.DTO;
using DDDSample.Entity;
using DDDSample.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDSample.Domain
{
    public class UserInsertActor : ReceiveActor
    {        
        private readonly ILoggingAdapter log = Context.GetLogger();
        private readonly UserRepository userRepository;

        public static Props Props(UserRepository _userRepository)
        {            
            return Akka.Actor.Props.Create(() => new UserInsertActor(_userRepository));
        }

        public UserInsertActor(UserRepository _userRepository)
        {
            userRepository = _userRepository;
            Receive<KafkaMessage>(msg => {
                Person pserson = DTOUtils.ReadToObject<Person>(msg.message);
                if (pserson != null)
                {
                    User addUSer = new User()
                    {
                        NickName = pserson.NickName,
                        MyId = pserson.MyId,
                        PassWord = "**********"
                    };
                    userRepository.Add(addUSer);
                    userRepository.SaveChanges();
                }
            });
        }

    }
}
