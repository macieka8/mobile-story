using UnityEngine;
using TMPro;

namespace Game
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _dayText;
        [SerializeField] TextMeshProUGUI _timeText;

        void Update()
        {
            _dayText.text = $"Day {TimeSystem.Instance.Day}";
            _timeText.text = $"{TimeSystem.Instance.Hour:00}:{TimeSystem.Instance.Minute:00}";
        }
    }
}
