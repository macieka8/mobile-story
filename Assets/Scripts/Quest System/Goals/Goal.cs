using System;
using System.Collections.Generic;

namespace Game
{
    public interface ITreeNode<T>
    {
        public T Parent { get; }
        public ICollection<T> Children { get; }
    }

    public abstract class Goal : IPersistant, ITreeNode<Goal>
    {
        Goal _parent;
        List<Goal> _children = new List<Goal>();

        GoalData _data;
        public bool Completed { get; protected set; } = false;
        public GoalData Data => _data;

        public Goal Parent => _parent;
        public ICollection<Goal> Children => _children;

        public event Action<Goal> OnGoalUpdate = delegate { };
        public event Action<Goal> OnGoalCompleted = delegate { };

        public Goal(GoalData data)
        {
            _data = data;
        }

        public void AddChild(Goal child)
        {
            _children.Add(child);
            child._parent = this;
        }

        public abstract string GetGoalProgressString();

        protected void InvokeOnGoalUpdate()
        {
            OnGoalUpdate.Invoke(this);
        }

        protected void InvokeOnGoalCompleted()
        {
            if (Data.OnCompletedEvents != null)
            {
                foreach (var gameEvent in Data.OnCompletedEvents)
                {
                    gameEvent.RaiseEvent();
                }
            }
            OnGoalCompleted.Invoke(this);
        }

        public abstract void EvaluateCompletion();

        public abstract object Save();
        public abstract void Load(object data, IGameDataHandler dataHandler);
    }
}
