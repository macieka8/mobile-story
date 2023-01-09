using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class QuestMenu : MonoBehaviour
    {
        [SerializeField] QuestMenuEntry _entryPrefab;

        [SerializeField] Transform _contentParent;
        [SerializeField] Button _changeToStartedButton;
        [SerializeField] Button _changeToFinishedButton;
        
        [SerializeField] ActiveQuestDisplayer _activeQuestDisplayer;
        [SerializeField] ToggleGroup _toggleGroup;

        QuestState _activeQuestState = QuestState.Started;

        void Start()
        {
            _changeToStartedButton.onClick.AddListener(ChangeStateToStarted);
            _changeToFinishedButton.onClick.AddListener(ChangeStateToFinished);
        }

        void OnEnable()
        {
            if (_activeQuestState == QuestState.Started) ChangeStateToStarted();
            else ChangeStateToFinished();
        }

        void ChangeStateToStarted()
        {
            _activeQuestState = QuestState.Started;

            ClearQuestMenu();
            foreach (var quest in QuestManager.Instance.StartedQuests)
            {
                var entry = Instantiate(_entryPrefab, _contentParent);
                entry.InitializeEntry(quest.QuestData.Name, _activeQuestDisplayer.ActiveQuest == quest, quest, _activeQuestDisplayer, _toggleGroup);
            }
        }

        void ChangeStateToFinished()
        {
            _activeQuestState = QuestState.Finished;

            ClearQuestMenu();
            foreach (var quest in QuestManager.Instance.FinishedQuests)
            {
                var entry = Instantiate(_entryPrefab, _contentParent);
                entry.InitializeEntry(quest.QuestData.Name, _activeQuestDisplayer.ActiveQuest == quest, quest, _activeQuestDisplayer, _toggleGroup);
            }
        }

        void ClearQuestMenu()
        {
            for (int i = _contentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_contentParent.GetChild(i).gameObject);
            }
        }
    }
}
