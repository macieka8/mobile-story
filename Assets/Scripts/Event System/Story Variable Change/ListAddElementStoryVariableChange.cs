using UnityEngine;

namespace Game
{
    public class ListAddElementStoryVariableChange : StoryVariableChange
    {
        [Header("List Element")]
        public string ElementToAdd;

        public override void ApplyChange(GameStory story)
        {
            story.AddListElement(VariableName, ElementToAdd);
        }
    }
}
