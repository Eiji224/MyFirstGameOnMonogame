using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Fathom;

public class LevelGenerator
{
    private readonly Random _random;
    private const int _maxJumpHeight = 3;
    private const int _maxJumpDistance = 7;

    private (int, int)[][] AnchorPoints =
    [
        [(0, 0), (7, 0), (4, 1), (7, 2), (1, 3), (5, 5)],
        [(1, 0), (6, 0), (3, 1), (0, 2), (4, 3), (0, 5)]
    ];
    
    public LevelGenerator(int? seed = null)
    {
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
    }

    public void GenerateLevel(TileMap tileMap)
    {
        GenerateGround(tileMap);
        var sections = SplitLevelSections(tileMap);
        foreach (var section in sections)
        {
            foreach (var tile in section)
            {
                tileMap.SetTile(tile);
            }
        }
        //GeneratePlatforms(tileMap);
        CleanLevel(tileMap);
    }

    private void GenerateGround(TileMap tileMap)
    {
        for (var x = 0; x < tileMap.Width; x++)
        {
            for (var y = tileMap.Height - 3; y < tileMap.Height; y++)
            {
                tileMap.SetTile(new Tile(x, y, TileType.Ground));
            }
        } 
    }

    private List<Tile[,]> SplitLevelSections(TileMap tileMap)
    {
        var tilesSections = new List<Tile[,]>();
        var (sectWidth, sectHeight) = (tileMap.Width / 40, tileMap.Height / 37);
        
        for (var sizeHeight = 0; sizeHeight < tileMap.Height; sizeHeight += 10)
        {
            for (var sizeWidth = 0; sizeWidth < tileMap.Width; sizeWidth += 10)
            {
                var tiles = TileMap.InitializeMap(sectWidth, sectHeight);
                
                var anchorTemplateIndex = _random.Next(0, AnchorPoints.Length);
                var platformsLeft = _random.Next(1, 4);
                for (; platformsLeft > 0; platformsLeft--)
                {
                    var anchorPointIndex = _random.Next(0, AnchorPoints[anchorTemplateIndex].Length);
                    var anchorPoint = AnchorPoints[anchorTemplateIndex][anchorPointIndex];
                    
                    var platformLength = _random.Next(3, 6);
                    for (var i = 0; i < platformLength && anchorPoint.Item1 + i < sectWidth; i++)
                    {
                        tiles[anchorPoint.Item1 + i, anchorPoint.Item2] = new Tile(sizeWidth + anchorPoint.Item1 + i,
                            sizeHeight + anchorPoint.Item2, TileType.Platform);
                    }
                }

                /*for (var y = 0; y < sectHeight; y++)
                {
                    for (var x = 0; x < sectWidth; x++)
                    {
                        if (y > sectHeight - 1 || x > sectWidth - 1)
                        {
                            break;
                        }
                        
                        var platformLength = _random.Next(3, 6);
                        for (var i = 0; i < platformLength && x + i < sectWidth; i++)
                        {
                            tiles[x + i, y] = new Tile(sizeWidth + x + i, sizeHeight + y, TileType.Platform);
                        }

                        x += platformLength;
                        y += 2;
                        platformsLeft--;
                    }
                }*/

                tilesSections.Add(tiles);
            }
        }
        
        return tilesSections;
    }

    private void GeneratePlatforms(TileMap tileMap)
    {
        for (var y = 5; y < tileMap.Height - 3; y++)
        {
            for (var x = 0; x < tileMap.Width - 4; x++)
            {
                var currentTile = tileMap.GetTile(x, y);
                
                if (currentTile != TileType.Empty || _random.Next(25) != 0)
                    continue;
                
                var platformLength = _random.Next(3, 6);

                /*var isPlatformsGlued = false;
                for (var i = 0; i < platformLength && x + i < tileMap.Width; i++)
                {
                    if (isPlatformsGlued)
                        break;
                    
                    for (var j = -2; j <= 2; j++)
                    {
                        if (tileMap.GetTile(x + i, y + j) == GroundTile)
                        {
                            x += platformLength + 7;
                            isPlatformsGlued = true;
                            break;
                        }
                    }
                }
                if (isPlatformsGlued)
                    continue;*/
                
                for (var i = 0; i < platformLength && x + i < tileMap.Width; i++)
                {
                    tileMap.SetTile(new Tile(x + i, y, TileType.Platform));
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
                if (neighborTile != TileType.Empty)
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
                if (currentTile != TileType.Empty && IsIsolated(tileMap, x, y))
                {
                    tileMap.SetTile(new Tile(x, y));
                }
            }
        }
    }
} 