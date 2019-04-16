using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace DDDSample.DTO
{
    public class DTOUtils
    {
        // Create a User object and serialize it to a JSON stream.  
        public static string WriteFromObject<T>(T srcObject)
        {
            //Create a stream to serialize the object to.  
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(ms, srcObject);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        // Deserialize a JSON stream to a User object.  
        public static T ReadToObject<T>(string json) where T : new()
        {
            T deserializedUser = new T();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            try
            {                
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
                deserializedUser = (T)ser.ReadObject(ms);
            }
            catch(Exception e)
            {
                deserializedUser = default(T);
            }
            finally
            {
                if(ms!=null)
                    ms.Close();
            }            
            return deserializedUser;
        }
    }
}
