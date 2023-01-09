using System;

namespace Game
{
    public class DialogueGoal : Goal
    {
        public struct SerializableData
        {
            public bool completed;
        }

        public DialogueGoalData DialogueGoalData => Data as DialogueGoalData;

        public DialogueGoal(GoalData data) : base(data)
        {
            DialogueGoalData.Story.TagResolverManager.AddTagListener(
                DialogueGoalData.TagToEncounter,
                HandleTagEncounter);
        }

        public void HandleTagEncounter(string tag)
        {
            Completed = true;
            InvokeOnGoalUpdate();
            InvokeOnGoalCompleted();
            
            DialogueGoalData.Story.TagResolverManager.RemoveTagListener(DialogueGoalData.TagToEncounter, HandleTagEncounter);
        }

        public override string GetGoalProgressString()
        {
            return Completed ? "O" : "X";
        }

        public override void EvaluateCompletion()
        {
            InvokeOnGoalUpdate();
        }

        public override object Save()
        {
            return new SerializableData()
            {
                completed = Completed
            };
        }

        public override void Load(object data, IGameDataHandler dataHandler)
        {
            var deserializedData = dataHandler.ToObject<SerializableData>(data);
            Completed = deserializedData.completed;
            if (Completed)
            {
                DialogueGoalData.Story.TagResolverManager.RemoveTagListener(DialogueGoalData.TagToEncounter, HandleTagEncounter);
            }
        }
    }
}
