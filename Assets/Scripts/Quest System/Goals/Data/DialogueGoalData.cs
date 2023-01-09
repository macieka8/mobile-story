using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Quest/Goal/DialogueGoal")]
    public class DialogueGoalData : GoalData
    {
        [SerializeField] GameStory _story;
        [SerializeField] string _tagToEncounter;

        public GameStory Story => _story;
        public string TagToEncounter => _tagToEncounter;

        public override Goal InitializeGoal()
        {
            return new DialogueGoal(this);
        }
    }
}
