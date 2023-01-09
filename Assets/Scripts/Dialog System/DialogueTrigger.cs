using UnityEngine;
using System;

namespace Game
{
	public class DialogueTrigger : MonoBehaviour, IPersistant
    {
		struct PersistantData
        {
			public bool FlowCreated;
        }

		[SerializeField] GameStory _gameStory;
		[SerializeField] string _dialogueKnot;
		[SerializeField] CombatEntityIdentifier _triggerEntity;
		[SerializeField] float _triggerRadius = 5f;
		[SerializeField] GameObject _startDialogueButton;

		Transform _transform;

		ContactFilter2D _contactFilter;
		bool _flowCreated = false;
		readonly Collider2D[] _foundEntities = new Collider2D[5];
		Collider2D _entityThatTriggeredDialogue;

		public event Action<GameStory> OnDialogueTriggered;
		public event Action<GameStory> OnDialogueResumed;
		public event Action<GameStory> OnDialoguePaused;

        void Awake()
        {
			_transform = transform;
			_contactFilter = new ContactFilter2D();
			_contactFilter.useTriggers = true;
		}

        void Start()
        {
			_startDialogueButton.SetActive(false);
			_gameStory.OnFlowSwitched += HandleFlowSwitched;
		}

		void Update()
		{
			if (_entityThatTriggeredDialogue == null)
			{
				WaitForTrigger();
			}
			else
			{
				WaitForTriggererLeave();
			}
        }

        void OnDestroy()
        {
			_gameStory.OnFlowSwitched -= HandleFlowSwitched;
		}

		void HandleFlowSwitched(string flowName)
        {
			if (flowName != _dialogueKnot)
            {
				OnDialoguePaused?.Invoke(_gameStory);
            }
		}

		void WaitForTrigger()
        {
			if (Physics2D.OverlapCircle(_transform.position, _triggerRadius, _contactFilter, _foundEntities) < 1) return;

			foreach (var foundEntity in _foundEntities)
			{
				if (foundEntity &&
					foundEntity.TryGetComponent<CombatEntity>(out var foundCombatEntity) &&
					foundCombatEntity.Identifier == _triggerEntity &&
					foundCombatEntity.IsVisibleBy(_transform.position) &&
					Vector2.Distance(foundEntity.transform.position, transform.position) < _triggerRadius)
				{
					_startDialogueButton.SetActive(true);
					_entityThatTriggeredDialogue = foundEntity;
					break;
				}
			}
		}

		void WaitForTriggererLeave()
		{
			if (Vector2.Distance(_entityThatTriggeredDialogue.transform.position, transform.position) > _triggerRadius * 1.01f)
			{
				_startDialogueButton.SetActive(false);
				_entityThatTriggeredDialogue = null;
				OnDialoguePaused?.Invoke(_gameStory);
			}
		}

		public void StartDialogue()
        {
			_startDialogueButton.SetActive(false);
			if (!_flowCreated)
			{
				_gameStory.SwitchFlow(_dialogueKnot);
				_gameStory.ChoosePath(_dialogueKnot);
				_flowCreated = true;
				OnDialogueTriggered?.Invoke(_gameStory);
			}
			else
			{
				_gameStory.SwitchFlow(_dialogueKnot);
				OnDialogueResumed?.Invoke(_gameStory);
			}
		}

		public void ChoosePath(string path)
        {
			StartDialogue();
			_gameStory.ChoosePath(path);
			OnDialogueTriggered?.Invoke(_gameStory);
		}

        public void Load(object data, IGameDataHandler dataHandler)
		{
			var loadedData = dataHandler.ToObject<PersistantData>(data);
			_flowCreated = loadedData.FlowCreated;
		}

		public object Save()
		{
			return new PersistantData { FlowCreated = _flowCreated };
		}
    }
}
