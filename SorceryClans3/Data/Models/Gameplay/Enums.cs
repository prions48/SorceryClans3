namespace SorceryClans3.Data.Models
{
	public enum SoldierType
	{
		Standard,
		LesserUndead,
		GreaterUndead,
		LesserSpirit,
		GreaterSpirit,
		LesserDemon,
		GreaterDemon,
		Beast,
		Nephilim
	}
	public enum HealthLevel
	{
		Uninjured = 0,
		Hurt = 1,
		Wounded = 2,
		Critical = 3,
		Dead = 4
	}
	public enum HealStatus
	{
		NotYet,
		NoHealNeeded,
		AllFail,
		HealOnly,
		LevelOnly,
		AllSuccess
	}
	public enum MagicColor
	{
		None = 0,
		Black = 1,
		Red = 2,
		Blue = 3,
		Green = 4,
		White = 5,
		Purple = 6
	}
	public static class EnumUtils
	{
		public static HealthLevel Hurt(this HealthLevel current, int lvl = 1)
		{
			if (lvl < 0)
				return current;
			int curr = (int)current;
			if (curr + lvl >= 4)
				return HealthLevel.Dead;
			return (HealthLevel)(curr + lvl);
		}
		public static HealthLevel Heal(this HealthLevel current, int lvl = 1)
		{
			if (lvl < 0)
				return current;
			int curr = (int)current;
			if (curr - lvl <= 0)
				return HealthLevel.Uninjured;
			return (HealthLevel)(curr - lvl);
		}
		public static string Icon(this HealStatus status)
		{
			switch (status)
			{
				case HealStatus.NotYet: return MudBlazor.Icons.Material.TwoTone.Cancel;
				case HealStatus.NoHealNeeded: return MudBlazor.Icons.Material.TwoTone.Cancel;
				case HealStatus.AllFail: return MudBlazor.Icons.Material.TwoTone.SmsFailed;
				case HealStatus.AllSuccess: return MudBlazor.Icons.Material.TwoTone.LocalHospital;
				case HealStatus.HealOnly: return MudBlazor.Icons.Material.TwoTone.Healing;
				case HealStatus.LevelOnly: return MudBlazor.Icons.Material.TwoTone.LocalHospital;
				default: return "";
			}
		}
		public static MudBlazor.Color Color(this HealStatus status)
		{
			switch (status)
			{
				case HealStatus.NotYet: return MudBlazor.Color.Default;
				case HealStatus.NoHealNeeded: return MudBlazor.Color.Default;
				case HealStatus.AllFail: return MudBlazor.Color.Error;
				case HealStatus.AllSuccess: return MudBlazor.Color.Success;
				case HealStatus.HealOnly: return MudBlazor.Color.Success;
				case HealStatus.LevelOnly: return MudBlazor.Color.Warning;
				default: return MudBlazor.Color.Default;
			}
		}
		public static MudBlazor.Color Color(this HealthLevel health)
		{
			switch (health)
			{
				case HealthLevel.Uninjured: return MudBlazor.Color.Default;
				case HealthLevel.Hurt: return MudBlazor.Color.Primary;
				case HealthLevel.Wounded: return MudBlazor.Color.Warning;
				case HealthLevel.Critical: return MudBlazor.Color.Error;
				default: return MudBlazor.Color.Error;
			}
		}
		public static string Icon(this MagicColor color)
		{
			switch (color)
			{
				case MagicColor.None: return MudBlazor.Icons.Material.Filled.HighlightOff;
				/* https://pictogrammers.com/library/mdi/ */
				case MagicColor.Black: return "<svg viewBox=\"0 0 24 24\"><path d=\"M12,2A9,9 0 0,0 3,11C3,14.03 4.53,16.82 7,18.47V22H9V19H11V22H13V19H15V22H17V18.46C19.47,16.81 21,14 21,11A9,9 0 0,0 12,2M8,11A2,2 0 0,1 10,13A2,2 0 0,1 8,15A2,2 0 0,1 6,13A2,2 0 0,1 8,11M16,11A2,2 0 0,1 18,13A2,2 0 0,1 16,15A2,2 0 0,1 14,13A2,2 0 0,1 16,11M12,14L13.5,17H10.5L12,14Z\" /></svg>";
				case MagicColor.Red: return MudBlazor.Icons.Material.Filled.LocalFireDepartment;
				case MagicColor.Blue: return MudBlazor.Icons.Material.Filled.WaterDrop;
				case MagicColor.Green: return "<svg viewBox=\"0 0 24 24\"><path d=\"M17,8C8,10 5.9,16.17 3.82,21.34L5.71,22L6.66,19.7C7.14,19.87 7.64,20 8,20C19,20 22,3 22,3C21,5 14,5.25 9,6.25C4,7.25 2,11.5 2,13.5C2,15.5 3.75,17.25 3.75,17.25C7,8 17,8 17,8Z\" /></svg>";
				case MagicColor.White: return MudBlazor.Icons.Material.Filled.WbSunny;
				case MagicColor.Purple: return "<svg viewBox=\"0 0 24 24\"><title>auto-fix</title><path d=\"M7.5,5.6L5,7L6.4,4.5L5,2L7.5,3.4L10,2L8.6,4.5L10,7L7.5,5.6M19.5,15.4L22,14L20.6,16.5L22,19L19.5,17.6L17,19L18.4,16.5L17,14L19.5,15.4M22,2L20.6,4.5L22,7L19.5,5.6L17,7L18.4,4.5L17,2L19.5,3.4L22,2M13.34,12.78L15.78,10.34L13.66,8.22L11.22,10.66L13.34,12.78M14.37,7.29L16.71,9.63C17.1,10 17.1,10.65 16.71,11.04L5.04,22.71C4.65,23.1 4,23.1 3.63,22.71L1.29,20.37C0.9,20 0.9,19.35 1.29,18.96L12.96,7.29C13.35,6.9 14,6.9 14.37,7.29Z\" /></svg>";
				default: return "";
			}
		}
		public static MudBlazor.Color Color(this MagicColor color)
		{
			switch (color)
			{
				case MagicColor.None: return MudBlazor.Color.Default;
				case MagicColor.Black: return MudBlazor.Color.Dark;
				case MagicColor.Red: return MudBlazor.Color.Error;
				case MagicColor.Blue: return MudBlazor.Color.Info;
				case MagicColor.Green: return MudBlazor.Color.Success;
				case MagicColor.White: return MudBlazor.Color.Warning;
				case MagicColor.Purple: return MudBlazor.Color.Primary;
				default: return MudBlazor.Color.Inherit;
			}
		}
		/// <summary>
		/// can this soldier do leadership and research, and cost leadership weight?
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool Independent(this SoldierType type)
		{
			switch (type)
			{
				case SoldierType.Standard: return true;
				case SoldierType.Beast: return false;
				case SoldierType.LesserUndead: return false;
				case SoldierType.GreaterUndead: return true;
				case SoldierType.LesserSpirit: return false;
				case SoldierType.GreaterSpirit: return false;
				case SoldierType.LesserDemon: return false;
				case SoldierType.GreaterDemon: return true;
				case SoldierType.Nephilim: return true;
				default: return false;
			}
		}
		public static int PowerAdj(this SoldierType type, int factor)
		{
			switch (type)
			{
				case SoldierType.LesserSpirit: case SoldierType.LesserDemon: return 0;
				case SoldierType.LesserUndead: case SoldierType.GreaterUndead: return factor / 3;
				case SoldierType.Nephilim: return factor / 2;
				case SoldierType.Standard: case SoldierType.GreaterDemon: case SoldierType.Beast: return factor;
				case SoldierType.GreaterSpirit: return factor * 2;
				default: return factor;
			}
		}
	}
	public enum SkillStat
	{
		Combat = 0,
		Magic = 1,
		Subtlety = 2,
		Heal = 3
	}
	public enum BoostStat
	{
		Combat = 0,
		Magic = 1,
		Subtlety = 2,
		HP = 3,
		Heal = 4
	}
	public enum OccupiedStatus
    {
        All,
        Occupied,
        Unoccupied
    }
}