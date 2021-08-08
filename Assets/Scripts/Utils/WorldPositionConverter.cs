using UnityEngine;

public static class WorldPositionConverter
{
    public static Vector3 GetRandomPointOnBoard()
    {
        var board = Board.Instance;
        var worldPosition = new Vector3(Random.Range(0, board.GetWidth), 0f, Random.Range(0, board.GetHeight));
        return worldPosition;
    }

    public static Vector3 GetRandomWorldPositionInCircle(Vector3 centrOfCircle, float radius)
    {
        // �������� ��������� ���� ��� ����������
        float randomAng = Random.Range(0, Mathf.PI * 2);

        // ��������� ������ ���������� �� ���������� ����� ����������
        float randomRadius = Random.Range(-radius, radius);
        while (randomRadius > -1 && randomRadius < 1)
        {
            randomRadius = Random.Range(-radius, radius);
        }

        // ����� �� ���������� � �������� randomRadius
        var worldPosition = centrOfCircle + new Vector3(Mathf.Cos(randomAng) * randomRadius, 0, Mathf.Sin(randomAng) * randomRadius);
        worldPosition.y = 0;
        return worldPosition;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.y = 0;
        return worldPosition;
    }

}
