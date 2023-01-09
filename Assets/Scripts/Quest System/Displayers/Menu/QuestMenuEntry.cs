using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class QuestMenuEntry : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _questNameText;
        [SerializeField] Toggle _isActiveToggle;

        Quest _quest;
        ActiveQuestDisplayer _activeQuestDisplayer;

        public void InitializeEntry(string name, bool toggleOn, Quest quest, ActiveQuestDisplayer activeQuestDisplayer, ToggleGroup toggleGroup)
        {
            _quest = quest;
            _activeQuestDisplayer = activeQuestDisplayer;
            _questNameText.text = name;

            _isActiveToggle.group = toggleGroup;
            _isActiveToggle.isOn = toggleOn;
            _isActiveToggle.onValueChanged.AddListener(HandleToggleChanged);
        }

        void HandleToggleChanged(bool value)
        {
            if (value)
            {
                _activeQuestDisplayer.SetNewActiveQuest(_quest);
            }
        }

        public void SetToggleActive(bool on)
        {
            _isActiveToggle.isOn = on;
        }
    }
}
