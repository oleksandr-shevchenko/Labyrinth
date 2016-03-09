using UnityEngine;
using System.Collections;
namespace OtherScripts
{
    /// <summary>
    /// This class implements data structure of multi list to create a graph.
    /// </summary>
    public class MultiList
    {
        public int[] headList;
        public int[] nextList;
        public int[] previousList;
        public int[] listOfWeights;
        private int _counterOfAdding = 1;

        public MultiList(int vNum, int eNum)
        {
            headList = new int[vNum];
            nextList = new int[eNum + 1];
            previousList = new int[eNum + 1];
            listOfWeights = new int[eNum + 1];
        }

        // This method adds new vertex to this multi list (graph).
        public void Add(int u, int v, int weight)
        {
            nextList[_counterOfAdding] = headList[u];
            previousList[_counterOfAdding] = v;
            listOfWeights[_counterOfAdding] = weight;
            headList[u] = _counterOfAdding++;
        }
    }
}