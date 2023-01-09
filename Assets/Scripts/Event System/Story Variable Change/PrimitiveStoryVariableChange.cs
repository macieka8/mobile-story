using System;
using UnityEngine.Scripting.APIUpdating;

namespace Game
{
    [Serializable]
    public class PrimitiveStoryVariableChange<T> : StoryVariableChange
    {
        public T Value;

        public override void ApplyChange(GameStory story)
        {
            story.SetVariable(VariableName, Value);
        }

        public override bool CheckIfTrue(GameStory story)
        {
            return story.GetVariable<T>(VariableName).Equals(Value);
        }
    }

    [MovedFrom(false, sourceAssembly: "SharedAssembly")]
    public class IntStoryVariableChange : PrimitiveStoryVariableChange<int> { }
    [MovedFrom(false, sourceAssembly: "SharedAssembly")]
    public class BoolStoryVariableChange : PrimitiveStoryVariableChange<bool> { }
    [MovedFrom(false, sourceAssembly: "SharedAssembly")]
    public class StringStoryVariableChange : PrimitiveStoryVariableChange<string> { }
}
