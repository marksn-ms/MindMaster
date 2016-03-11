using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandmarkTechnologies.MoveTree
{
    internal class AdjacencyMatrix<T, U>
    {
        public List<U> Labels;
        public T[,] Matrix { get; private set; }
        public T[] Row(U key)
        {
            int i = Labels.IndexOf(key);
            var result = new T[Labels.Count()];
            for (int j = 0; j < Labels.Count(); j++)
                result[j] = Matrix[i, j];
            return result;
        }
        public T[] Column(U key)
        {
            int i = Labels.IndexOf(key);
            var result = new T[Labels.Count()];
            for (int j = 0; j < Labels.Count(); j++)
                result[j] = Matrix[j, i];
            return result;
        }
        public static AdjacencyMatrix<T, U> Create(List<U> nodes, Func<U, U, T> distance)
        {
            var graph = new AdjacencyMatrix<T, U>();
            graph.Labels = new List<U>(nodes);
            graph.Matrix = new T[nodes.Count(), nodes.Count()];

            // populate distance matrix
            for (int i = 0; i < nodes.Count(); i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    graph.Matrix[i, j] = graph.Matrix[j, i] = distance(nodes[i], nodes[j]);
                }
            }

            return graph;
        }
    }
}
