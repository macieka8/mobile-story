using UnityEngine;
using TMPro;

namespace Game
{
    public class ActiveQuestDisplayer : MonoBehaviour
    {
        [SerializeField] GameObject _displayedParent;
        [SerializeField] GameObject _hiddenParent;

        [SerializeField] TextMeshProUGUI _questNameText;
        [SerializeField] ActiveGoalDisplayer _goalDisplayerPrefab;
        [SerializeField] Transform _goalsParent;

        public Quest ActiveQuest
        {
            get => QuestManager.Instance.ActiveQuest;
            set => QuestManager.Instance.ActiveQuest = value;
        }

        void Start()
        {
            ShowQuest(ActiveQuest != null);
        }

        void OnEnable()
        {
            QuestManager.Instance.OnQuestStarted += SetNewActiveQuest;
            QuestManager.Instance.OnQuestFinished += HandleQuestFinished;
            QuestManager.Instance.OnActiveQuestChanged += SetNewActiveQuest;
        }

        void OnDisable()
        {
            QuestManager.Instance.OnQuestStarted -= SetNewActiveQuest;
            QuestManager.Instance.OnQuestFinished -= HandleQuestFinished;
            QuestManager.Instance.OnActiveQuestChanged -= SetNewActiveQuest;
        }

        void HandleQuestFinished(Quest obj)
        {
            if (obj != ActiveQuest) return;
            ShowQuest(false);
        }

        public void SetNewActiveQuest(Quest obj)
        {
            if (obj == null || obj.QuestState == QuestState.Finished) return;

            ShowQuest(true);
            ActiveQuest = obj;

            _questNameText.text = obj.QuestData.Name;

            ClearQuestMenu();
            foreach (var goal in obj.Goals)
            {
                var goalDisplayer = Instantiate(_goalDisplayerPrefab, _goalsParent);
                goalDisplayer.InitializeGoalDisplayer(goal);
            }
        }

        void ShowQuest(bool active)
        {
            _displayedParent.SetActive(active);
            _hiddenParent.SetActive(!active);
        }

        void ClearQuestMenu()
        {
            for (int i = _goalsParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_goalsParent.GetChild(i).gameObject);
            }
        }
    }
}
