using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public static class PoissonDiscSampling
{
    public static List<Vector2> GeneratePoints(Vector2 cellSize, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30, bool shuffle = false)
    {
        int[,] grid = new int[CeilToInt(sampleRegionSize.x / cellSize.x), CeilToInt(sampleRegionSize.y / cellSize.y)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();
        Vector2 candidate;

        spawnPoints.Add(sampleRegionSize / 2);
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                candidate = spawnCentre + new Vector2(Random.Range(cellSize.x, 2 * cellSize.x) * (Random.Range(0, 2) * 2 - 1), Random.Range(cellSize.y, 2 * cellSize.y) * (Random.Range(0, 2) * 2 - 1));
                if (IsValid(candidate, sampleRegionSize, cellSize, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize.x), (int)(candidate.y / cellSize.y)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
                spawnPoints.RemoveAt(spawnIndex);
        }
        return shuffle ? points.Shuffle() as List<Vector2> : points;
    }

    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, Vector2 cellSize, List<Vector2> points, int[,] grid)
    {
        if (candidate.x < 0 || candidate.x >= sampleRegionSize.x || candidate.y < 0 || candidate.y >= sampleRegionSize.y)
            return false;

        int cellX = (int)(candidate.x / cellSize.x);
        int cellY = (int)(candidate.y / cellSize.y);
        int searchStartX = Max(0, cellX - 2);
        int searchEndX = Min(cellX + 2, grid.GetLength(0) - 1);
        int searchStartY = Max(0, cellY - 2);
        int searchEndY = Min(cellY + 2, grid.GetLength(1) - 1);

        for (int x = searchStartX; x <= searchEndX; x++)
            for (int y = searchStartY; y <= searchEndY; y++)
            {
                int pointIndex = grid[x, y] - 1;
                if (pointIndex != -1 && Abs((candidate - points[pointIndex]).x) < cellSize.x && Abs((candidate - points[pointIndex]).y) < cellSize.y)
                    return false;
            }
        return true;
    }
}