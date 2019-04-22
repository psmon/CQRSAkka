using NUnit.Framework;

using DDDSample.DTO;
using System.Runtime.Serialization.Json;
using System.Net;
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
        

        [Test]
        public void TestBitOperatorAreEqual()
        {
            int aaa = (int)ServicePointManager.SecurityProtocol;
            int bbb = (int)ServicePointManager.SecurityProtocol;
            int a = 3072;            /* 60 = 0011 1100 */
            int b = 768;            /* 13 = 0000 1101 */
            int c = 192;            
            aaa = a | b | c;
            bbb |= a | b | c;            
            Assert.AreEqual(aaa, bbb);
            Assert.AreEqual(a, 3072 | 0);
        }
        
    }
}