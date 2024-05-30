using System;
using System.Collections.Generic;

public class TravelingSalesmanProblem
{
    public class Node : IComparable<Node>
    {
        public int City;
        public List<int> Path;
        public int Cost;
        public int Heuristic;

        public Node(int city, List<int> path, int cost, int heuristic = 0)
        {
            City = city;
            Path = new List<int>(path);
            Path.Add(city);
            Cost = cost;
            Heuristic = heuristic;
        }

        public int CompareTo(Node other)
        {
            return (Cost + Heuristic).CompareTo(other.Cost + other.Heuristic);
        }
    }

    // Breadth-First Search (BFS)
    public static List<int> SolveTSP_BFS(int[][] distanceMatrix, int startCity)
    {
        int n = distanceMatrix.Length;
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(new Node(startCity, new List<int>(), 0));

        while (frontier.Count > 0)
        {
            Node node = frontier.Dequeue();
            if (node.Path.Count == n)
            {
                node.Path.Add(startCity); 
                return node.Path;
            }
            for (int i = 0; i < n; i++)
            {
                if (!node.Path.Contains(i))
                {
                    frontier.Enqueue(new Node(i, node.Path, node.Cost + distanceMatrix[node.City][i]));
                }
            }
        }
        return null; 
    }

    // Uniform Cost Search (UCS)
    public static List<int> SolveTSP_UCS(int[][] distanceMatrix, int startCity)
    {
        int n = distanceMatrix.Length;
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(new Node(startCity, new List<int>(), 0));

        while (frontier.Count > 0)
        {
            Node node = frontier.Dequeue();
            if (node.Path.Count == n)
            {
                node.Path.Add(startCity);  
                return node.Path;
            }
            for (int i = 0; i < n; i++)
            {
                if (!node.Path.Contains(i))
                {
                    frontier.Enqueue(new Node(i, node.Path, node.Cost + distanceMatrix[node.City][i]));
                }
            }
        }
        return null; 
    }

    // A* Search
    public static List<int> SolveTSP_AStar(int[][] distanceMatrix, int startCity)
    {
        int n = distanceMatrix.Length;
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(new Node(startCity, new List<int>(), 0, 0));

        while (frontier.Count > 0)
        {
            Node node = frontier.Dequeue();
            if (node.Path.Count == n)
            {
                node.Path.Add(startCity);  
                return node.Path;
            }
            for (int i = 0; i < n; i++)
            {
                if (!node.Path.Contains(i))
                {
                    int newCost = node.Cost + distanceMatrix[node.City][i];
                    int heuristic = Heuristic(distanceMatrix, i, startCity, node.Path);
                    frontier.Enqueue(new Node(i, node.Path, newCost, heuristic));
                }
            }
        }
        return null; 
    }

    public static int Heuristic(int[][] distanceMatrix, int currentCity, int startCity, List<int> visitedCities)
    {
        int minDist = int.MaxValue;
        for (int i = 0; i < distanceMatrix.Length; i++)
        {
            if (!visitedCities.Contains(i) && distanceMatrix[currentCity][i] < minDist)
            {
                minDist = distanceMatrix[currentCity][i];
            }
        }
        return minDist;
    }

    public static void Main(string[] args)
    {
        int[][] distanceMatrix = {
            new int[] { 0, 29, 20, 21 },
            new int[] { 29, 0, 15, 17 },
            new int[] { 20, 15, 0, 28 },
            new int[] { 21, 17, 28, 0 }
        };
        int startCity = 0;

        List<int> pathBFS = SolveTSP_BFS(distanceMatrix, startCity);
        Console.WriteLine("BFS Path: " + string.Join(" -> ", pathBFS) + " with cost " + CalculatePathCost(distanceMatrix, pathBFS));

        List<int> pathUCS = SolveTSP_UCS(distanceMatrix, startCity);
        Console.WriteLine("UCS Path: " + string.Join(" -> ", pathUCS) + " with cost " + CalculatePathCost(distanceMatrix, pathUCS));

        List<int> pathAStar = SolveTSP_AStar(distanceMatrix, startCity);
        Console.WriteLine("A* Path: " + string.Join(" -> ", pathAStar) + " with cost " + CalculatePathCost(distanceMatrix, pathAStar));
    }

    public static int CalculatePathCost(int[][] distanceMatrix, List<int> path)
    {
        int cost = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            cost += distanceMatrix[path[i]][path[i + 1]];
        }
        return cost;
    }
}

// Simple Priority Queue Implementation
public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> data;

    public PriorityQueue()
    {
        this.data = new List<T>();
    }

    public void Enqueue(T item)
    {
        data.Add(item);
        int ci = data.Count - 1;
        while (ci > 0)
        {
            int pi = (ci - 1) / 2;
            if (data[ci].CompareTo(data[pi]) >= 0) break;
            T tmp = data[ci];
            data[ci] = data[pi];
            data[pi] = tmp;
            ci = pi;
        }
    }

    public T Dequeue()
    {
        int li = data.Count - 1;
        T frontItem = data[0];
        data[0] = data[li];
        data.RemoveAt(li);

        --li;
        int pi = 0;
        while (true)
        {
            int ci = pi * 2 + 1;
            if (ci > li) break;
            int rc = ci + 1;
            if (rc <= li && data[rc].CompareTo(data[ci]) < 0)
                ci = rc;
            if (data[pi].CompareTo(data[ci]) <= 0) break;
            T tmp = data[pi];
            data[pi] = data[ci];
            data[ci] = tmp;
            pi = ci;
        }
        return frontItem;
    }

    public int Count
    {
        get { return data.Count; }
    }
}
