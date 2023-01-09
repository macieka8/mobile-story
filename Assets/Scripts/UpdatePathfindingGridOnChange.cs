using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UpdatePathfindingGridOnChange : MonoBehaviour
    {
        [SerializeField] PathfindingGrid _grid;

        void OnDisable()
        {
            if (_grid != null)
            {
                _grid.RecreateGrid();
            }
        }
    }
}
