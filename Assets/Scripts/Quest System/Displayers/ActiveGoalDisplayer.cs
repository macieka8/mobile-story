using UnityEngine;
using TMPro;

namespace Game
{
    public class ActiveGoalDisplayer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _goalDescriptionText;
        [SerializeField] TextMeshProUGUI _goalProgressText;
        [SerializeField] Color _completedTextColor;
        Goal _goal;

        public void InitializeGoalDisplayer(Goal goal)
        {
            _goal = goal;
            _goal.OnGoalUpdate += HandleGoalUpdate;
            _goal.OnGoalCompleted += HandleGoalCompleted;

            if (_goal.Parent != null && !_goal.Parent.Completed)
            {
                _goal.Parent.OnGoalCompleted += HandleParentCompleted;

                _goalDescriptionText.gameObject.SetActive(false);
                _goalProgressText.gameObject.SetActive(false);
            }

            HandleGoalUpdate(_goal);
            if (goal.Completed) HandleGoalCompleted(goal);
        }

        void HandleParentCompleted(Goal obj)
        {
            _goalDescriptionText.gameObject.SetActive(true);
            _goalProgressText.gameObject.SetActive(true);

            _goal.Parent.OnGoalCompleted -= HandleParentCompleted;
        }

        void HandleGoalCompleted(Goal obj)
        {
            _goalDescriptionText.color = _completedTextColor;
            _goalProgressText.color = _completedTextColor;
        }

        void HandleGoalUpdate(Goal obj)
        {
            _goalDescriptionText.text = _goal.Data.Description;
            _goalProgressText.text = _goal.GetGoalProgressString();
        }

        void OnDestroy()
        {
            if (_goal == null) return;
            _goal.OnGoalUpdate -= HandleGoalUpdate;
            _goal.OnGoalCompleted -= HandleGoalCompleted;

            if (_goal.Parent != null)
                _goal.Parent.OnGoalCompleted -= HandleParentCompleted;
        }
    }
}
