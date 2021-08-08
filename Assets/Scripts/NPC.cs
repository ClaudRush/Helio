using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private float _speed;
    private float _radius;
    private float _waitTime;
    private float _luck;

    private int _currentPathIndex;
    private List<Vector3> _pathVectorList;
    private Pathfinding _pathfinding;

    public void Init(float speed, float radius, float waitTime, float luck)
    {
        _speed = speed;
        _radius = radius;
        _waitTime = waitTime;
        _luck = luck;
        transform.GetComponent<SphereCollider>().radius = radius;
    }

    private void Start()
    {
        _pathfinding = GetComponent<Pathfinding>();
        SetRandomPoint();
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<NPC>();
        var distanceNPC = Vector3.Distance(transform.localPosition, enemy.transform.localPosition);

        if (distanceNPC > enemy._radius && !Mathf.Approximately(_radius, enemy._radius))
            return;

        var luckSum = _luck + enemy._luck;
        var luckRandom = Random.Range(0, luckSum);

        if (luckRandom >= _luck)
            Destroy(gameObject);
        else
            Destroy(enemy.gameObject);
    }

    private void Move()
    {
        if (_pathVectorList != null)
        {
            Vector3 targetPosition = _pathVectorList[_currentPathIndex] + Vector3.one / 2;
            if (Vector3.Distance(transform.localPosition, targetPosition) > 0.1f)
            {
                Vector3 moveDir = (targetPosition - transform.localPosition).normalized;
                transform.localPosition = transform.localPosition + (_speed * Time.deltaTime * moveDir);
            }
            else
            {
                _currentPathIndex++;
                IsReachedLastPointOnPath(targetPosition);
            }
        }
    }

    private void IsReachedLastPointOnPath(Vector3 targetPosition)
    {
        if (_currentPathIndex >= _pathVectorList.Count)
        {
            RemovePath();
            transform.localPosition = Vector3.LerpUnclamped(transform.localPosition, targetPosition, 1f);
            IsStopAfterPatrolling();
        }
    }

    private void RemovePath()
    {
        _pathVectorList = null;
    }

    private void IsStopAfterPatrolling()
    {
        bool isStop = Random.Range(0, 2) > 0;

        if (isStop)
            StartCoroutine(WaitBeforStartMoving(_waitTime));
        else
            StartCoroutine(WaitBeforStartMoving(0));
    }


    public void SetPath(Vector3 targetPosition)
    {
        _currentPathIndex = 0;

        // Если targetPosition находися в радиусе NPC
        //if (Mathf.Pow(targetPosition.x - transform.localPosition.x, 2f) + Mathf.Pow(targetPosition.z - transform.localPosition.z, 2f) <= _radius * _radius)
            _pathVectorList = _pathfinding.FindPath(transform.localPosition, targetPosition);

        if (_pathVectorList != null && _pathVectorList.Count > 1)
            _pathVectorList.RemoveAt(0);
    }

    private void SetRandomPoint()
    {
        Vector3 worldEndPosition = WorldPositionConverter.GetRandomWorldPositionInCircle(transform.localPosition, _radius);
        while (Board.Instance.GetValue(worldEndPosition) == null || !Board.Instance.GetValue(worldEndPosition).IsWalkable)
        {
            worldEndPosition = WorldPositionConverter.GetRandomWorldPositionInCircle(transform.localPosition, _radius);
        }

        Board.Instance.GetXY(worldEndPosition, out int x, out int y);

        List<PathNode> path = _pathfinding.FindPath((int)transform.localPosition.x, (int)transform.localPosition.z, x, y);

        if (path != null)
        {
            DrawLine.Path(path, path.Count / _speed);
            SetPath(worldEndPosition);
        }
    }

    private IEnumerator WaitBeforStartMoving(float time)
    {
        yield return new WaitForSeconds(time);
        SetRandomPoint();
    }
}
