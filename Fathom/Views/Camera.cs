using Microsoft.Xna.Framework;

namespace Fathom;

public class Camera
{
    public Vector2 Position { get; private set; }
    public Vector2 ViewportSize { get; }
    public Matrix TransformMatrix { get; private set; }
    
    private Vector2 _levelBounds;
    private const float _zoom = 1f;

    public Camera(Vector2 viewportSize, Vector2 levelBounds)
    {
        ViewportSize = viewportSize;
        _levelBounds = levelBounds;
    }

    public void Follow(Player target)
    {
        Position = new Vector2(
            target.Position.X + (target.BoundingBox.Width / 2) - (ViewportSize.X / _zoom / 2),
            target.Position.Y + (target.BoundingBox.Height / 2) - (ViewportSize.Y / _zoom / 2)
        );
        
        Position = new Vector2(
            MathHelper.Clamp(Position.X, 0, _levelBounds.X - ViewportSize.X / _zoom),
            MathHelper.Clamp(Position.Y, 0, _levelBounds.Y - ViewportSize.Y / _zoom)
        );
        
        UpdateTransformMatrix();
    }

    private void UpdateTransformMatrix()
    {
        TransformMatrix = Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                         Matrix.CreateScale(_zoom);
    }
}