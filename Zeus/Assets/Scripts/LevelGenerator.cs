using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Размер карты
    public int width = 20;
    public int height = 15;

    // Префабы для тайлов
    public GameObject groundTile;
    public GameObject waterTile;
    public GameObject wallTile;
    public GameObject forestTile;
    public GameObject grassTile;
    public GameObject sandTile;

    // Префабы для событий
    public GameObject enemyPrefab;
    public GameObject trapPrefab;
    public GameObject bonusPrefab;

    // Вероятности для каждого тайла
    public float waterChance = 0.2f;   // 20% вода
    public float forestChance = 0.15f; // 15% лес
    public float sandChance = 0.1f;    // 10% песок
    public float grassChance = 0.1f;   // 10% трава
    public float wallChance = 0.05f;   // 5% случайные стены

    // Префабы для Start и End Point
    public GameObject startPointPrefab;
    public GameObject endPointPrefab;

    // Координаты для начала и конца пути
    public Vector2Int startPoint;
    public Vector2Int endPoint;

    // Карта для генерации путей
    private Node[,] grid;

    void Start()
    {
        GenerateLevel();
        PlaceStartAndEndPoints(); // Добавляем вызов функции для размещения Start и End Points
        CreateMultiplePaths(startPoint, endPoint, 3); // Создаем три пути
    }

    // Функция для размещения Start и End Points
    void PlaceStartAndEndPoints()
    {
        // Создаем стартовую точку
        Instantiate(startPointPrefab, new Vector2(startPoint.x, startPoint.y), Quaternion.identity);

        // Создаем конечную точку
        Instantiate(endPointPrefab, new Vector2(endPoint.x, endPoint.y), Quaternion.identity);
    }

    void GenerateLevel()
    {
        grid = new Node[width, height]; // Создаем сетку узлов для A* алгоритма

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Создаем стены по краям
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    Instantiate(wallTile, new Vector2(x, y), Quaternion.identity);
                    grid[x, y] = new Node(x, y, true); // Узел непроходим (стена)
                }
                else
                {
                    // Генерируем случайный тайл для внутренней части карты
                    float rand = Random.Range(0f, 1f);
                    GameObject tile;
                    bool isWall = false;

                    if (rand < waterChance)
                    {
                        tile = waterTile;
                        isWall = true; // Вода непроходима, как и стены
                    }
                    else if (rand < waterChance + forestChance)
                    {
                        tile = forestTile;
                    }
                    else if (rand < waterChance + sandChance + forestChance)
                    {
                        tile = sandTile;
                    }
                    else if (rand < waterChance + sandChance + forestChance + grassChance)
                    {
                        tile = grassTile;
                    }
                    else if (rand < waterChance + sandChance + forestChance + grassChance + wallChance)
                    {
                        tile = wallTile;
                        isWall = true; // Стены непроходимы
                    }
                    else
                    {
                        tile = groundTile;
                    }

                    Instantiate(tile, new Vector2(x, y), Quaternion.identity);
                    grid[x, y] = new Node(x, y, isWall); // Вода и стены непроходимы
                }
            }
        }
    }

    // Алгоритм A* для поиска пути
    List<Node> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        Node startNode = grid[startPos.x, startPos.y];
        Node endNode = grid[endPos.x, endPos.y];

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                return RetracePath(startNode, endNode); // Найден путь
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (neighbor.isWall || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null; // Путь не найден
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    void CreateMultiplePaths(Vector2Int start, Vector2Int end, int pathCount)
    {
        for (int i = 0; i < pathCount; i++)
        {
            // Изменяем конечные точки, чтобы создавать разные пути
            Vector2Int newEnd = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1));
            List<Node> path = FindPath(start, newEnd);
            if (path != null)
            {
                PlacePath(path);
                PlaceEventsOnPath(path); // Размещаем события на пути
            }
        }
    }

    void PlacePath(List<Node> path)
    {
        foreach (Node node in path)
        {
            Instantiate(groundTile, new Vector2(node.x, node.y), Quaternion.identity);
        }
    }

    void PlaceEventsOnPath(List<Node> path)
    {
        foreach (Node node in path)
        {
            float rand = Random.Range(0f, 1f);

            if (rand < 0.1f) // 10% шанс на врага
            {
                Instantiate(enemyPrefab, new Vector2(node.x, node.y), Quaternion.identity);
            }
            else if (rand < 0.2f) // 10% шанс на ловушку
            {
                Instantiate(trapPrefab, new Vector2(node.x, node.y), Quaternion.identity);
            }
            else if (rand < 0.3f) // 10% шанс на бонус
            {
                Instantiate(bonusPrefab, new Vector2(node.x, node.y), Quaternion.identity);
            }
        }
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.x + x;
                int checkY = node.y + y;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstY = Mathf.Abs(nodeA.y - nodeB.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}

public class Node
{
    public int x;
    public int y;
    public bool isWall;
    public int gCost; // расстояние от стартовой точки
    public int hCost; // предполагаемое расстояние до конечной точки
    public Node parent;

    public Node(int x, int y, bool isWall)
    {
        this.x = x;
        this.y = y;
        this.isWall = isWall;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }
}