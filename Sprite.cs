using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class Sprite()
{
    public Texture2D Texture { get; }
    public Rectangle SpriteRect { get; set; }

    public Sprite(Texture2D texture, Rectangle spriteRect) : this()
    {
        Texture = texture;
        SpriteRect = spriteRect;
    }

    public void MoveTo(int x, int y) => SpriteRect = new Rectangle(SpriteRect.X + x, SpriteRect.Y + y,
        SpriteRect.Width, SpriteRect.Height);
}