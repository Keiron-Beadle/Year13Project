using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Object_Moving
{
    class Grid //This is the 'meat' of the program. Majority of A* pathfinding goes on here.
    {

        Random rnd = new Random();

        List<Node> listOfNodes;
        Point nextPoint = new Point();

        char[,] mapArray;
        bool noMovement = false;

        public Grid(List<Node> inNodeList, char[,] inArray)
        {
            listOfNodes = inNodeList;
            mapArray = inArray;
        }
        
        public List<Point> FindPath(Point start, Point end, List<Node> nodeList)
        {
            List<Point> closedSet = new List<Point>(); //Holds nodes already visited
            List<Point> openSet = new List<Point> { start }; //Holds nodes adjacent to searched nodes
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>(); //Dictionary to hold where we came from and where we are.
            Dictionary<Point, int> currentDistance = new Dictionary<Point, int>(); //Current distance from start
            Dictionary<Point, float> predictedDistance = new Dictionary<Point, float>(); //Predicted distance using basic math.

            currentDistance.Add(start, 0); 
            predictedDistance.Add(start, 0 + +Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y));

            if (start == end)
            {
                noMovement = true; //This is used so that when the zombie reaches their end point - they will stop moving. *prevents crash*
            }

            if (!noMovement)
            {   
                while (openSet.Count > 0)
                {
                    Point current = (from p in openSet orderby predictedDistance[p] ascending select p).First(); //Orders dictionary to find lowest predicted distance route
                    if (current.X == end.X && current.Y == end.Y) //If we're at the end, 
                    {
                        return ReconstructPath(cameFrom, end); //Reconstruct our path.
                    }

                    openSet.Remove(current); //Remove current node we're searching out of openset
                    closedSet.Add(current); //Add it to the list that we've already searched.
                    foreach (Point neighbor in GetNeighborNodes(current, nodeList))
                    {
                        int tempCurrentDistance = currentDistance[current]; //Foreach of the neighbouring nodes to the current one,
                        currentDistance[neighbor] = tempCurrentDistance; //set new predicted distances 
                        predictedDistance[neighbor] = currentDistance[neighbor] + Math.Abs(neighbor.X - end.X) + Math.Abs(neighbor.Y - end.Y);
                        if (closedSet.Contains(neighbor) && tempCurrentDistance >= currentDistance[neighbor])
                        {
                            continue; 
                        }

                        if (!closedSet.Contains(neighbor) || tempCurrentDistance < currentDistance[neighbor])
                        {
                            if (cameFrom.Keys.Contains(neighbor))
                            {
                                cameFrom[neighbor] = current;
                            }
                            else
                            {
                                cameFrom.Add(neighbor, current);
                            }

                            if (!openSet.Contains(neighbor))
                            {
                                openSet.Add(neighbor);
                            }
                        }
                    }
                }

                return FindPath(start, RandomNode(start), nodeList); //Little bit of recursive to find path.
            }
            noMovement = false;
            List<Point> dontMove = new List<Point> { start };
            return dontMove;
        }

        private List<Point> GetNeighborNodes(Point current, List<Node> nodeList)
        {
            int currentOriginX = current.X;
            int currentOriginY = current.Y;
            
            List<Point> neighborNodes = new List<Point>();

            #region NeighborNodesFinding
            for (int m = 0; m < 9; m++)
            {
                current.X += 1;
                if (mapArray[current.X, current.Y] == 'Q' || mapArray[current.X, current.Y] == 'W' ||
                    mapArray[current.X, current.Y] == 'E' || mapArray[current.X, current.Y] == 'R' || 
                    mapArray[current.X, current.Y] == 'T' || mapArray[current.X, current.Y] == 'Y' || 
                    mapArray[current.X, current.Y] == 'U' || mapArray[current.X, current.Y] == 'I' || 
                    mapArray[current.X, current.Y] == 'O' || mapArray[current.X, current.Y] == 'P' || 
                    mapArray[current.X, current.Y] == 'A' || mapArray[current.X, current.Y] == 'D')
                {
                    break; //If we reach a wall, cut off the search immediately. 
                }

                if (mapArray[current.X, current.Y] == ' ' || mapArray[current.X, current.Y] == 'X') //If 'empty' space or 'weapon space'
                {
                    neighborNodes.Add(new Point(current.X, current.Y)); //we've found another node to add
                    break;
                }

                if (current.X == 21)
                {
                    break; //If we reach the end of the world, stop search.
                }
            } //This repeats for the other directions. 

            current.X = currentOriginX;
            for (int m = 0; m < 9; m++)
            {
                if (current.X > 0)
                {
                    current.X -= 1;

                }
                if (mapArray[current.X, current.Y] == 'Q' || mapArray[current.X, current.Y] == 'W' ||
                    mapArray[current.X, current.Y] == 'E' || mapArray[current.X, current.Y] == 'R' ||
                    mapArray[current.X, current.Y] == 'T' || mapArray[current.X, current.Y] == 'Y' ||
                    mapArray[current.X, current.Y] == 'U' || mapArray[current.X, current.Y] == 'I' ||
                    mapArray[current.X, current.Y] == 'O' || mapArray[current.X, current.Y] == 'P' ||
                    mapArray[current.X, current.Y] == 'A' || mapArray[current.X, current.Y] == 'D')
                {
                    break;
                }

                if (mapArray[current.X, current.Y] == ' ' || mapArray[current.X, current.Y] == 'X')
                {
                    neighborNodes.Add(new Point(current.X, current.Y));
                    break;
                }

                if (current.X == 0)
                {
                    break;
                }
            }

            current.X = currentOriginX;
            for (int m = 0; m < 9; m++)
            {
                current.Y += 1;
                if (mapArray[current.X, current.Y] == 'Q' || mapArray[current.X, current.Y] == 'W' ||
                    mapArray[current.X, current.Y] == 'E' || mapArray[current.X, current.Y] == 'R' ||
                    mapArray[current.X, current.Y] == 'T' || mapArray[current.X, current.Y] == 'Y' ||
                    mapArray[current.X, current.Y] == 'U' || mapArray[current.X, current.Y] == 'I' ||
                    mapArray[current.X, current.Y] == 'O' || mapArray[current.X, current.Y] == 'P' ||
                    mapArray[current.X, current.Y] == 'A' || mapArray[current.X, current.Y] == 'D')
                {
                    break;
                }

                if (mapArray[current.X, current.Y] == ' ' || mapArray[current.X, current.Y] == 'X')
                {
                    neighborNodes.Add(new Point(current.X, current.Y));
                    break;
                }

                if (current.Y == 21)
                {
                    break;
                }
            }

            current.Y = currentOriginY;
            for (int m = 0; m < 9; m++)
            {
                if (current.Y > 0)
                {
                    current.Y -= 1;

                }
                if (mapArray[current.X, current.Y] == 'Q' || mapArray[current.X, current.Y] == 'W' ||
                    mapArray[current.X, current.Y] == 'E' || mapArray[current.X, current.Y] == 'R' ||
                    mapArray[current.X, current.Y] == 'T' || mapArray[current.X, current.Y] == 'Y' ||
                    mapArray[current.X, current.Y] == 'U' || mapArray[current.X, current.Y] == 'I' ||
                    mapArray[current.X, current.Y] == 'O' || mapArray[current.X, current.Y] == 'P' ||
                    mapArray[current.X, current.Y] == 'A' || mapArray[current.X, current.Y] == 'D')
                {
                    break;
                }

                if (mapArray[current.X, current.Y] == ' ' || mapArray[current.X, current.Y] == 'X')
                {
                    neighborNodes.Add(new Point(current.X, current.Y));
                    break;
                }

                if (current.Y == 0)
                {
                    break;
                }
            }

            current.Y = currentOriginY;
            #endregion
            return neighborNodes; //Returns all the found neighborNodes to input node.
        }

        public Point RandomNode(Point currentPoint)
        {
            bool foundNode = false;
            do
            {
                rnd = new Random(rnd.Next(1, 1000));  //Primarily used of a backup just to send zombie away if a problem occurs. 
                int nodeIndex = rnd.Next(0, listOfNodes.Count); //No real function in game.
                
                nextPoint = new Point(((int)listOfNodes.ElementAt(nodeIndex).Position.X - 5) / 10, ((int)listOfNodes.ElementAt(nodeIndex).Position.Z - 5) / 10);

                if (nextPoint != currentPoint)
                {
                    foundNode = true;
                }
            } while (foundNode == false);

            return nextPoint;
        }

        int DistanceBetweenTwoPoints(Point p1, Point p2)
        {
            int Px = (int)p1.X;
            int Pz = (int)p1.Y;
            int Nx = (int)p2.X;
            int Nz = (int)p2.Y; //Finds distance between 2 points, usually used to find valid nodes to spawn on.

            double distance = Math.Sqrt(((Px - Nx) * (Px - Nx)) + ((Pz - Nz) * (Pz - Nz)));
            return (int)distance;
        }

        public Point GetRandomStart(Point currentPoint, List<Point> zombieStart)
        {
            Point nextPoint; //Finds a start for a zombie. 
            bool foundNode = false;
            do
            {
                int nodeIndex = rnd.Next(0, listOfNodes.Count);
                nextPoint = new Point(((int)listOfNodes.ElementAt(nodeIndex).Position.X - 5) / 10, ((int)listOfNodes.ElementAt(nodeIndex).Position.Z - 5) / 10);
                int distance = DistanceBetweenTwoPoints(currentPoint, nextPoint);
                if (distance > 3 && distance < 10) //Random chooses node, if the distance between zombie + player at spawn is between these
                {                                //It's classified as a valid spawn and will be returned the point generated.
                    if (!zombieStart.Contains(nextPoint))
                        foundNode = true;
                    if (nextPoint.X == 1 || nextPoint.Y == 1 || nextPoint.X == 20 || nextPoint.Y == 20)
                        foundNode = false;
                }

            } while (foundNode == false);

            return nextPoint;
        }

        private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            if (!cameFrom.Keys.Contains(current))
            {
                return new List<Point> { };
            }

            List<Point> path = ReconstructPath(cameFrom, cameFrom[current]); //Loops through to reconstruct the path we followed to player.
            path.Add(current);
            return path;
        }

        public BoundingBox GetBoundingBox(Node inNode)
        {
            BoundingBox newBox = inNode.collisionBox; //Simple get for the boundingbox of a node, used for checking
            return newBox;                            // when zombie's target position needs to be updated
        }


    }
}
