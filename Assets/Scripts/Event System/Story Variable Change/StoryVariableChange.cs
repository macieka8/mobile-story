using System;

namespace Game
{
    [Serializable]
    public abstract class StoryVariableChange
    {
        public string VariableName;

        public abstract void ApplyChange(GameStory story);
        public virtual bool CheckIfTrue(GameStory story) => throw new NotImplementedException();
    }
}
