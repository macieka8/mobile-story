using System.Collections.Generic;
using System;

namespace Game
{
    public class TagResolverManager
    {
        List<TagResolver> _tagResolvers = new List<TagResolver>();

        public TagResolverManager() { }

        public void ResolveTag(string tag)
        {
            foreach (var tagResolver in _tagResolvers)
            {
                if (tag.StartsWith(tagResolver.TagName))
                {
                    tagResolver.InvokeOnTagFound(tag);
                    break;
                }
            }
        }

        public void AddTagListener(string tag, Action<string> fun)
        {
            var resolver = _tagResolvers.Find(tagResolver => tagResolver.TagName == tag);
            if (resolver == null)
            {
                resolver = new TagResolver(tag);
                _tagResolvers.Add(resolver);
            }
            resolver.OnTagFound += fun;
        }

        public void RemoveTagListener(string tag, Action<string> fun)
        {
            var resolver = _tagResolvers.Find(tagResolver => tagResolver.TagName == tag);
            if (resolver != null)
            {
                resolver.OnTagFound -= fun;
                if (resolver.SubscriptionCount < 1) _tagResolvers.Remove(resolver);
            }
        }
    }
}
