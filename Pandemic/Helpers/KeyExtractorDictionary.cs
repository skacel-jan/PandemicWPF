using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    class KeyExtractorDictionary<TKey, TValue> : Dictionary<TKey, TValue>
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
