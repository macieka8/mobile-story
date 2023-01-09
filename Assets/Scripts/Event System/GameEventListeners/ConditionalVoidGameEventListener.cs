using UnityEngine;

namespace Game
{
    public class ConditionalVoidGameEventListener : VoidGameEventListener
    {
        [SerializeField] GameStory _story;
        [Header("Trigger Event if Variable == Value")]
        [SerializeReference] StoryVariableChange _storyVariableCondition;

        public void SetCondition(StoryVariableChange condition)
        {
            _storyVariableCondition = condition;
        }

        public override void OnEventRaised(Void value)
        {
            if (_storyVariableCondition.CheckIfTrue(_story))
                base.OnEventRaised(value);
        }
    }
}
