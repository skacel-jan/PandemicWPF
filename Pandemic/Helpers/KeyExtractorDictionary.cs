using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pandemic
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

        protected KeyExtractorDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public void Add(TValue value)
        {
            Add(_extractor(value), value);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public override void OnDeserialization(object sender)
        {
            base.OnDeserialization(sender);
        }
    }
}