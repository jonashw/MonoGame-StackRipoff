using System;

namespace MonoGame_StackRipoff
{
    public struct Size3
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Size3(float x, float y, float z)
        {
            X = Math.Abs(x);
            Y = Math.Abs(y);
            Z = Math.Abs(z);
        }
    }
}