using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fathom;

public class LevelModel
{
    public Player Player { get; set; }
    public List<Platform> Platforms { get; set; }

    public void Initialize()
    {
        Player = new Player(new Vector2(200, 20), 32, 32);
        Platforms = [ new Platform(new Vector2(200, 100), 32, 16) ];
    }
}