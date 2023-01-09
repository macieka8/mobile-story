namespace Game
{
    public interface IGameDataHandler
    {
        public void Save(GameData gameData);
        public GameData Load();
        public T ToObject<T>(object obj);
    }
}
