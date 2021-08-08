using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private NPC _NPCPrefab;
    [SerializeField] private Transform _cubesParent;

    [Header("NPC Parameters")]
    [SerializeField, FloatRangeSlider(1f, 20f)] private FloatRange _speed = new FloatRange(10f);
    [SerializeField, FloatRangeSlider(1f, 25f)] private FloatRange _radius = new FloatRange(5f);
    [SerializeField, FloatRangeSlider(1f, 10f)] private FloatRange _waitTime = new FloatRange(3f);
    [SerializeField, FloatRangeSlider(0f, 5f)] private FloatRange _luck = new FloatRange(3f);

    private List<NPC> _NPCList = new List<NPC>();


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            AlternativeHandleTouch();
        }
    }

    private void HandleTouch()
    {
        Vector3 worldPosition = WorldPositionConverter.GetRandomPointOnBoard();
        var current = Instantiate(_NPCPrefab, worldPosition, Quaternion.identity, _cubesParent);
        current.Init(_speed.RandomValueInRange, _radius.RandomValueInRange, _waitTime.RandomValueInRange, _luck.RandomValueInRange);
        _NPCList.Add(current);
        current.name = _NPCList.Count.ToString();
    }

    private void AlternativeHandleTouch()
    {
        var board = Board.Instance;
        Vector3 worldPosition = WorldPositionConverter.GetMouseWorldPosition();
        board.GetXY(worldPosition, out int x, out int y);

        if (board.GetValue(x, y).IsWalkable)
        {
            var cellBlock = Instantiate(_blockPrefab, new Vector3(x + 0.5f, 1, y + 0.5f), Quaternion.identity);
            board.GetValue(x, y).SetIsWalkable(!board.GetValue(x, y).IsWalkable, cellBlock);
        }
        else
        {
            Destroy(board.GetValue(x, y).CellBlock);
            board.GetValue(x, y).SetIsWalkable(!board.GetValue(x, y).IsWalkable);
        }
    }
}

