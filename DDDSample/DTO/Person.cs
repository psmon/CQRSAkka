using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;


namespace DDDSample.DTO
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public string NickName { get; set; }

        [DataMember]
        public string MyId { get; set; }
    }


}
