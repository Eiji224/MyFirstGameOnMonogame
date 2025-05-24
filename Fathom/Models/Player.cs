using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class Player(Vector2 position, int width, int height)
{
    public Vector2 Position { get; private set; } = position;
    public Vector2 Direction { get; private set; } = Vector2.Zero;
    public Rectangle BoundingBox { get; private set; } = new((int)position.X, (int)position.Y, width, height);
    public Vector2 Velocity { get; set; } = Vector2.Zero;
    public bool IsOnGround { get; set; } = false;

    public void Move(Vector2 velocity)
    {
        Position += velocity;
        Direction = velocity;
        UpdateBoundingBox();
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
        UpdateBoundingBox();
    }

    public void ResetVelocity() => Velocity = Vector2.Zero;

    public void UpdateBoundingBox()
    {
        BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height);
    }
}