using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Quest/Quest")]
    public class QuestData : ScriptableObject
    {
        [System.Serializable]
        public class GoalDataEntry
        {
            public GoalData GoalData;
            [Tooltip("if True GoalData is not visible before HideBehind is completed")]
            public GoalData HideBehind;
        }

        [Tooltip("Raise this event when the quest starts")]
        [SerializeField] VoidGameEvent _questStartedGameEvent;
        [Tooltip("Raise this event when the quest finishes")]
        [SerializeField] VoidGameEvent _questFinishedGameEvent;
        [SerializeField] Quest _questPrefab;

        [SerializeField] string _questName;
        [SerializeField] string _description;
        [SerializeField] List<GoalDataEntry> _goalDatas;

        public string Name => _questName;
        public string Description => _description;
        public IEnumerable<GoalDataEntry> GoalDatas => _goalDatas;
        public int GoalDatasCount => _goalDatas.Count;
        public VoidGameEvent QuestStartedGameEvent => _questStartedGameEvent;
        public VoidGameEvent QuestFinshedGameEvent => _questFinishedGameEvent;

        public void StartQuest()
        {
            var quest = Instantiate(_questPrefab);
            quest.Initialize(this);
            _questStartedGameEvent?.RaiseEvent();
        }

        public Quest LoadQuest(Quest.SerializableData data, IGameDataHandler dataHandler)
        {
            var quest = Instantiate(_questPrefab);
            quest.Load(data, this, dataHandler);
            return quest;
        }
    }
}
