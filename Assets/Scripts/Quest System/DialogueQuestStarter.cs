using UnityEngine;

namespace Game
{
    public class DialogueQuestStarter : MonoBehaviour
    {
		[SerializeField] GameStory _story;
		[SerializeField] QuestData _quest;
		string _tagName;

		void Awake()
        {
			_tagName = $"Quest.Start.{_quest.Name}";
		}
		
		void OnEnable()
        {
			if (!QuestManager.Instance.HasQuest(_quest))
				_story.TagResolverManager.AddTagListener(_tagName, HandleTagResolve);
		}

		void OnDisable()
		{
			_story.TagResolverManager.RemoveTagListener(_tagName, HandleTagResolve);
		}

        void HandleTagResolve(string tag)
        {
			_quest.StartQuest();
			_story.TagResolverManager.RemoveTagListener(_tagName, HandleTagResolve);
		}
	}
}
