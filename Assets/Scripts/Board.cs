using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private PathNode _cellPrefab;

    public PathNode[,] GridArray { get; private set; }

    public int GetWidth => GridArray.GetLength(0);
    public int GetHeight => GridArray.GetLength(1);

    public Transform CellPrefabTransform  => _cellPrefab.transform;

    private void Awake()
    {
        Instance = this;
        GridArray = new PathNode[_width, _height];
        CreateCells();
        DrawLine.Grid(_width, _height);
    }

    private void CreateCells()
    {
        for (int x = 0; x < GetWidth; x++)
        {
            for (int y = 0; y < GetHeight; y++)
            {
                var cell = Instantiate(_cellPrefab, new Vector3(x + .5f, 0, y + .5f), Quaternion.identity, transform);
                cell.transform.localScale = cell.transform.localScale - new Vector3(.1f, 0, .1f);
                cell.InitCell(x, y);
                SetValue(x, y, cell);
                GridArray[x, y].IsWalkable = true;
            }
        }
    }

    public void SetValue(int x, int y, PathNode value)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            GridArray[x, y] = value;
        }
    }
    public void SetValue(Vector3 worldPosition, PathNode value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    public PathNode GetValue(int x, int y)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            return GridArray[x, y];
        }
        else
        {
            return default(PathNode);
        }
    }
    public PathNode GetValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / _cellPrefab.transform.GetChild(0).localScale.x);
        y = Mathf.FloorToInt(worldPosition.z / _cellPrefab.transform.GetChild(0).localScale.z);
    }
}
