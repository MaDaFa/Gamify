namespace Gamify.Core.Interfaces
{
    public interface ISerializer<TObject>
    {
        TObject Deserialize(string serializedObj);

        string Serialize(TObject obj);
    }
}
