using System;

namespace Game
{
    public class TagResolver
    {
        protected string _tagName;
        int _subscriptions = 0;
        protected event Action<string> _onTagFound;

        public string TagName => _tagName;
        public int SubscriptionCount => _subscriptions;
        public event Action<string> OnTagFound
        {
            add { _subscriptions++; _onTagFound += value; }
            remove { _subscriptions--; _onTagFound -= value; }
        }
        
        public TagResolver(string tag)
        {
            _tagName = tag;
        }
        
        public void InvokeOnTagFound(string tag) => _onTagFound?.Invoke(tag);
    }
}
