namespace Gamify.Core
{
    public interface ISerializer<TObject>
    {
        TObject Deserialize(string serializedObj);

        string Serialize(TObject obj);
    }
}
