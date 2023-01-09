using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TransformVariableRegister : MonoBehaviour
    {
        [SerializeField] TransformVariable transformVariable;

        void Awake()
        {
            transformVariable.Value = transform;
        }

        void OnDestroy()
        {
            if (transformVariable.Value == transform)
                transformVariable.Value = null;
        }
    }
}
