using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Pandemic
{
    public class CircularCollection<T> : ObservableObject, IEnumerable<T>
    {
        private readonly Queue<T> queue;

        public CircularCollection(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            queue = new Queue<T>(items);
        }

        public T Current => queue.Peek();

        public int Count => queue.Count;

        public IEnumerator<T> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        public T Next()
        {
            T item = queue.Dequeue();
            queue.Enqueue(item);

            RaisePropertyChanged(nameof(Current));

            return item;
        }
    }
}