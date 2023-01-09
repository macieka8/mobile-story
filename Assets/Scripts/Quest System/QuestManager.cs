using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public class QuestManager : MonoBehaviour, IPersistant
    {
        public struct SerializableData
        {
            public List<Quest.SerializableData> quests;
            public int activeQuestId;
        }

        static QuestManager _instance;
        public static QuestManager Instance => _instance;

        [SerializeField] List<QuestData> _allQuestData;

        public List<Quest> StartedQuests = new List<Quest>();
        public List<Quest> FinishedQuests = new List<Quest>();

        public Quest ActiveQuest { get; set; }

        public event Action<Quest> OnQuestStarted;
        public event Action<Quest> OnQuestUpdated;
        public event Action<Quest> OnQuestFinished;

        public event Action<Quest> OnActiveQuestChanged;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public void AddQuest(Quest newQuest)
        {
            if (!StartedQuests.Contains(newQuest))
            {
                StartedQuests.Add(newQuest);
                newQuest.OnStateChanged += HandleQuestStateChanged;
                newQuest.OnUpdated += HandleQuestUpdated;
                OnQuestStarted?.Invoke(newQuest);
            }
        }

        private void HandleQuestUpdated(Quest quest)
        {
            OnQuestUpdated?.Invoke(quest);
        }

        void HandleQuestStateChanged(Quest quest)
        {
            if (quest.QuestState == QuestState.Finished)
            {
                StartedQuests.Remove(quest);
                FinishedQuests.Add(quest);
                OnQuestFinished?.Invoke(quest);
                quest.OnStateChanged -= HandleQuestStateChanged;
                quest.OnUpdated -= HandleQuestUpdated;
            }
        }

        public int GetQuestId(Quest quest)
        {
            return _allQuestData.IndexOf(quest.QuestData);
        }

        public Quest GetQuestFromId(int id)
        {
            foreach (var quest in StartedQuests)
            {
                if (GetQuestId(quest) == id) return quest;
            }
            return null;
        }

        public bool HasQuest(QuestData questData)
        {
            foreach (var quest in StartedQuests)
                if (quest.QuestData == questData) return true;
            foreach (var quest in FinishedQuests)
                if (quest.QuestData == questData) return true;
            return false;
        }

        Quest.SerializableData SaveQuest(Quest quest)
        {
            var data = new Quest.SerializableData()
            {
                questDataIndex = _allQuestData.IndexOf(quest.QuestData),
                questState = quest.QuestState,
                goals = new List<object>()
            };

            foreach (var goal in quest.Goals)
            {
                data.goals.Add(goal.Save());
            }

            return data;
        }

        public object Save()
        {
            List<Quest.SerializableData> questsSerialized = new List<Quest.SerializableData>();

            foreach (var quest in StartedQuests)
                questsSerialized.Add(SaveQuest(quest));
            foreach (var quest in FinishedQuests)
                questsSerialized.Add(SaveQuest(quest));

            return new SerializableData()
            {
                quests = questsSerialized,
                activeQuestId = ActiveQuest ? GetQuestId(ActiveQuest) : -1
            };
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            ClearAllQuests();

            var loadedQuests = dataHandler.ToObject<SerializableData>(data);
            foreach (var questData in loadedQuests.quests)
            {
                if (questData.questDataIndex < 0 || questData.questDataIndex >= _allQuestData.Count)
                    throw new Exception("QuestData index out of bounds");

                var quest = _allQuestData[questData.questDataIndex].LoadQuest(questData, dataHandler);
                quest.OnStateChanged += HandleQuestStateChanged;
                quest.OnUpdated += HandleQuestUpdated;
                if (questData.questState == QuestState.Finished)
                {
                    FinishedQuests.Add(quest);
                }
                else StartedQuests.Add(quest);
            }

            if (loadedQuests.activeQuestId >= _allQuestData.Count)
                throw new Exception($"Active Quest Index out of bounds");
            if (loadedQuests.activeQuestId > -1)
            {
                ActiveQuest = GetQuestFromId(loadedQuests.activeQuestId);
                OnActiveQuestChanged?.Invoke(ActiveQuest);
            }
        }

        void ClearAllQuests()
        {
            foreach (var item in StartedQuests)
            {
                Destroy(item.gameObject);
            }
            foreach (var item in FinishedQuests)
            {
                Destroy(item.gameObject);
            }

            StartedQuests.Clear();
            FinishedQuests.Clear();
        }
    }
}
