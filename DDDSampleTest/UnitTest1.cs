using NUnit.Framework;

using DDDSample.DTO;
using System.Runtime.Serialization.Json;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void JsonShouldConvertString()
        {
            Person person = new Person();
            person.NickName = "test";            

            string jsonStr = DTOUtils.WriteFromObject<Person>(person);
            Person person2 = DTOUtils.ReadToObject<Person>(jsonStr);

            Assert.AreEqual(person.NickName, person2.NickName);            
        }
        
    }
}