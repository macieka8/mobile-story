namespace Game
{
    public class KillGoal : Goal
    {
        struct SerializableData
        {
            public int currentKillCount;
            public bool completed;
        }

        int _currentKillCount = 0;

        public KillGoalData KillGoalData => Data as KillGoalData;
        public int CurrentKillCount => _currentKillCount;

        public KillGoal(GoalData data) : base(data)
        {
            _currentKillCount = CombatSystem.Instance.GetKillCount(KillGoalData.CombatEntityType);
            CombatSystem.Instance.OnEntityKilled += HandleOnEntityKilled;

            EvaluateCompletion();
        }

        void HandleOnEntityKilled(CombatEntity killedEntity)
        {
            if (killedEntity.Identifier == KillGoalData.CombatEntityType)
            {
                _currentKillCount++;

                EvaluateCompletion();
            }
        }

        public override void EvaluateCompletion()
        {
            if (_currentKillCount >= KillGoalData.RequiredKillCount)
                Completed = true;

            InvokeOnGoalUpdate();
            if (Completed) InvokeOnGoalCompleted();
        }

        public override string GetGoalProgressString()
        {
            return $"{CurrentKillCount}/{KillGoalData.RequiredKillCount}";
        }

        public override object Save()
        {
            return new SerializableData()
            {
                currentKillCount = _currentKillCount,
                completed = Completed
            };
        }

        public override void Load(object data, IGameDataHandler dataHandler)
        {
            var deserializedData = dataHandler.ToObject<SerializableData>(data);
            _currentKillCount = deserializedData.currentKillCount;
            Completed = deserializedData.completed;
        }
    }
}
