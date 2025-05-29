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
        [
            (0, 0), 
            (7, 0), 
            (4, 1), 
            (7, 2), 
            (1, 3), 
            (5, 5)
            // -----------
            //(0, 0),
            //(1, 1)
        ],
        [
            (1, 0), 
            (6, 0), 
            (3, 1), 
            (0, 2), 
            (4, 3), 
            (0, 5)
            // --------------
            //(0, 1),
            //(1, 0)
        ]
    ];
    
    private readonly int _startX;
    private readonly int _startY;
    private readonly int _endX;
    private readonly int _endY;
    
    public LevelGenerator(int? seed = null, int width = 0, int height = 0)
    {
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
        
        _startY = height - 4;
        _startX = 4;
        _endY = 0;
        _endX = width - 4;
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
        
        if (!HasPathToEnd(tileMap))
        {
            AddMissingPlatforms(tileMap);
        }
        
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

                tilesSections.Add(tiles);
            }
        }
        
        return tilesSections;
    }
    
    private bool HasPathToEnd(TileMap tileMap)
    {
        var visited = new bool[tileMap.Width, tileMap.Height + 1];
        var queue = new Queue<(int x, int y)>();
        queue.Enqueue((_startX, _startY));
        visited[_startX, _startY] = true;

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            
            for (var dx = -_maxJumpDistance; dx <= _maxJumpDistance; dx++)
            {
                for (var dy = -_maxJumpHeight; dy <= _maxJumpHeight; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    var newX = x + dx;
                    var newY = y + dy;

                    if (newX < 0 || newX >= tileMap.Width || newY < 0 || newY >= tileMap.Height)
                        continue;

                    if (visited[newX, newY])
                        continue;

                    var tile = tileMap.GetTile(newX, newY);
                    if (tile == TileType.Platform || tile == TileType.Ground)
                    {
                        if (newX == _endX && newY == _endY)
                            return true;
                        visited[newX, newY] = true;
                        queue.Enqueue((newX, newY));
                    }
                }
            }
        }
        return false;
    }
    
    public void AddMissingPlatforms(TileMap tileMap)
    {
        var visited = new bool[tileMap.Width, tileMap.Height];
        var queue = new Queue<(int x, int y)>();
        queue.Enqueue((_startX, _startY));
        visited[_startX, _startY] = true;
        (int x, int y)? lastReachable = null;
        
        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            for (var dx = -_maxJumpDistance; dx <= _maxJumpDistance; dx++)
            {
                for (var dy = -_maxJumpHeight; dy <= _maxJumpHeight; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    var newX = x + dx;
                    var newY = y + dy;

                    if (newX < 0 || newX >= tileMap.Width || newY < 0 || newY >= tileMap.Height)
                        continue;

                    if (visited[newX, newY])
                        continue;

                    var tile = tileMap.GetTile(newX, newY);
                    if (tile == TileType.Platform || tile == TileType.Ground)
                    {
                        visited[newX, newY] = true;
                        queue.Enqueue((newX, newY));
                        lastReachable = (newX, newY);
                    }
                }
            }
        }

        if (lastReachable.HasValue)
        {
            var (startX, startY) = lastReachable.Value;
            var targetX = _endX;
            var targetY = _endY;
            
            while (startX < targetX)
            {
                var dx = Math.Min(_maxJumpDistance, targetX - startX);
                var newX = startX + dx;
                var newY = startY;
                
                if (newY >= 0 && newY < tileMap.Height)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        if (newX + i < tileMap.Width)
                        {
                            tileMap.SetTile(new Tile(newX + i, newY, TileType.Platform));
                        }
                    }
                    startX = newX;
                }
                else
                {
                    for (var dy = -_maxJumpHeight; dy <= _maxJumpHeight; dy++)
                    {
                        newY = startY + dy;
                        if (newY >= 0 && newY < tileMap.Height && tileMap.GetTile(newX, newY) == TileType.Empty)
                        {
                            for (var i = 0; i < 3; i++)
                            {
                                if (newX + i < tileMap.Width)
                                {
                                    tileMap.SetTile(new Tile(newX + i, newY, TileType.Platform));
                                }
                            }
                            startX = newX;
                            startY = newY;
                            break;
                        }
                    }
                }
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