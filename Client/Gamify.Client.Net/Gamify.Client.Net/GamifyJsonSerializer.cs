using Newtonsoft.Json;

namespace Gamify.Client.Net
{
    public class GamifyJsonSerializer<TObject> : IGamifySerializer<TObject>
    {
        public TObject Deserialize(string serializedObj)
        {
            return JsonConvert.DeserializeObject<TObject>(serializedObj);
        }

        public string Serialize(TObject obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
