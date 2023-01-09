namespace Game
{
    public interface IState
    {
        public int Priority { get; }
        public IState ProcessTransitions();
        public void Enter();
        public void Exit();
    }
}
