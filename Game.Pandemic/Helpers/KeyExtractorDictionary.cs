using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Game.Pandemic.Helpers
{
    [Serializable]
    public class KeyExtractorDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private readonly Func<TValue, TKey> _extractor;

        public KeyExtractorDictionary(Func<TValue, TKey> extractor)
        {
            _extractor = extractor;
        }

        public KeyExtractorDictionary(Func<TValue, TKey> extractor, IEnumerable<TValue> values) :this(extractor)
        {
            foreach (var value in values)
            {
                Add(value);
            }
        }

        public void Add(TValue value)
        {
            Add(_extractor(value), value);
        }
    }
}
