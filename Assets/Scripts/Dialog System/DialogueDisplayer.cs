using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class DialogueDisplayer : MonoBehaviour, IPersistant
    {
		struct PersistantData
        {
			public string CurrentPath;
        }

		[Header("Display References")]
		[SerializeField] TextDisplayer _storyText;
		[SerializeField] Transform _choicesParent;
		[SerializeField] Button _buttonPrefab;

		[Header("Story References")]
		[SerializeField] DialogueTrigger _trigger;
		[SerializeField] GameStory _story;

		string _currentPath;

		void OnEnable()
        {
			_trigger.OnDialogueTriggered += HandleDialogueTriggered;
            _trigger.OnDialoguePaused += HandleDialoguePaused;
			_trigger.OnDialogueResumed += HandleDialogueResumed;

			_storyText.OnTextDisplayed += HandleTextDisplayed;
		}

        void OnDisable()
		{
			_trigger.OnDialogueTriggered -= HandleDialogueTriggered;
			_trigger.OnDialoguePaused -= HandleDialoguePaused;
			_trigger.OnDialogueResumed -= HandleDialogueResumed;

			_storyText.OnTextDisplayed -= HandleTextDisplayed;
		}

		void HandleTextDisplayed()
        {
			ContinueStory();
		}

        void HandleDialogueTriggered(GameStory obj)
        {
			_storyText.SetPause(false);
			ContinueStory();
		}

		void HandleDialogueResumed(GameStory obj)
		{
			_story.ChoosePath(_currentPath);
			ContinueStory();
			_storyText.SetPause(false);
		}

		void HandleDialoguePaused(GameStory obj)
        {
			if (!string.IsNullOrEmpty(_story.CurrentPath))
					_currentPath = _story.CurrentPath;

			_storyText.SetPause(true);
			ClearDisplayChoices();
		}

		void ContinueStory()
		{
			if (!string.IsNullOrEmpty(_story.CurrentPath))
				_currentPath = _story.CurrentPath;
			if (_story.CanContinue)
			{
				_storyText.DisplayText(_story.Continue().Trim());
				ClearDisplayChoices();
			}
			else
            {
				DisplayChoices();
            }
		}

		void DisplayChoices()
        {
			ClearDisplayChoices();
			if (_story.Choices.Count > 0)
			{
				CreateDisplayChoices();
			}
		}

		void CreateDisplayChoices()
		{
			foreach (var choice in _story.Choices)
			{
				var button = Instantiate(_buttonPrefab, _choicesParent);

				var choiceText = button.GetComponentInChildren<TextMeshProUGUI>();
				choiceText.text = choice.text.Trim();

				button.onClick.AddListener(() => {
					_story.ChooseChoiceIndex(choice.index);
					ContinueStory();
				});
			}
		}

		void ClearDisplayChoices()
        {
			for (int i = _choicesParent.childCount - 1; i >= 0; i--)
			{
				Destroy(_choicesParent.GetChild(i).gameObject);
			}
		}

        public object Save()
        {
			return new PersistantData { CurrentPath = _currentPath };
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
			var decodedData = dataHandler.ToObject<PersistantData>(data);
			_currentPath = decodedData.CurrentPath;
        }
    }
}
