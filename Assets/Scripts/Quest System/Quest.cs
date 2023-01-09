using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Game
{
    public enum QuestState
    {
        Await,
        Started,
        Finished
    }

    public class Quest : MonoBehaviour
    {
        public struct SerializableData
        {
            public int questDataIndex;
            public QuestState questState;
            public List<object> goals;
        }

        QuestData _questData;
        QuestState _questState = QuestState.Await;
        List<Goal> _goals;

        public event Action<Quest> OnStateChanged;
        public event Action<Quest> OnUpdated;

        public QuestData QuestData => _questData;
        public QuestState QuestState => _questState;
        public IEnumerable<Goal> Goals => _goals;

        public void Initialize(QuestData quest)
        {
            _questData = quest;

            _goals = new List<Goal>(_questData.GoalDatasCount);
            foreach (var goalData in _questData.GoalDatas)
            {
                var goal = goalData.GoalData.InitializeGoal();

                if (goalData.HideBehind != null)
                {
                    var parent = _goals.Find(goal => goal.Data == goalData.HideBehind);
                    parent?.AddChild(goal);
                }
                goal.OnGoalUpdate += HandleGoalUpdate;

                _goals.Add(goal);
            }

            _questState = QuestState.Started;
            QuestManager.Instance.AddQuest(this);

            // Make sure QuestManager gets notified if quest is already completed
            foreach (var goal in _goals)
            {
                if (goal.Completed)
                {
                    goal.EvaluateCompletion();
                }
            }
        }

        void OnDestroy()
        {
            if (_goals == null || !_goals.Any()) return;
            foreach (var goal in _goals)
            {
                goal.OnGoalUpdate -= HandleGoalUpdate;
            }
        }

        void HandleGoalUpdate(Goal goal)
        {
            OnUpdated?.Invoke(this);
            if (CheckIfCompleted())
            {
                _questState = QuestState.Finished;
                OnStateChanged?.Invoke(this);
                _questData.QuestFinshedGameEvent?.RaiseEvent();
            }
        }

        public bool CheckIfCompleted()
        {
            return _goals.All(goal => goal.Completed);
        }

        public void Load(SerializableData questSerializableData, QuestData questData, IGameDataHandler dataHandler)
        {
            _questState = questSerializableData.questState;
            _questData = questData;

            _goals = new List<Goal>(_questData.GoalDatasCount);
            int i = 0;
            foreach (var goalData in _questData.GoalDatas)
            {
                var goal = goalData.GoalData.InitializeGoal();
                goal.Load(questSerializableData.goals[i], dataHandler);
                goal.OnGoalUpdate += HandleGoalUpdate;
                if (goalData.HideBehind != null)
                {
                    var parent = _goals.Find(goal => goal.Data == goalData.HideBehind);
                    parent?.AddChild(goal);
                }

                _goals.Add(goal);
                i++;
            }
        }
    }
}
