using System.Collections.Generic;
using UnityEngine;

public static class DrawLine
{
    public static void Path(List<PathNode> path, float duration)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(GetWorldPosition(path[i].X, path[i].Y, Board.Instance.CellPrefabTransform) + new Vector3(.5f, 0, .5f),
                GetWorldPosition(path[i + 1].X, path[i + 1].Y, Board.Instance.CellPrefabTransform) + new Vector3(.5f, 0, .5f),
                Color.green, duration);
        }
    }

    public static void Grid(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y, Board.Instance.CellPrefabTransform),
                    GetWorldPosition(x, y + 1, Board.Instance.CellPrefabTransform), Color.green, 100f);

                Debug.DrawLine(GetWorldPosition(x, y, Board.Instance.CellPrefabTransform),
                    GetWorldPosition(x + 1, y, Board.Instance.CellPrefabTransform), Color.green, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height, Board.Instance.CellPrefabTransform),
            GetWorldPosition(width, height, Board.Instance.CellPrefabTransform), Color.green, 100f);

        Debug.DrawLine(GetWorldPosition(width, 0, Board.Instance.CellPrefabTransform),
            GetWorldPosition(width, height, Board.Instance.CellPrefabTransform), Color.green, 100f);
    }

    private static Vector3 GetWorldPosition(int x, int y, Transform cellPrefab)
    {
        return new Vector3(x, .1f, y) * cellPrefab.GetChild(0).localScale.x;
    }
}
