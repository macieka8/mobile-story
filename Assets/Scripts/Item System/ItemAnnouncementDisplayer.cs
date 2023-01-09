using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game
{
    public class ItemAnnouncementDisplayer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _addedItemText;
        [SerializeField] Image _addedItemImage;
        [SerializeField] float _timeToLive;

        float _currentTimeToLive;

        void Awake()
        {
            _currentTimeToLive = _timeToLive;
        }

        void Update()
        {
            _currentTimeToLive -= Time.deltaTime;
            if (_currentTimeToLive <= 0f)
            {
                Destroy(gameObject);
            }
        }

        public void Setup(ItemSlot addedItem)
        {
            _addedItemImage.sprite = addedItem.Item.Icon;
            _addedItemText.text = $"{addedItem.Item.name} x {addedItem.Count}";
        }
    }
}
