using System;
using System.Collections.Generic;
using System.Text;

namespace AstarConsole
{
    public class Astar
    {
        // An array [x][y] of TileTypes representing the map
        private TileType[][] map;

        // List of nodes that form the closed list (nodes that have been visited)
        private List<Node> closedList;

        // List of nodes that form the open list (nodes that have been found, but not visited)
        private List<Node> openList;

        // The map's dimensions
        private int mapWidth;
        private int mapHeight;

        public Astar(TileType[][] map)
        {
            this.map = map;
            mapWidth = map.Length;
            mapHeight = map[0].Length;
        }

        // This is the entry point for path finding for the A* algorithm
        // Need to trace a path from startX/Y to targetX/Y, and return a list of nodes that will
        // define a path from the start to the end in order.
        public List<Node> FindPath(int startX, int startY, int targetX, int targetY)
        {
            List<Node> path = new List<Node>();

            // Reset the open and closed lists (to allow for re-running many times
            openList = new List<Node>();
            closedList = new List<Node>();

            // The only node we can find at the start is the one we're standing on, so add this
            // to the list of open nodes
            Node startNode = new Node(startX, startY, null);
            openList.Add(startNode);

            /* 
             * We have initialised the lists, can now start the process of path finding.
             * This consists of:
             * 1. Search through open list to find the node with the lowest estimated distance
             * 2. Visit this node, and find all of its surrounding nodes, adding valid nodes to the open list
             *    A valid node is traversable (not a wall), and not already on the closed list
             * 3. Move the visited node from the open to the closed list
             * 4. Repeat until we either visit the target node (success) or the open list is empty (fail)
             */

            // Rather than an infinite loop, use an abort counter to prevent the path finding from
            // bogging down the CPU if the path becomes very long.
            int abortCounter = 500;
            while (abortCounter-- != 0)
            {
                Node cheapestNode = FindCheapestNode(targetX, targetY);
            
                // If a node cannot be found, return the empty path to indicate failure
                if (cheapestNode == null) return path;

                if (cheapestNode.X == targetX && cheapestNode.Y == targetY)
                {
                    // We have found the target node, generate the path and return it
                    Node node = cheapestNode;
                    while (node != null)
                    {
                        path.Add(node);
                        node = node.PreviousNode;
                    }
                    // Reverse the path so that it starts from the beginning
                    path.Reverse();
                    return path;
                }

                // Visit the cheapest node, looking for surrounding nodes that can be opened
                VisitNode(cheapestNode, targetX, targetY);
            }

            return path;
        }


        // Search through the open list, and find the node with the lowest estimated total distance to the target
        private Node FindCheapestNode(int targetX, int targetY)
        {
            // If open list is empty, this is a failure condition. Return null to indicate no path exists
            if (openList.Count == 0) return null;

            Node cheapestNode = openList[0];
            foreach (var openNode in openList)
            {
                if (openNode.GetEstimatedTotalDistance(targetX, targetY) < cheapestNode.GetEstimatedTotalDistance(targetX, targetY))
                {
                    cheapestNode = openNode;
                }
            }
            return cheapestNode;
        }


        private void VisitNode(Node node, int targetX, int targetY)
        {
            // Get the centre x and y coordinates that we're going to search around
            int cx = node.X;
            int cy = node.Y;

            // Loop through the deltax/y to check all surrounding nodes
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    // Skip the centre node
                    if (dx == 0 && dy == 0) continue;

                    // Get the node to check's coordinates
                    int nx = cx + dx;
                    int ny = cy + dy;

                    // Skip the node if it is outside the map
                    if (nx < 0 || ny < 0 || nx >= mapWidth || ny >= mapHeight) continue;

                    // Skip the node if it isn't navigable
                    if (!IsNavigable(nx, ny)) continue;

                    // Skip the node if it is on the closed list
                    if (FindOnClosedList(nx, ny) != null) continue;

                    // If we get here, we have found a valid open node
                    Node potentialNode = new Node(nx, ny, node);

                    // If the node is already on the open list, we replace it if the new path is cheaper
                    Node existingOpenNode = FindOnOpenList(nx, ny);

                    if (existingOpenNode == null)
                    {
                        // Not currently on open list so simply add a new node
                        openList.Add(potentialNode);
                    }
                    else
                    {
                        // Already on open list - need to determine whether the new path is cheaper
                        if (potentialNode.GetEstimatedTotalDistance(targetX, targetY) < existingOpenNode.GetEstimatedTotalDistance(targetX, targetY))
                        {
                            // The newly found node is cheaper than the existing, replace the existing open node with the new node
                            openList.Remove(existingOpenNode);
                            openList.Add(potentialNode);
                        }
                    }
                }
            }

            // We have scanned all of the surrounding nodes, now close the node we're currently visiting
            openList.Remove(node);
            closedList.Add(node);
        }


        // Finds a node on the open list, or returns null if no node could be found
        private Node FindOnOpenList(int x, int y)
        {
            foreach (Node node in openList)
            {
                if (node.X == x && node.Y == y) return node;
            }
            return null;
        }


        // Finds a node on the closed list, or returns null if no node could be found
        private Node FindOnClosedList(int x, int y)
        {
            foreach (Node node in closedList)
            {
                if (node.X == x && node.Y == y) return node;
            }
            return null;
        }

        private bool IsNavigable(int nx, int ny)
        {
            if (map[nx][ny] == TileType.Wall) return false;
            return true;
        }

    }
}
