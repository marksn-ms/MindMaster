using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SandmarkTechnologies.MoveTree
{
    // Class: MoveTree<T,U>
    // Interesting things about this class:
    //   - Low coupling with consumers.  Because the only dependency needed to use this
    //     class are the Type of the distance measurement field and the Type of the move 
    //     itself, the class is easy to use for a variety of problem flavors.
    //   - This class is more tightly coupled with the Node class, but implemented in a way
    //     that allows the consumer to be unaware of the Node class, which is also generic
    //     regarding the T,U types.
    //   - Because of the solution that is used, involving a tree and breadth-first-search
    //     algorithm, the class will always suggest the move that will result in the fewest
    //     number of guesses necessary to reach a definitive solution.
    public class MoveTree<T,U>
    {
        // Internal storage for the set of move transitions.  An alternative was to house 
        // this in an adjacency matrix (see AdjacencyMatrix.cs) but implementation of the
        // search and pruning methods was easier with a list.
        private List<Node<U>> _moveTree;

        /// <summary>
        /// Constructor that initializes the tree from set of possible answers.
        /// </summary>
        /// <param name="answers">The set of possible answers.</param>
        public MoveTree(IEnumerable<U> answers)
        {
            // create the initial list of nodes
            var start = answers.Select(x => new Node<U>(x));

            // build the tree of moves from the starting nodes
            _moveTree = Node<U>.Search(start);
        }

        /// <summary>
        /// Returns true if there is only one answer possible from the current state.
        /// </summary>
        public bool HasEureka { get { return _moveTree != null && _moveTree.Count() == 1; } }

        /// <summary>
        /// Returns the best answer after evaluating the possible answers available.
        /// </summary>
        /// <returns>Best (suggested or only) answer.</returns>
        public U BestMove()
        {
            if (_moveTree.Count() == 1)
                return _moveTree.First().Move;
            // pick the first item from the set with lowest expected number of moves
            var bestNode = _moveTree.OrderBy(x => x.GetMaxPartition().Count()).First();
            Debug.Assert(bestNode != null);  // This should always return something until Eureka!
            return bestNode.Move;
        }

        /// <summary>
        /// Returns true if the specified move is feasible from the current state with the specified distance.
        /// </summary>
        /// <param name="move">The move to test.</param>
        /// <param name="distance">The distance to test.</param>
        /// <returns></returns>
        public bool IsMoveValid(U move, T distance)
        {
            return _moveTree.Where(x => Node<U>.CalculateDistance(move, x.Move).Equals(distance)).Count() != 0;
        }

        /// <summary>
        /// Replaces the current move tree with a new one based on the new set of possible 
        /// answers, given the specified move from the current state was selected.
        /// </summary>
        /// <param name="move">The specified move from which to regenerate the move tree.</param>
        /// <param name="distance">The distance from the specified move.</param>
        public void PruneFromMove(U move, T distance)
        {
            Node<U> moveNode = _moveTree.Where(x => x.Parent == null && x.Move.Equals(move)).First();
            // we can prune all but bestNode.Children of distance howClose
            var feasible = moveNode.Children
                .Where(x => Node<U>.CalculateDistance(moveNode.Move, x.Move).Equals(distance))
                .Select(x => new Node<U>(x.Move));
            _moveTree = Node<U>.Search(feasible);
        }
    }
}
