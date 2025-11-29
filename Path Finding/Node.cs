using UnityEngine;
using StudiesWork.DataModifiers;
namespace StudiesWork.PathFinding
{
   public class Node : IHeapItem<Node>
   {
      public Node Parent;
      public Vector2 WorldPoint;
      public bool IsBlock;
      public int GridX;
      public int GridY;
      public int CostG;
      public int CostH;
      public int heapIndex;
      public int CostF => CostG + CostH;
      public int HeapIndex { get => heapIndex; set => heapIndex = value; }
      public int CompareTo(Node other) => -(CostF.CompareTo(other.CostF) == 0 ? CostH.CompareTo(other.CostH) : CostF.CompareTo(other.CostF));
      public Node(Vector2 worldPoint, bool isBlock, int x, int y)
      {
			WorldPoint = worldPoint;
         IsBlock = isBlock;
         GridX = x;
         GridY = y;
         CostG = 0;
         CostH = 0;
      }
		public int GetDistance(Node otherNode)
		{
			int distX = Mathf.Abs(GridX - otherNode.GridX);
			int distY = Mathf.Abs(GridY - otherNode.GridY);
			if(distX > distY)
				return WorldBuild.DIAGONAL_COST * distY + WorldBuild.STRAIGTH_COST * (distX - distY);
			return WorldBuild.DIAGONAL_COST * distX + WorldBuild.STRAIGTH_COST * (distY - distX);
		}
	};
};