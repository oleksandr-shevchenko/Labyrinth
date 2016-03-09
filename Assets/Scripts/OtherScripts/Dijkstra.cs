using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OtherScripts
{
    /// <summary>
    /// This struct describes the structure of the vertex
    /// </summary>
    public struct Point
    {
        public int x;
        public int y;
        public int vertex;
        public Point(int x, int y, int vertex)
        {
            this.x = x;
            this.y = y;
            this.vertex = vertex;
        }
    }
    /// <summary>
    /// This class implements Dijkstra's algorithm to calculate a shortest patch. 
    /// </summary>
    public class Dijkstra : MonoBehaviour
    {
        private List<Point> _listOfVertex;    // The list of vertex contains the shortest path.

        public Dijkstra()
        {
            _listOfVertex = new List<Point>();
        }


        private int[] DijkstraRMQ(int startVertex, int endVertex, MultiList graph)
        {
            int numberOfVertices = graph.headList.Length;
            bool[] used = new bool[numberOfVertices];   // Markers.
            int[] previous = new int[numberOfVertices]; // Previous vertices.   
            int[] distance = new int[numberOfVertices]; // Distances.
            RMQ rmq = new RMQ(numberOfVertices); // RMQ

            // Initialisation.

            for (int i = 0; i < previous.Length; i++)
                previous[i] = -1;

            for (int i = 0; i < distance.Length; i++)
                distance[i] = int.MaxValue / 2;

            rmq.Set(startVertex, distance[startVertex] = 0);

            while (true)
            {
                int tempVertex = rmq.MinIndex(); // Choose a near vertex.

                // If can't chose a near vertex.
                if (tempVertex == -1 || tempVertex == endVertex)
                    break;

                used[tempVertex] = true; // Mark the current vertex.
                rmq.Set(tempVertex, int.MaxValue / 2); // Set her value to RMQ.

                // Loop through adjacent vertices.
                for (int i = graph.headList[tempVertex]; i != 0; i = graph.nextList[i])
                {
                    int newVertVertex = graph.previousList[i];
                    int weight = graph.listOfWeights[i];

                    // If improve the estimate of the distance.
                    if (!used[newVertVertex] && distance[newVertVertex] > distance[tempVertex] + weight)
                    {
                        // Do it.
                        rmq.Set(newVertVertex, distance[newVertVertex] = distance[tempVertex] + weight);

                        // Mark a previous vertex. 
                        previous[newVertVertex] = tempVertex;
                    }
                }
            }

            // Recover a shortest patch.

            Stack<int> stack = new Stack<int>();

            for (int v = endVertex; v != -1; v = previous[v])
            {
                stack.Push(v);
            }

            int[] shortestPatch = new int[stack.Count];

            // Get the verteces from stack.
            for (int i = 0; i < shortestPatch.Length; i++)
                shortestPatch[i] = stack.Pop();

            return shortestPatch;

        }

        // This method transport a matrix to multi list (graph).
        private MultiList TransportToGraph(int[,] matrix, int n, int m)
        {
            int countOfEdge = 0;
            // Loop through a matrix to calculate a number of adge.
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    if (matrix[i, j] == 0)
                        continue;
                    if (i - 1 >= 0 && matrix[i - 1, j] != 0)
                        countOfEdge++;
                    if (i + 1 < n && matrix[i + 1, j] != 0)
                        countOfEdge++;
                    if (j - 1 >= 0 && matrix[i, j - 1] != 0)
                        countOfEdge++;
                    if (j + 1 < m && matrix[i, j + 1] != 0)
                        countOfEdge++;
                }

            MultiList multiList = new MultiList(n * m, countOfEdge);

            // Loop through a matrix to add the verteces to a multi list (graph).
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    if (matrix[i, j] == 0)
                        continue;

                    if (i - 1 >= 0 && matrix[i - 1, j] != 0)
                    {
                        multiList.Add(GetVertex(i, j, m), GetVertex(i - 1, j, m), 1);
                    }
                    if (i + 1 < n && matrix[i + 1, j] != 0)
                    {
                        multiList.Add(GetVertex(i, j, m), GetVertex(i + 1, j, m), 1);
                    }
                    if (j - 1 >= 0 && matrix[i, j - 1] != 0)
                    {
                        multiList.Add(GetVertex(i, j, m), GetVertex(i, j - 1, m), 1);
                    }
                    if (j + 1 < m && matrix[i, j + 1] != 0)
                    {
                        multiList.Add(GetVertex(i, j, m), GetVertex(i, j + 1, m), 1);
                    }

                    _listOfVertex.Add(new Point(i, j, GetVertex(i, j, m)));
                }
            return multiList;
        }

        // This method calculates a vertex by using position in the matrix.
        private int GetVertex(int x, int y, int counter)
        {

            return x * counter + y;
        }

        // This method calculates a position  by using a vertex in the matrix.
        private Point GetPosition(int vertex)
        {
            foreach (var point in _listOfVertex)
                if (point.vertex == vertex)
                    return point;
            return new Point();
        }

        // This method return the first position from a shortest patch
        public Point GetNextStep(Point s, Point end, int[,] matrix)
        {
            MultiList ml = TransportToGraph(matrix, matrix.GetLength(0), matrix.GetLength(1));

            int[] shortestPatch = DijkstraRMQ(GetVertex(s.x, s.y, matrix.GetLength(1)),
                GetVertex(end.x, end.y, matrix.GetLength(1)), ml);

            Point resultPoint = new Point(0, 0, 0);

            // If a shortest patch is  
            if (shortestPatch.Length >= 2)
            {
                resultPoint = GetPosition(shortestPatch[shortestPatch.Length - 2]);
            }
            return resultPoint;
        }
    }
}