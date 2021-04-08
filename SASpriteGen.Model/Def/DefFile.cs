using System.Collections.Generic;

namespace SASpriteGen.Model.Def
{
	public class DefFile
	{
		public string Name;

		public DefType Type;
		public int Width;
		public int Height;
		public int GroupsCount;

		public Dictionary<DefAnimation, DefGroup> Groups;

		public DefFile(string name)
		{
			Name = name;

			Groups = new Dictionary<DefAnimation, DefGroup>();
		}
	}
}
