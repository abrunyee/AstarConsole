using System;
using System.Collections.Generic;
using System.Text;

namespace AstarConsole
{
    public class Node
    {
        private int x;
        private int y;
        private double distanceTravelled;
        private Node previousNode;

    
        // Constructor
        // Node requires its x and y coordinates and a reference to the previous node
        // The previous node is used for pathing and also to calculate the distance travelled
        public Node(int x, int y, Node previousNode)
        {
            this.x = x;
            this.y = y;
            // Use the property setter as this also calculates and stores the distance travelled
            PreviousNode = previousNode;
        }


        // Get the distance from this node to another (pythagoras)
        private double GetDistanceToNode(Node node)
        {
            int deltaX = x - node.X;
            int deltaY = y - node.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }


        // The X-coordinate of the node
        public int X
        {
            get { return x; }
        }


        // The Y-coordinate of the node
        public int Y
        {
            get { return y; }
        }


        // The total distance travelled along the path
        public double DistanceTravelled
        {
            get { return distanceTravelled; }
        }


        // Hold a reference to the previous node in the path. 
        // This is the node we travelled from to get to this node.
        public Node PreviousNode
        {
            get { return previousNode; }
            set
            {
                previousNode = value;
                distanceTravelled = 
                    previousNode == null ? 0 : previousNode.DistanceTravelled + GetDistanceToNode(previousNode);
            }
        }


        // Heuristic is an estimate of how far from the target the node is.
        // Simple option is to use Pythagoras.
        public double GetHeuristic(int targetX, int targetY)
        {
            int deltaX = x - targetX;
            int deltaY = y - targetY;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }


        // Return the estimated total distance for this node (estimate of distance from start node to target)
        // This is the heuristic function f() in the specification for A*
        public double GetEstimatedTotalDistance(int targetX, int targetY)
        {
            return distanceTravelled + GetHeuristic(targetX, targetY);
        }
    }

}
