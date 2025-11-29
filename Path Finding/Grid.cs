using UnityEngine;
using System.Collections.Generic;
using StudiesWork.DataModifiers;
namespace StudiesWork.PathFinding
{
	public readonly struct Grid
	{
		private readonly Node[,] _grid;
		private readonly Vector2 _originPoint;
		private readonly Vector2 _centerPoint;
		private readonly float _cellSize;
		private readonly int _width;
		private readonly int _height;
		public readonly int Width => _width;
		public readonly int Height => _height;
		public Grid(Collider2D collider, Vector2 originPoint, float lookDistance)
		{
			_originPoint = originPoint;
			_cellSize = (collider.bounds.extents.x + collider.bounds.extents.y) / 2f;
			_width = (int)(lookDistance * 2f);
			_height = (int)(lookDistance * 2f);
			_grid = new Node[_width, _height];
			_centerPoint = new Vector2(_width, _height) / 2f;
			Vector2 worldPoint;
			for(int x = 0; x < _width; x++)
				for(int y = 0; y < _height; y++)
				{
					worldPoint = (new Vector2(x, y) - _centerPoint) * _cellSize + _originPoint;
					_grid[x, y] = new Node(worldPoint, Physics2D.OverlapCircle(worldPoint, _cellSize, WorldBuild.SceneMask), x, y);
				}
		}
		public readonly Node GetNode(int x, int y) => _grid[x, y];
		public Node GetNode(Vector2 worldPoint)
		{
			Vector2 point = (worldPoint - _originPoint) / _cellSize + _centerPoint;
			return GetNode(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));
		}
		public readonly Node[] GetNeighbours(Node node)
		{
			List<Node> neighbours = new();
			int checkX;
			int checkY;
			for(short x = -1; x <= 1; x++)
				for(short y = -1; y <= 1; y++)
					if(x != 0 || y != 0)
					{
						checkX = node.GridX + x;
						checkY = node.GridY + y;
						if(checkX >= 0 && checkX < _width && checkY >= 0 && checkY < _height)
							neighbours.Add(_grid[checkX, checkY]);
					}
			return neighbours.ToArray();
		}
	};
};
