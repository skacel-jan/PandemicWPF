using System;
using System.Collections.Generic;

namespace Pandemic
{
    public class KeyExtractorDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private readonly Func<TValue, TKey> _extractor;

        public KeyExtractorDictionary(Func<TValue, TKey> extractor)
        {
            _extractor = extractor;
        }

        public void Add(TValue value)
        {
            Add(_extractor(value), value);
        }
    }
}