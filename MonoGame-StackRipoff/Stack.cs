using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame_StackRipoff
{
    public class Stack
    {
        private readonly List<RectangularPrism> _prisms;
        public IEnumerable<RectangularPrism> Prisms { get { return _prisms; } }

        public long Score
        {
            get { return _prisms.Count - _startingPrisms; }
        }

        private readonly int _startingPrisms;

        public Stack(int startingPrisms)
        {
            _startingPrisms = Math.Max(0, startingPrisms);
            _prisms = Enumerable.Range(0, _startingPrisms)
                .Select(i => RectangularPrismFactory.MakeStandard(new Vector3(0, i * RectangularPrismFactory.TileHeight, 0)))
                .ToList();
        }

        public void Push(RectangularPrism prism)
        {
            _prisms.Add(prism);
        }

        public void Pop()
        {
            if (_prisms.Count == 0)
            {
                return;
            }
            _prisms.RemoveAt(_prisms.Count - 1);
        }

        public RectangularPrism CreateNextUnboundPrism()
        {
            return RectangularPrismFactory.MakeStandard(
                new Vector3(
                    0,
                    _prisms.Count*RectangularPrismFactory.TileHeight,
                    0));
        }
    }
}