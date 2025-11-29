using UnityEngine;
namespace StudiesWork.DataModifiers
{
	public class WorldBuild
	{
		public const short STRAIGTH_COST = 10;
		public const short DIAGONAL_COST = 14;
		private enum Layers
		{
			Scene,
			Target
		};
		public static readonly LayerMask SceneMask = GetMask(Layers.Scene);
		public static readonly LayerMask TargetMask = GetMask(Layers.Target);
		private static LayerMask GetMask(Layers layerName)
		{
			for(int i = 0; i < 32; i++)
				if(LayerMask.LayerToName(i) != string.Empty && LayerMask.LayerToName(i).ToUpper() == layerName.ToString().ToUpper())
					return 1 << i;
			return 0;
		}
	};
};