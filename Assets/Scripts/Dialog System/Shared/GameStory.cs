using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using System;

namespace Game
{
    [CreateAssetMenu(menuName = "Story/Story")]
    public class GameStory : ScriptableObject, IPersistant
    {
        struct PersistantData
        {
            public string StoryJson;
        }


        [SerializeField] TextAsset _inkJSONAsset;
        [SerializeField] TagResolverManager _tagResolverManager;
        Story _story;

        public TagResolverManager TagResolverManager => _tagResolverManager;
        public bool CanContinue => _story.canContinue;
        public List<Choice> Choices => _story.currentChoices;
        public string CurrentPath => _story.state.currentPathString;
        public VariablesState VariableState => _story.variablesState;

        public event Action OnStoryContinue
        {
            add => _story.onDidContinue += value;
            remove => _story.onDidContinue -= value;
        }

        public event Action<string> OnFlowSwitched;

        void OnEnable()
        {
            if (_story == null)
            {
                RestartStory();
            }
        }

        void ResolveTags()
        {
            foreach (var tag in _story.currentTags)
            {
                _tagResolverManager.ResolveTag(tag);
            }
        }

        public string Continue()
        {
            var text = _story.Continue();
            ResolveTags();
            return text;
        }

        public void ChooseChoiceIndex(int choiceId)
        {
            _story.ChooseChoiceIndex(choiceId);
        }

        public void ChoosePath(string path)
        {
            _story.ChoosePathString(path);
        }

        public void SwitchFlow(string flowName)
        {
            OnFlowSwitched?.Invoke(flowName);
            _story.SwitchFlow(flowName);
        }

        public T GetVariable<T>(string variableName)
        {
            return (T)_story.variablesState[variableName];
        }

        public void SetVariable<T>(string variableName, T value)
        {
            _story.variablesState[variableName] = value;
        }

        public void AddListElement(string listName, string element)
        {
            var list = _story.variablesState[listName] as InkList;
            list.AddItem(element);
            _story.variablesState[listName] = list;
        }

        public void RestartStory()
        {
            _story = new Story(_inkJSONAsset.text);
            _tagResolverManager = new TagResolverManager();
        }

        public object Save()
        {
            return new PersistantData { StoryJson = _story.state.ToJson() };
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var jsonData = dataHandler.ToObject<PersistantData>(data);
            _story.state.LoadJson(jsonData.StoryJson);
        }
    }
}
