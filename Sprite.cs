using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public class Sprite()
{
    public Texture2D Texture { get; }
    public Rectangle Position { get; set; }

    public Sprite(Texture2D texture, Rectangle position) : this()
    {
        Texture = texture;
        Position = position;
    }

    public void MoveTo(int x, int y) => Position = new Rectangle(Position.X + x, Position.Y + y,
        Position.Width, Position.Height);
}