using System.Collections.Generic;

namespace MonoGame_StackRipoff
{
    namespace MonoGameInscribedTriangles
    {
        public class CircularArray<T>
        {
            private readonly T[] _items;
            private int _index;

            public CircularArray(params T[] items)
            {
                _items = items;
            }

            public IEnumerable<T> All { get { return _items; } }
            public int Count { get { return _items.Length; } }

            public T GetCurrent()
            {
                return _items[_index];
            }

            public void Next(int n = 1)
            {
                _index += n;
                if (_index >= _items.Length)
                {
                    _index -= _items.Length;
                }
            }

            public void Prev()
            {
                _index--;
                if (_index < 0)
                {
                    _index = _items.Length - 1;
                }
            }
        }
    }
}