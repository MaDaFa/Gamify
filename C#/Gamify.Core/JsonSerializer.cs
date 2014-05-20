using Gamify.Core.Interfaces;
using Newtonsoft.Json;

namespace Gamify.Core
{
    public class JsonSerializer<TObject> : ISerializer<TObject>
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
