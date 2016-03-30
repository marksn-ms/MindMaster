using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MoveTreeLib
{
    /// <summary>
    /// A move consists of an answer, the possible connectivity options (distances) to next guess options,
    /// and the resulting state (which answers are/aren't feasible). 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    [DebuggerDisplay("Move={Move},Parent={Parent?.Move},Children={Children.Count}")]
    internal class Node<U> : IGrouping<int, Node<U>>
    {
        public Node<U> Parent { get; set; }
        public List<Node<U>> Children { get; } = new List<Node<U>>();
        public U Move { get; set; }  // the chosen answer this node represents

        public Node(U move, Node<U> parent = null)
        {
            Move = move;
            Parent = parent;
        }

        public static int CalculateDistance(U o1, U o2)
        {
            int result = 0;
            for (int i = 0; i < Math.Min(o1.ToString().Length, o2.ToString().Length); i++)
            {
                if (o1.ToString().Substring(i, 1) == o2.ToString().Substring(i, 1))
                    result++;
            }
            return result;
        }

        public int Depth
        {
            get
            {
                var p = Parent;
                int i = 0;
                while (p != null)
                {
                    p = Parent.Parent;
                    i++;
                }
                return i;
            }
        }

        public int Key
        {
            get
            {
                return Parent == null ? 0 : CalculateDistance(Move, Parent.Move);
            }
        }

        // T is the type of the value upon which the values will be partitioned
        // U is the type of the value represented by the node
        public static List<Node<U>> Search(IEnumerable<Node<U>> start)
        {
            // move through the answers it can be and 
            Queue<Node<U>> queue = new Queue<Node<U>>();
            List<Node<U>> result = new List<Node<U>>();
            foreach (var m in start)
            {
                queue.Enqueue(m);
            }

            while (queue.Count() > 0)
            {
                var current = queue.Dequeue();

                // as we process each item, add what should be its children
                if (current.Parent == null) // if no parent, just process children
                {
                    // add each of the (other) start nodes as children of this node
                    current.Children.AddRange(start.Where(x => x.Move.ToString() != current.Move.ToString()).Select(x => new Node<U>(x.Move, current)));
                }
                else // if I have a parent, then I must belong to of one of my parent's partitions
                {
                    // loop through my partition-mates - this is how I prune and don't visit nonfeasible solutions
                    current.Children.AddRange(
                        current
                            .Parent
                            .Children
                            .Where(x => x.Move.ToString() != current.Move.ToString() && CalculateDistance(x.Move, current.Parent.Move) == CalculateDistance(current.Move, current.Parent.Move))
                            .Select(x => new Node<U>(x.Move, current))
                            );
                }
                result.Add(current);

                if (current.Children.Count() == 0)
                {
                    string moves = current.Move.ToString();
                    var p = current.Parent;
                    while (p != null)
                    {
                        moves = p.Move.ToString() + "->" + moves;
                        p = p.Parent;
                    }
                    Debug.WriteLine($"Eureka! {moves}");
                }

                // after we process each item, enqueue its children
                foreach (var c in current.Children)
                {
                    queue.Enqueue(c);
                }
            }

            return result;
        }

        public IEnumerator<Node<U>> GetEnumerator()
        {
            if (Parent == null)
                return Enumerable.Empty<Node<U>>().GetEnumerator();
            int d = CalculateDistance(Move, Parent.Move);
            return Parent.Children.Where(x => CalculateDistance(Move, x.Move) == d).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Parent == null)
                return Enumerable.Empty<Node<U>>().GetEnumerator();
            int d = CalculateDistance(Move, Parent.Move);
            return Parent.Children.Where(x => CalculateDistance(Move, x.Move) == d).GetEnumerator();
        }

        IEnumerable<Node<U>> GetTree()
        {
            yield return this;
            foreach (var c in Children)
                foreach (var cc in c.GetTree())
                    yield return cc;
        }

        // I want to recurse through the tree and return the grouping of the largest size, where
        // the grouping is determined using the distance method.
        public IGrouping<int, Node<U>> GetMaxPartition()
        {
            // get my childrens' (or my) biggest partition
            List<IGrouping<int, Node<U>>> groupings = new List<System.Linq.IGrouping<int, Node<U>>>();
            if (Parent != null)
            {
                groupings.AddRange(Parent.Children.GroupBy(x => CalculateDistance(Move, x.Move)));
            }
            if (Children.Count() > 0)
            {
                groupings.AddRange(Children.GroupBy(x => CalculateDistance(Move, x.Move)));
            }
            if (groupings.Count() == 0)
                return null;
            IGrouping<int, Node<U>> maxGrouping = groupings.OrderBy(x => x.Count()).First();
            int maxGroupCount = maxGrouping.Count();
            return maxGrouping;
        }
    }
}
