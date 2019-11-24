using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.gui;
using C3.MonoGame;

namespace EVCMonoGame.src.A.I
{
	public class Pathfinder
	{
		//public Rectangle Size;

		public byte[,] navGrid;
		public Rectangle gridBounds;
		public int agentMindestBreite;

		public Pathfinder(Rectangle gridBounds, int agentMindestBreite = 10)
		{
			//	Size = new Rectangle(0, 0, x, y);
			//	Weight = new byte[x, y];

			//	for (var i = 0; i < x; i++)
			//	{
			//		for (var j = 0; j < y; j++)
			//		{
			//			Weight[i, j] = defaultValue;
			//		}
			//	}
			this.gridBounds = gridBounds;
			this.agentMindestBreite = agentMindestBreite;

			//Weight = CollisionManager.GenerateNavGrid(gridBounds, agentMindestBreite); // neue unfertige implementation
			navGrid = CollisionManager.GenerateLevelNavGrid(gridBounds.Width, gridBounds.Height, agentMindestBreite);
		}

		// To do: Implementierung einer Lösung die Relativ zum Agenten ist. Spart immens performance
		public List<Point> PathfindTo(Point to)
		{

			Point start = new Point(
				(gridBounds.Center.X - gridBounds.Location.X) / agentMindestBreite - 1,
				(gridBounds.Center.Y - gridBounds.Location.Y) / agentMindestBreite - 1
			);

			to = new Point(
				(to.X - gridBounds.Center.X + gridBounds.Width / 2) / agentMindestBreite - 1,
				(to.Y - gridBounds.Center.Y + gridBounds.Height / 2) / agentMindestBreite - 1
			);

			Console.WriteLine("Start: " + start);
			Console.WriteLine("To: " + to);

			return Pathfind(start, to);
		}

		public bool IsPositionInNavgrid(Vector2 WorldPosition)
		{
			// Convert WorldPosition to Grid coordinate
			Point gridCoordinate = new Point((int)WorldPosition.X / agentMindestBreite, (int)WorldPosition.Y / agentMindestBreite);

			// Wenn außerhalb vom Navgrid
			if (gridCoordinate.X < 0 || gridCoordinate.Y < 0 || gridCoordinate.X >= navGrid.GetLength(0) || gridCoordinate.Y >= navGrid.GetLength(1))
				return false;

			if (navGrid[gridCoordinate.X, gridCoordinate.Y] == 0)
				return true;
			else
				return false;
		}
		

		///	<summary>
		///	A* Algo übernommen und überarbeitet von http://stevephillips.me/blog/implementing-pathfinding-algorithm-xna
		///	Algo hat aber einige Fehler die ich noch näher untersuchen muss
		///	</summary>
		public List<Point> Pathfind(Point start, Point end)
		{

			// Abstrahiere WorldPosition auf Navgrid
			Point locStart = new Point((int)start.X / agentMindestBreite, (int)start.Y / agentMindestBreite);
			Point locEnd = new Point((int)end.X / agentMindestBreite, (int)end.Y / agentMindestBreite);

			// Wenn außerhalb vom Navgrid
			if (locStart.X < 0 || locStart.Y < 0 || locStart.X >= navGrid.GetLength(0) || locStart.Y >= navGrid.GetLength(1) ||
				locEnd.X < 0 || locEnd.Y < 0 || locEnd.X >= navGrid.GetLength(0) || locEnd.Y >= navGrid.GetLength(1))
				return null;

			if (navGrid[locStart.X, locStart.Y] == 1)
			{
				return null;
			}
			if (navGrid[locEnd.X, locEnd.Y] == 1)
			{
				return null;
			}

			// nodes that have already been analyzed and have a path from the start to them
			var closedSet = new List<Point>();
			// nodes that have been identified as a neighbor of an analyzed node, but have 
			// yet to be fully analyzed
			var openSet = new List<Point> { locStart };
			// a dictionary identifying the optimal origin point to each node. this is used 
			// to back-track from the end to find the optimal path
			var cameFrom = new Dictionary<Point, Point>();
			// a dictionary indicating how far each analyzed node is from the start
			var currentDistance = new Dictionary<Point, int>();
			// a dictionary indicating how far it is expected to reach the end, if the path 
			// travels through the specified node. 
			var predictedDistance = new Dictionary<Point, float>();


			// initialize the start node as having a distance of 0, and an estmated distance 
			// of y-distance + x-distance, which is the optimal path in a square grid that 
			// doesn't allow for diagonal movement
			currentDistance.Add(locStart, 0);
			predictedDistance.Add(
				locStart,
				0 + +Math.Abs(locStart.X - locEnd.X) + Math.Abs(locStart.Y - locEnd.Y)
			);

			int debugCounter = 0;

			// if there are any unanalyzed nodes, process them
			while (openSet.Count > 0)
			{
				// get the node with the lowest estimated cost to finish
				var current = (
					from p in openSet orderby predictedDistance[p] ascending select p
				).First();


				// if it is the finish, return the path
				if (current.X == locEnd.X && current.Y == locEnd.Y)
				{
					// generate the found path
					return ConvertWaypointsToWorldPosition(ReconstructPath(cameFrom, locEnd));
				}

				// Optimiertere Variante die Grob die Lösung akzeptiert
				//if (Math.Abs(current.X - end.X) <= 1 && Math.Abs(current.Y - end.Y) <= 1)
				//{
				//	Console.WriteLine("Current:" + current + " and End:" + end);
				//	// generate the found path
				//	return ReconstructPath(cameFrom, end);
				//}
				//Console.WriteLine(debugCounter++);
				//Console.WriteLine(current);

				// move current node from open to closed
				openSet.Remove(current);
				closedSet.Add(current);

				// process each valid node around the current node
				foreach (var neighbor in GetNeighborNodes(current))
				{
					var tempCurrentDistance = currentDistance[current] + 1;

					// if we already know a faster way to this neighbor, use that route and 
					// ignore this one
					if (closedSet.Contains(neighbor)
						&& tempCurrentDistance >= currentDistance[neighbor])
					{
						continue;
					}

					// if we don't know a route to this neighbor, or if this is faster, 
					// store this route
					if (!closedSet.Contains(neighbor)
						|| tempCurrentDistance < currentDistance[neighbor])
					{
						if (cameFrom.Keys.Contains(neighbor))
						{
							cameFrom[neighbor] = current;
						}
						else
						{
							cameFrom.Add(neighbor, current);
						}

						currentDistance[neighbor] = tempCurrentDistance;
						predictedDistance[neighbor] =
							currentDistance[neighbor]
							+ Math.Abs(neighbor.X - locEnd.X)
							+ Math.Abs(neighbor.Y - locEnd.Y);

						// if this is a new node, add it to processing
						if (!openSet.Contains(neighbor))
						{
							openSet.Add(neighbor);
						}
					}
				}
			}

			// unable to figure out a path, abort.
			//throw new Exception(
			//	string.Format(
			//		"unable to find a path between {0},{1} and {2},{3}",
			//		start.X, start.Y,
			//		end.X, end.Y
			//	)
			//);
			return null;
		}

		/// <summary>
		/// Return a list of accessible nodes neighboring a specified node
		/// </summary>
		/// <param name="node">The center node to be analyzed.</param>
		/// <returns>A list of nodes neighboring the node that are accessible.</returns>
		private IEnumerable<Point> GetNeighborNodes(Point node)
		{
			var nodes = new List<Point>();


			// up
			if (node.Y != 0 && navGrid[node.X, node.Y - 1] == 0)
			{
				nodes.Add(new Point(node.X, node.Y - 1));
			}

			// right
			if (node.X + 1 < navGrid.GetLength(0) && navGrid[node.X + 1, node.Y] == 0)
			{
				nodes.Add(new Point(node.X + 1, node.Y));
			}

			// down
			if (node.Y + 1 < navGrid.GetLength(1) && navGrid[node.X, node.Y + 1] == 0)
			{
				nodes.Add(new Point(node.X, node.Y + 1));
			}

			// left
			if (node.X != 0 && navGrid[node.X - 1, node.Y] == 0)
			{
				nodes.Add(new Point(node.X - 1, node.Y));
			}

			return nodes;
		}

		/// <summary>
		/// Process a list of valid paths generated by the Pathfind function and return 
		/// a coherent path to current.
		/// </summary>
		/// <param name="cameFrom">A list of nodes and the origin to that node.</param>
		/// <param name="current">The destination node being sought out.</param>
		/// <returns>The shortest path from the start to the destination node.</returns>
		private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
		{
			if (!cameFrom.Keys.Contains(current))
			{
				return new List<Point> { current };
			}


			var path = ReconstructPath(cameFrom, cameFrom[current]);
			path.Add(current);
			return path;
		}

		private List<Point> ConvertWaypointsToWorldPosition(List<Point> GridWaypoints)
		{
			List<Point> waypoints = new List<Point>();

			foreach(Point waypoint in GridWaypoints)
				waypoints.Add(new Point(waypoint.X * agentMindestBreite, waypoint.Y * agentMindestBreite));

			return waypoints;
		}
	}
}
