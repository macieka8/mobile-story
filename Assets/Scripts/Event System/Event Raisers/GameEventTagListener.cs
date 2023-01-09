using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public class GameEventTagListener : MonoBehaviour
    {
        static readonly string GAMEEVENT_TAG = "RaiseGameEvent.";

        [SerializeField] GameStory _story;

        void OnEnable()
        {
            _story.TagResolverManager.AddTagListener(GAMEEVENT_TAG, HandleGameEventFound);
        }

        void OnDisable()
        {
            _story.TagResolverManager.RemoveTagListener(GAMEEVENT_TAG, HandleGameEventFound);
        }

        void HandleGameEventFound(string tag)
        {
            var gameEventName = tag.Substring(GAMEEVENT_TAG.Length);

            var loadGameEventHandle = Addressables.LoadAssetAsync<VoidGameEvent>(gameEventName);
            loadGameEventHandle.Completed += (AsyncOperationHandle<VoidGameEvent> asyncOp) =>
            {
                asyncOp.Result.RaiseEvent();
            };
        }
    }
}
