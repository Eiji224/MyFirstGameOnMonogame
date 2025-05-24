using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public interface IView
{
    public void LoadContent(ContentManager content);
    public void Draw(SpriteBatch spriteBatch, Camera camera);
}