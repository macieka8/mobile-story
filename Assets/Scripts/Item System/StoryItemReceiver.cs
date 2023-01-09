using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public class StoryItemReceiver : MonoBehaviour
    {
        static readonly string ITEM_TAG_START = "Item.";
        static readonly string ITEM_COUNT_SIGN = ":";

        [SerializeField] GameStory _story;
        Inventory _inventory;

        void Awake()
        {
            _inventory = GetComponent<Inventory>();
        }

        void OnEnable()
        {
            _story.TagResolverManager.AddTagListener(ITEM_TAG_START, HandleItemReceived);
        }

        void OnDisable()
        {
            _story.TagResolverManager.RemoveTagListener(ITEM_TAG_START, HandleItemReceived);
        }

        void HandleItemReceived(string tag)
        {
            int countSignIndex = tag.LastIndexOf(ITEM_COUNT_SIGN);
            int count = int.Parse(tag.Substring(countSignIndex + 1));
            string itemName = tag.Substring(ITEM_TAG_START.Length, countSignIndex - ITEM_TAG_START.Length);

            var loadItemHandler = Addressables.LoadAssetAsync<Item>(itemName);
            loadItemHandler.Completed += (AsyncOperationHandle<Item> obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    _inventory.AddItem(new ItemSlot() { Item = obj.Result, Count = count });
                }
                else
                {
                    Debug.LogWarning("Failed to load item asset");
                }
            };
        }
    }
}
