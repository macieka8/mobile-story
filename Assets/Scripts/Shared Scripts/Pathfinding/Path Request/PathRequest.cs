using System;
using UnityEngine;

namespace Game
{
    public struct PathRequest
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public Action<PathResponse> Callback;

        public PathRequest(Vector2 startPosition, Vector2 endPosition, Action<PathResponse> callback)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            Callback = callback;
        }
    }
}
