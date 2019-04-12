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
            person.name = "test";
            person.age = 11;

            string jsonStr = DTOUtils.WriteFromObject<Person>(person);
            Person person2 = DTOUtils.ReadToObject<Person>(jsonStr);

            Assert.AreEqual(person.name, person2.name);            
        }
        
    }
}