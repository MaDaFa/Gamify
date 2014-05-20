namespace Gamify.Client.Net
{
    public interface ISerializer<TObject>
    {
        TObject Deserialize(string serializedObj);

        string Serialize(TObject obj);
    }
}
