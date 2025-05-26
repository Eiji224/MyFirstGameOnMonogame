using System;
using Microsoft.Xna.Framework;

namespace Fathom;

public class LevelGenerator
{
    private readonly Random _random;
    
    private const int EmptyTile = 0;
    private const int GroundTile = 1;
    private const int PlatformTile = 2;

    public LevelGenerator(int? seed = null)
    {
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
    }

    public void GenerateLevel(TileMap tileMap)
    {
        GenerateGround(tileMap);
        GeneratePlatforms(tileMap);
        CleanLevel(tileMap);
    }

    private void GenerateGround(TileMap tileMap)
    {
        for (var x = 0; x < tileMap.Width; x++)
        {
            for (var y = tileMap.Height - 3; y < tileMap.Height; y++)
            {
                tileMap.SetTile(x, y, GroundTile);
            }
        } 
    }

    private void GeneratePlatforms(TileMap tileMap)
    {
        for (var y = 5; y < tileMap.Height - 4; y++)
        {
            for (var x = 0; x < tileMap.Width; x++)
            {
                var currentTile = tileMap.GetTile(x, y);
                
                if (currentTile != EmptyTile || _random.Next(20) != 0)
                    continue;
                
                var platformLength = _random.Next(3, 6);
                for (var i = 0; i < platformLength && x + i < tileMap.Width; i++)
                {
                    tileMap.SetTile(x + i, y, PlatformTile);
                }
                
                x += platformLength + 7; // Пропускаем длину платформы и длину прыжка (7 блоков)
            }
        }
    }

    private bool IsIsolated(TileMap tileMap, int x, int y)
    {
        // Тайл считается изолированным, если вокруг него нет других твердых тайлов
        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                
                var checkX = x + i;
                var checkY = y + j;

                if (checkX < 0 || checkX >= tileMap.Width || checkY < 0 || checkY >= tileMap.Height)
                    continue;
                
                var neighborTile = tileMap.GetTile(checkX, checkY);
                if (neighborTile != EmptyTile)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void CleanLevel(TileMap tileMap)
    {
        for (var x = 0; x < tileMap.Width; x++)
        {
            for (var y = 0; y < tileMap.Height; y++)
            {
                var currentTile = tileMap.GetTile(x, y);
                if (currentTile != EmptyTile && IsIsolated(tileMap, x, y))
                {
                    tileMap.SetTile(x, y, EmptyTile);
                }

                if (currentTile != EmptyTile && tileMap.GetTile(x, y + 1) == GroundTile)
                {
                    tileMap.SetTile(x, y, EmptyTile);
                }
            }
        }
    }
} 