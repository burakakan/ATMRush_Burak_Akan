using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public static class RandomPoints
{
   public static void /*List<Vector2>*/ Generate(Rect region, int numOfPoints, float minDistX, float minDistY)
    {
        float xMin = region.xMin;
        float xMax = region.xMax;
        float yMin = region.yMin;
        float yMax = region.yMax;
        List<Vector2> set = new List<Vector2>(numOfPoints);
        int tries;
        for (int i = 0; i < numOfPoints; i++)
        {
            tries = 0;
            do
            {
                set[i] = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
                tries++;
            }
            while (tries < 100 && set.GetRange(0,i).Exists(p => Abs(set[i].x - p.x) < minDistX || Abs(set[i].y - p.y) < minDistY));
        }
    }


}
