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
            _startingPrisms = Math.Max(1, startingPrisms);
            _prisms = Enumerable.Range(0, _startingPrisms)
                .Select(i => RectangularPrismFactory.MakeStandard(new Vector3(0, i * RectangularPrismFactory.TileHeight, 0)))
                .ToList();
        }

        public RectangularPrism Top
        {
            get { return _prisms.Last(); }
        }

        public void Push(RectangularPrism prism)
        {
            _prisms.Add(prism);
        }

        public RectangularPrism CreateNextUnboundPrism()
        {
            var top = Top;
            return RectangularPrismFactory.Make(
                new Size3(top.Size.X, top.Size.Y, top.Size.Z),
                new Vector3(
                    top.Position.X,
                    _prisms.Count*RectangularPrismFactory.TileHeight,
                    top.Position.Z),
                SceneColors.NextPrismColor());
        }
    }
}