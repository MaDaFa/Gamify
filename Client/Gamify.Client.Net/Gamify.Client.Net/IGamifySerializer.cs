namespace Gamify.Client.Net
{
    public interface IGamifySerializer<TObject>
    {
        TObject Deserialize(string serializedObj);

        string Serialize(TObject obj);
    }
}
