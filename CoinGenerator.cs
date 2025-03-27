using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame;

public static class CoinGenerator
{
    private static readonly Random _randomizer = new();
    
    public static void GenerateCoin(List<Sprite> coins, Texture2D texture, int screenWidth, int screenHeight)
    {
        var coinRect = new Rectangle(0, 0, texture.Width / 16, texture.Height / 16);
        bool areCoordinatesUnique;
    
        do
        {
            coinRect.X = _randomizer.Next(0, screenWidth - coinRect.Width);
            coinRect.Y = _randomizer.Next(0, screenHeight - coinRect.Height);

            areCoordinatesUnique = coins.All(coin => !coin.Position.Intersects(coinRect));
        }
        while (!areCoordinatesUnique);
    
        var newCoin = new Sprite(texture, coinRect);
        coins.Add(newCoin);
    }
}