using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace Game
{
    public class JsonDataHandler : MonoBehaviour, IGameDataHandler
    {
        PersistantDataManager _persistantDataManager;
        JsonSerializerSettings _settings;

        void Awake()
        {
            _persistantDataManager = GetComponent<PersistantDataManager>();
        }

        void Start()
        {
            _settings = new JsonSerializerSettings();
            _settings.TypeNameHandling = TypeNameHandling.None;
            _settings.Formatting = Formatting.Indented;
        }

        public void Save(GameData gameData)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, _persistantDataManager.FileName);

            var json = JsonConvert.SerializeObject(gameData, _settings);
            File.WriteAllText(fullPath, json);

            Debug.Log($"Path: {fullPath}");
        }

        public GameData Load()
        {
            var fullPath = Path.Combine(Application.persistentDataPath, _persistantDataManager.FileName);

            var json = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<GameData>(json, _settings);
        }

        public T ToObject<T>(object obj)
        {
            return ((Newtonsoft.Json.Linq.JToken)obj).ToObject<T>();
        }
    }
}
