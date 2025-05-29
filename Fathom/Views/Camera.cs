using Microsoft.Xna.Framework;

namespace Fathom;

public class Camera
{
    public Vector2 Position { get; private set; }
    public Vector2 ViewportSize { get; }
    public Matrix TransformMatrix { get; private set; }
    
    private Vector2 _levelBounds;
    private float Zoom { get; } = 1f;

    public Camera(Vector2 viewportSize, Vector2 levelBounds)
    {
        ViewportSize = viewportSize;
        _levelBounds = levelBounds;
    }

    /*public void ShowEntireLevel()
    {
        var zoomX = ViewportSize.X / _levelBounds.X;
        var zoomY = ViewportSize.Y / _levelBounds.Y;
        Zoom = MathHelper.Min(zoomX, zoomY);
        
        Position = new Vector2(
            (_levelBounds.X - ViewportSize.X / Zoom) / 2,
            (_levelBounds.Y - ViewportSize.Y / Zoom) / 2
        );

        UpdateTransformMatrix();
    }*/

    public void Follow(Player target)
    {
        Position = new Vector2(
            target.Position.X + (target.BoundingBox.Width / 2) - (ViewportSize.X / Zoom / 2),
            target.Position.Y + (target.BoundingBox.Height / 2) - (ViewportSize.Y / Zoom / 2)
        );
        
        Position = new Vector2(
            MathHelper.Clamp(Position.X, 0, _levelBounds.X - ViewportSize.X / Zoom),
            MathHelper.Clamp(Position.Y, 0, _levelBounds.Y - ViewportSize.Y / Zoom)
        );
        
        UpdateTransformMatrix();
    }

    private void UpdateTransformMatrix()
    {
        TransformMatrix = Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                         Matrix.CreateScale(Zoom);
    }

    /*public bool IsInView(Rectangle objectBounds)
    {
        var cameraBounds = new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            (int)(ViewportSize.X / Zoom),
            (int)(ViewportSize.Y / Zoom)
        );
        return cameraBounds.Intersects(objectBounds);
    }*/
}