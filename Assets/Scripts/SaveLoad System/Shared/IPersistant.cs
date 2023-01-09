namespace Game
{
    public interface IPersistant
    {
        object Save();
        void Load(object data, IGameDataHandler dataHandler);
    }
}
