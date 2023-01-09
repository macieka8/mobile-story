using System;
using UnityEngine;

namespace Game
{
    public class PathRequestManager : MonoBehaviour
    {
        static PathRequestManager _instance;

        Pathfinding _pathfinding;

        void Awake()
        {
            _instance = this;
            _pathfinding = GetComponent<Pathfinding>();
        }

        public static void RequestPath(Vector2 startPosition, Vector2 endPosition, Action<PathResponse> callback)
        {
            var request = new PathRequest(startPosition, endPosition, callback);
            _instance._pathfinding.StartFindPath(request);
        }
    }
}
