using System.Collections.Generic;

namespace SASpriteGen.Model.Def
{
	public class DefGroup
	{
		public DefAnimation GroupNum;
		public int ItemsCount;

		public List<DefGroupItem> Items;

		public DefGroup()
		{
			Items = new List<DefGroupItem>();
		}

		public string GetGroupName(DefType type)
		{
			return type switch
			{
				DefType.Spells => (int)GroupNum switch
				{
					0 => "Group",
					_ => "Unhandled"
				},
				DefType.Creature => (int)GroupNum switch
				{
					0 => "Moving",
					1 => "Mouse Over",
					2 => "Standing",
					3 => "Getting Hit",
					4 => "Defend",
					5 => "Death",
					6 => "Unused Death",
					7 => "Turn Left (1)",
					8 => "Turn Right (1)",
					9 => "Turn Left (2)",
					10 => "Turn Right (2)",
					11 => "Attack Up",
					12 => "Attack Straight",
					13 => "Attack Down",
					14 => "Shoot Up",
					15 => "Shoot Straight",
					16 => "Shoot Down",
					17 => "2-Hex/Spell Attack Up",
					18 => "2-Hex/Spell Attack Straight",
					19 => "2-Hex/Spell Attack Down",
					20 => "Start Moving",
					21 => "Stop Moving",
					_ => "Unhandled"
				},
				DefType.AdventureObject => (int)GroupNum switch
				{
					0 => "Group",
					_ => "Unhandled"
				},
				DefType.Hero => (int)GroupNum switch
				{
					0 => "Looking Up",
					1 => "Looking Up-Right",
					2 => "Looking Right",
					3 => "Looking Down-Right",
					4 => "Looking Down",
					5 => "Moving Up",
					6 => "Moving Up-Right",
					7 => "Moving Right",
					8 => "Moving Down-Right",
					9 => "Moving Down",
					_ => "Unhandled"
				},
				DefType.Terrain => (int)GroupNum switch
				{
					0 => "Group",
					_ => "Unhandled"
				},
				DefType.Cursor => (int)GroupNum switch
				{
					0 => "Group",
					_ => "Unhandled"
				},
				DefType.Interface => (int)GroupNum switch
				{
					0 => "Group",
					_ => "Unhandled"
				},
				DefType.CombatHero => (int)GroupNum switch
				{
					0 => "Standing",
					1 => "Shuffle",
					2 => "Failure",
					3 => "Victory",
					4 => "Cast Spell",
					_ => "Unhandled",
				},
				_ => "Unhandled"
			};
		}
	}
}
