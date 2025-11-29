using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using StudiesWork.DataModifiers;
namespace StudiesWork.PathFinding
{
	public sealed class PathFinder
	{
		private readonly Grid _grid;
		private Heap<Node> _openSet;
		private HashSet<Node> _closedSet;
		public PathFinder(Grid grid) => _grid = grid;
		public Vector2[] FindPath(Vector2 startPoint, Vector2 endPoint)
		{
			Node startNode = _grid.GetNode(startPoint);
			Node endNode = _grid.GetNode(endPoint);
			if (startNode is null || endNode is null || startNode.IsBlock || endNode.IsBlock)
				return null;
			Stopwatch stopwatch = Stopwatch.StartNew();
			_openSet = new Heap<Node>(_grid.Width * _grid.Height);
			_closedSet = new HashSet<Node>();
			for (int x = 0; x < _grid.Width; x++)
				for (int y = 0; y < _grid.Height; y++)
				{
					Node node = _grid.GetNode(x, y);
					node.Parent = null;
					node.CostG = int.MaxValue;
					node.CostH = 0;
				}
			startNode.CostG = 0;
			startNode.CostH = startNode.GetDistance(endNode);
			_openSet.Add(startNode);
			Node currentNode;
			while(_openSet.Count > 0)
			{
				currentNode = _openSet.RemoveFirst();
				_closedSet.Add(currentNode);
				if (currentNode == endNode)
				{
					stopwatch.Stop();
					InfoLogger.Informer.LogInfo($"Path found in {stopwatch.ElapsedMilliseconds} ms");
					return RetracePath(endNode);
				}
				int tentativeCostG;
				foreach (Node neighbour in _grid.GetNeighbours(currentNode))
				{
					if (_closedSet.Contains(neighbour))
						continue;
					if (neighbour.IsBlock)
					{
						_closedSet.Add(neighbour);
						continue;
					}
					tentativeCostG = currentNode.CostG + currentNode.GetDistance(neighbour);
					if (tentativeCostG < neighbour.CostG)
					{
						neighbour.Parent = currentNode;
						neighbour.CostG = tentativeCostG;
						neighbour.CostH = neighbour.GetDistance(endNode);
						if (!_openSet.Contains(neighbour))
							_openSet.Add(neighbour);
						else
							_openSet.UpdateItem(neighbour);
					}
				}
			}
			return null;
		}
		private Vector2[] RetracePath(Node endNode)
		{
			List<Node> path = new() { endNode };
			Node currentNode = endNode.Parent;
			while(currentNode != null)
			{
				path.Add(currentNode);
				currentNode = currentNode.Parent;
			}
			Vector2[] wayPoints = SimplifyPath(path);
			Array.Reverse(wayPoints);
			return wayPoints;
		}
		private Vector2[] SimplifyPath(List<Node> path)
		{
			List<Vector2> wayPoints = new();
			Vector2 oldDirection = Vector2.zero;
			Vector2 newDirection;
			for(ushort i = 1; i < path.Count; i++)
			{
				newDirection = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
				if(newDirection != oldDirection)
					wayPoints.Add(path[i].WorldPoint);
				oldDirection = newDirection;
			}
			return wayPoints.ToArray();
		}
	};
};