using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VisualTests
{
    public class SceneDescriptor
    {
        public readonly string Name;
        public readonly Func<GraphicsDevice, ContentManager, Scene> Create;

        public SceneDescriptor(string name, Func<GraphicsDevice, ContentManager, Scene> create)
        {
            Name = name;
            Create = create;
        }
    }
}