using Newtonsoft.Json;

namespace GuessMyNumber.Client.Net
{
    public class GamifySerializer<TObject> : IGamifySerializer<TObject>
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
