using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    [System.Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid) : base(guid) {}
    }

    [CreateAssetMenu(menuName = "Soundtrack")]
    public class Soundtrack : ScriptableObject
    {
        [SerializeField] List<AssetReferenceAudioClip> _trackAssets;

        public int Count => _trackAssets.Count;
        public AsyncOperationHandle<IList<AudioClip>> LoadTrackAsync()
        {
            var handle = Addressables.LoadAssetsAsync<AudioClip>(_trackAssets, op => { }, Addressables.MergeMode.Union);
            return handle;
        }
    }
}
