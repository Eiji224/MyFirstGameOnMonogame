using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class Platform
{
    public Vector2 Position { get; private set; }
    public Rectangle BoundingBox { get; private set; }

    public Platform(Vector2 position, int width, int height)
    {
        Position = position;
        BoundingBox = new Rectangle((int)position.X, (int)position.Y, width, height);
    }
}