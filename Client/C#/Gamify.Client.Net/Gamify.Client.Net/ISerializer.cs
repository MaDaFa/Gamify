namespace Gamify.Client.Net
{
    public interface ISerializer
    {
        TObject Deserialize<TObject>(string serializedObj);

        string Serialize<TObject>(TObject obj);
    }
}
