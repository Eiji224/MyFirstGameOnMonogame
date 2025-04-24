using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class Player(Vector2 position, int width, int height)
{
    public Vector2 Position { get; private set; } = position;
    public Vector2 Direction { get; private set; } = Vector2.Zero;
    public Rectangle BoundingBox { get; private set; } = new((int)position.X, (int)position.Y, width, height);

    public Vector2 Velocity { get; private set; } = Vector2.Zero;

    public void Move(Vector2 direction)
    {
        Position += direction;
        Velocity += direction;
        Direction = direction;
    }

    public void MoveByGravitation()
    {
        Velocity += new Vector2(0, 0.1f);
        Move(Velocity);
    }
    public void ResetVelocity() => Velocity = Vector2.Zero;
}