using System.Collections.Generic;
using System.Collections.Immutable;

namespace Common.Collection
{
    public record TagDictionary
    {
        private ImmutableDictionary<string, ImmutableHashSet<string>> Tags { get; }

        private TagDictionary(Dictionary<string, HashSet<string>> tags)
        {
            Tags = tags.ToImmutableDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ToImmutableHashSet()
            );
        }

        public TagDictionary(List<string> tags) : this(Parser(tags))
        {
        }
        
        public ImmutableHashSet<string> this[string tag] => Tags[tag];

        private static Dictionary<string, HashSet<string>> Parser(List<string> tags)
        {
            var pairs = new Dictionary<string, HashSet<string>>();

            if (tags == null || tags.Count == 0)
                return pairs;

            foreach (var tag in tags)
            {
                var (tagName, tagValue) = SplitTag(tag);
                if (!pairs.ContainsKey(tagName))
                    pairs.Add(tagName, new HashSet<string>());

                pairs[tagName].Add(tagValue);
            }

            return pairs;
        }

        private static (string, string) SplitTag(string tag)
        {
            var index = tag.IndexOf(':');

            return index == -1
                ? (tag, tag)
                : (tag[..index], tag[(index + 1)..]);
        }
    }
}