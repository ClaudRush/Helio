using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public int X;
    public int Y;

    public int GCost { get; set; }
    public int HCost { get; set; }
    private int _fCost;
    public int FCost { get { return _fCost; } set { _fCost = GCost + HCost; } }

    public bool IsWalkable;
    public PathNode CameFromNode { get; set; }
    public GameObject CellBlock { get; set; }
    public void InitCell(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void SetIsWalkable(bool isWalkable, GameObject cellBlock)
    {
        IsWalkable = isWalkable;
        CellBlock = cellBlock;
    }
    public void SetIsWalkable(bool isWalkable)
    {
        IsWalkable = isWalkable;
    }


    public override string ToString()
    {
        return X + ", " + Y;
    }
}
