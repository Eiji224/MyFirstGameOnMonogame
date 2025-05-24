using Microsoft.Xna.Framework;

namespace Fathom;

public class Camera(Vector2 viewportSize, Vector2 levelBounds)
{
    public Matrix TransformMatrix { get; private set; }
    
    private Vector2 Position { get; set; }
    private float Zoom { get; } = 1f;

    public void Follow(Player target)
    {
        var targetCenter = new Vector2(
            target.Position.X + target.BoundingBox.Width / 2,
            target.Position.Y + target.BoundingBox.Height / 2
        );
        
        Position = targetCenter - viewportSize / 2 / Zoom;
        
        Position = new Vector2(
            MathHelper.Clamp(Position.X, 0, levelBounds.X - viewportSize.X / Zoom),
            MathHelper.Clamp(Position.Y, 0, levelBounds.Y - viewportSize.Y / Zoom)
        );
        
        TransformMatrix = Matrix.CreateScale(Zoom) *
                         Matrix.CreateTranslation(new Vector3(-Position, 0));
    }

    public bool IsInView(Rectangle objectBounds)
    {
        var cameraBounds = new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            (int)(viewportSize.X / Zoom),
            (int)(viewportSize.Y / Zoom)
        );
        return cameraBounds.Intersects(objectBounds);
    }
}