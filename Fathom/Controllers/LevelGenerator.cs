using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Fathom;

public class LevelGenerator(int? seed = null, int width = 0, int height = 0)
{
    private const int MaxJumpHeight = 3;
    private const int MaxJumpDistance = 7;
    private const int MinPlatformLength = 3;
    private const int MaxPlatformLength = 6;
    private const int MinPlatformsPerSection = 1;
    private const int MaxPlatformsPerSection = 4;
    private const int SectionSize = 10;
    private const int GroundHeight = 3;

    private readonly Random _random = seed.HasValue ? new Random(seed.Value) : new Random();
    private readonly int _startX = 4;
    private readonly int _startY = height - 4;
    private readonly int _endX = width - 4;
    private readonly int _endY = 0;

    private static readonly (int X, int Y)[][] AnchorPoints =
    [
        [
            (0, 0), 
            (7, 0), 
            (4, 1), 
            (7, 2), 
            (1, 3), 
            (5, 5)
        ],
        [
            (1, 0), 
            (6, 0), 
            (3, 1), 
            (0, 2), 
            (4, 3), 
            (0, 5)
        ]
    ];

    public void GenerateLevel(TileMap tileMap)
    {
        GenerateGround(tileMap);
        GeneratePlatformSections(tileMap);
        
        if (!HasPathToEnd(tileMap))
        {
            AddMissingPlatforms(tileMap);
        }
        
        CleanIsolatedPlatforms(tileMap);
    }

    private void GenerateGround(TileMap tileMap)
    {
        for (var x = 0; x < tileMap.Width; x++)
        {
            for (var y = tileMap.Height - GroundHeight; y < tileMap.Height; y++)
            {
                tileMap.SetTile(new Tile(x, y, TileType.Ground));
            }
        } 
    }

    private void GeneratePlatformSections(TileMap tileMap)
    {
        var sections = CreateLevelSections(tileMap);
        foreach (var section in sections)
        {
            foreach (var tile in section)
            {
                tileMap.SetTile(tile);
            }
        }
    }

    private List<Tile[,]> CreateLevelSections(TileMap tileMap)
    {
        var tilesSections = new List<Tile[,]>();
        var (sectionWidth, sectionHeight) = (tileMap.Width / 40, tileMap.Height / 37);
        
        for (var sectionY = 0; sectionY < tileMap.Height; sectionY += SectionSize)
        {
            for (var sectionX = 0; sectionX < tileMap.Width; sectionX += SectionSize)
            {
                var sectionTiles = GenerateSection(sectionWidth, sectionHeight, sectionX, sectionY);
                tilesSections.Add(sectionTiles);
            }
        }
        
        return tilesSections;
    }

    private Tile[,] GenerateSection(int width, int height, int offsetX, int offsetY)
    {
        var tiles = TileMap.InitializeMap(width, height);
        var templateIndex = _random.Next(0, AnchorPoints.Length);
        var platformCount = _random.Next(MinPlatformsPerSection, MaxPlatformsPerSection);

        for (var i = 0; i < platformCount; i++)
        {
            AddPlatformToSection(tiles, templateIndex, offsetX, offsetY, width);
        }

        return tiles;
    }

    private void AddPlatformToSection(Tile[,] tiles, int templateIndex, int offsetX, int offsetY, int width)
    {
        var anchorPointIndex = _random.Next(0, AnchorPoints[templateIndex].Length);
        var anchorPoint = AnchorPoints[templateIndex][anchorPointIndex];
        var platformLength = _random.Next(MinPlatformLength, MaxPlatformLength);

        for (var i = 0; i < platformLength && anchorPoint.X + i < width; i++)
        {
            tiles[anchorPoint.X + i, anchorPoint.Y] = new Tile(
                offsetX + anchorPoint.X + i,
                offsetY + anchorPoint.Y,
                TileType.Platform
            );
        }
    }
    
    private bool HasPathToEnd(TileMap tileMap)
    {
        var visited = new bool[tileMap.Width, tileMap.Height + 1];
        var queue = new Queue<(int X, int Y)>();
        queue.Enqueue((_startX, _startY));
        visited[_startX, _startY] = true;

        while (queue.Count > 0)
        {
            var (currentX, currentY) = queue.Dequeue();
            
            if (IsReachablePosition(currentX, currentY, _endX, _endY))
            {
                return true;
            }

            var reachablePositions = GetReachablePositions(tileMap, currentX, currentY, visited);
            foreach (var (newX, newY) in reachablePositions)
            {
                visited[newX, newY] = true;
                queue.Enqueue((newX, newY));
            }
        }
        return false;
    }

    private List<(int X, int Y)> GetReachablePositions(TileMap tileMap, int currentX, int currentY, bool[,] visited)
    {
        var positions = new List<(int X, int Y)>();

        for (var dx = -MaxJumpDistance; dx <= MaxJumpDistance; dx++)
        {
            for (var dy = -MaxJumpHeight; dy <= MaxJumpHeight; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                var newX = currentX + dx;
                var newY = currentY + dy;

                if (!IsValidPosition(tileMap, newX, newY) || visited[newX, newY])
                    continue;

                var tile = tileMap.GetTile(newX, newY);
                if (tile == TileType.Platform || tile == TileType.Ground)
                {
                    positions.Add((newX, newY));
                }
            }
        }

        return positions;
    }

    private static bool IsValidPosition(TileMap tileMap, int x, int y)
    {
        return x >= 0 && x < tileMap.Width && y >= 0 && y < tileMap.Height;
    }

    private static bool IsReachablePosition(int currentX, int currentY, int targetX, int targetY)
    {
        return currentX == targetX && currentY == targetY;
    }
    
    private void AddMissingPlatforms(TileMap tileMap)
    {
        var (lastReachableX, lastReachableY) = FindLastReachablePosition(tileMap);
        if (lastReachableX.HasValue && lastReachableY.HasValue)
        {
            BuildPathToEnd(tileMap, lastReachableX.Value, lastReachableY.Value);
        }
    }

    private (int? X, int? Y) FindLastReachablePosition(TileMap tileMap)
    {
        var visited = new bool[tileMap.Width, tileMap.Height];
        var queue = new Queue<(int X, int Y)>();
        queue.Enqueue((_startX, _startY));
        visited[_startX, _startY] = true;
        int? lastX = null, lastY = null;

        while (queue.Count > 0)
        {
            var (currentX, currentY) = queue.Dequeue();
            var reachablePositions = GetReachablePositions(tileMap, currentX, currentY, visited);

            foreach (var (newX, newY) in reachablePositions)
            {
                visited[newX, newY] = true;
                queue.Enqueue((newX, newY));
                lastX = newX;
                lastY = newY;
            }
        }

        return (lastX, lastY);
    }

    private void BuildPathToEnd(TileMap tileMap, int startX, int startY)
    {
        var currentX = startX;
        var currentY = startY;

        while (currentX < _endX)
        {
            var (nextX, nextY) = CalculateNextPlatformPosition(tileMap, currentX, currentY);
            CreatePlatform(tileMap, nextX, nextY);
            currentX = nextX;
            currentY = nextY;
        }
    }

    private (int X, int Y) CalculateNextPlatformPosition(TileMap tileMap, int currentX, int currentY)
    {
        var dx = Math.Min(MaxJumpDistance, _endX - currentX);
        var newX = currentX + dx;
        var newY = currentY;

        if (!IsValidPosition(tileMap, newX, newY))
        {
            for (var dy = -MaxJumpHeight; dy <= MaxJumpHeight; dy++)
            {
                newY = currentY + dy;
                if (IsValidPosition(tileMap, newX, newY) && tileMap.GetTile(newX, newY) == TileType.Empty)
                {
                    break;
                }
            }
        }

        return (newX, newY);
    }

    private static void CreatePlatform(TileMap tileMap, int x, int y)
    {
        const int platformLength = 3;
        for (var i = 0; i < platformLength; i++)
        {
            if (x + i < tileMap.Width)
            {
                tileMap.SetTile(new Tile(x + i, y, TileType.Platform));
            }
        }
    }
    
    private static bool IsIsolated(TileMap tileMap, int x, int y)
    {
        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                var checkX = x + dx;
                var checkY = y + dy;

                if (IsValidPosition(tileMap, checkX, checkY))
                {
                    var tile = tileMap.GetTile(checkX, checkY);
                    if (tile == TileType.Platform || tile == TileType.Ground)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    
    private static void CleanIsolatedPlatforms(TileMap tileMap)
    {
        for (var x = 0; x < tileMap.Width; x++)
        {
            for (var y = 0; y < tileMap.Height; y++)
            {
                if (tileMap.GetTile(x, y) == TileType.Platform && IsIsolated(tileMap, x, y))
                {
                    tileMap.SetTile(new Tile(x, y, TileType.Empty));
                }
            }
        }
    }
} 