using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Game
{
    public class PersistantDataManager : MonoBehaviour
    {
        [SerializeField] int _version;
        [SerializeField] string _fileName;

        [Header("Persistant ScriptableObjects")]
        [SerializeField] GameStory _story;

        IGameDataHandler _fileDataHandler;

        public string FileName => _fileName;

        void Awake()
        {
            _fileDataHandler = GetComponent<IGameDataHandler>();
        }

        public void Save()
        {
            if (CombatSystem.Instance.IsPlayerInCombat) return;
            GameData gameData = new GameData();

            gameData.story = _story.Save();
            foreach (var persitantObject in FindObjectsOfType<PersistantObject>(true))
            {
                gameData.objects.Add(persitantObject.Save());
            }

            _fileDataHandler.Save(gameData);
        }

        public void Load()
        {
            var asyncOp = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            asyncOp.completed += (oper) =>
            {
                var gameData = _fileDataHandler.Load();

                _story.Load(gameData.story, _fileDataHandler);
                foreach (var persistantObject in FindObjectsOfType<PersistantObject>(true))
                {
                    var data = gameData.objects.FirstOrDefault(item => item.guid == persistantObject.Id);
                    if (data != null)
                    {
                        persistantObject.Load(data, _fileDataHandler);
                    }
                    else
                    {
                        Destroy(persistantObject.gameObject);
                    }
                }
            };
        }
    }
}
