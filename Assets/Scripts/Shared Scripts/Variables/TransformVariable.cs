using UnityEngine;
using System;

namespace Game
{
    [CreateAssetMenu(menuName = "Variables/Transform")]
    public class TransformVariable : ScriptableObject
    {
        public Transform Value;
    }
}
