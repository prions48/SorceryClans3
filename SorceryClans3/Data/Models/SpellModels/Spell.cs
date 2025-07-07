using System;
using System.ComponentModel.DataAnnotations;
using SorceryClans3.Data.Abstractions;
namespace SorceryClans3.Data.Models
{
	public class Spell : ISpell
	{
		[Key] public Guid ID { get; set; }
		//base stats & casting
		public int MoneyToCast { get; set; }
		public int PowerToCast { get; set; }
		public int ColorToCast { get; set; }
		public MagicColor Color { get; set; }
		public int? UnprocessedConsumables { get; set; } = null;
		public int? Consumables { get; set; } = null;
		//main caster reqs
		public int? MagPowerToCast { get; set; }
		public int? MagColorToCast { get; set; }
		//results of spell
		public bool SoldierGranted { get; set; }
		public bool? Built { get; set; }
		public bool TargetSoldier
		{
			get
			{
				if (Beast != null || Harvest != null)
					return false;
				if (Power != null)
					return true;
				if (Spirit != null)
					return false;
				if (SpiritArtifact != null)
					return false;
				if (LesserUndead != null)
					return false;
				if (GreaterUndead != null)
					return true;
				if (LesserDemon != null)
					return false;
				if (GreaterDemon != null)
					return true;
				return false;
			}
		}
		public bool PowerGranted { get; set; }
		public bool ArtifactCreated { get; set; }
		//spell models
		public Beast? Beast { get; set; }
		public BeastHarvest? Harvest { get; set; }
		public PowerTemplate? Power { get; set; }
		public LesserSpirit? Spirit { get; set; }
		public GreaterSpirit? SpiritArtifact { get; set; }
		public LesserUndead? LesserUndead { get; set; }
		public GreaterUndead? GreaterUndead { get; set; }
		public LesserDemon? LesserDemon { get; set; }
		public GreaterDemon? GreaterDemon { get; set; }
		public AngelIcon? AngelIcon { get; set; }
		public bool Castable
		{
			get
			{
				if (Power != null)
					return true;
				if (Beast != null)
					return true;
				if (Harvest != null)
					return Consumables > 0;
				if (Spirit != null)
					return true;
				if (SpiritArtifact != null)
					return Built == false;
				if (LesserUndead != null)
					return true;
				if (GreaterUndead != null)
					return true;
				if (LesserDemon != null)
					return true;
				if (GreaterDemon != null)
					return GreaterDemon.Invested == null;
				if (AngelIcon != null)
					return true;
				return true;
			}
		}
		public bool CastQuantity => Harvest != null || LesserUndead != null || LesserDemon != null || Beast != null || AngelIcon != null;
		public bool ProcessesConsumables => Harvest != null;
		public bool UsesConsumables => Harvest != null || LesserUndead != null || Beast != null;
		public bool GeneratesSoldier => LesserUndead != null || Spirit != null;
		public bool GeneratesArtifact => SpiritArtifact != null || AngelIcon != null;//soon to add more!
		
		public int CastLimit(int money)
		{
			int lim = money / MoneyToCast;
			if (Harvest != null && UnprocessedConsumables != null && UnprocessedConsumables < lim)
				lim = UnprocessedConsumables.Value;
			return lim;
		}
		public Soldier? GenerateSoldier()
		{
			if (Spirit != null)
				return Spirit.GenerateSoldier();
			if (LesserDemon != null)
				return LesserDemon.GenerateSoldier();
			//the rest are generated from missions using the items prepared here (beast, necromancy, etc)
			return null;
		}
		public Artifact? GenerateArtifact()
		{
			if (SpiritArtifact != null)
				return SpiritArtifact.Artifact;
			if (AngelIcon != null)
				return AngelIcon.GenerateIcon();
			return null;
		}
		public string SpellName
		{
			get
			{
				if (Beast != null)
					return "Tame the " + Beast.FullName;
				if (Harvest != null)
					return "Harvest the " + Harvest.HarvestName;
				if (Power != null)
					return "Implant " + Power.PowerName;
				if (Spirit != null)
					return "Summon " + Spirit.GetName;
				if (SpiritArtifact != null)
					return "Construct " + SpiritArtifact.Artifact.ArtifactName;
				if (LesserUndead != null)
					return "Build " + ConsumablePrint(0) + " to resurrect " + LesserUndead.UndeadName;
				if (GreaterUndead != null)
					return "Resurrect " + GreaterUndead.UndeadName;
				if (LesserDemon != null)
					return "Summon " + LesserDemon.DemonName;
				if (GreaterDemon != null)
					return "Invest " + GreaterDemon.DemonName;
				if (AngelIcon != null)
					return "Create " + AngelIcon.ArtifactName();
				return "Unknown";
			}
		}
		public string? ConsumableName
		{
			get
			{
				if (Beast != null)
					return Beast.ToolName;
				if (Harvest != null)
					return Harvest.item;
				if (LesserUndead != null)
					return LesserUndead.tool;
				return null;
			}
		}
		public string? ConsumableCountDisplay
		{
			get
			{
				return Consumables.ToString() + " " + ConsumablePrint();
			}
		}
		public string? ConsumablePrint(int? count = null)
		{
			string? con = ConsumableName;
			if (con == null)
				return null;
			if (count == null)
				count = Consumables ?? 0;
			if (count != 1 && con[con.Length - 1] != 's')
			{
				if (con == "harness")
					con = con + "e";
				con = con + "s";
			}
			return con.Substring(0, 1).ToUpper() + con.Substring(1);
		}
		public string CastDisplay
		{
			get
			{
				string color = "";
				if (ColorToCast > 0)
					color = " and " + ColorToCast.ToString() + " " + Color.ToString() + " color";
				return PowerToCast.ToString() + " power" + color;
			}
		}
		public Spell()
		{
			ID = Guid.NewGuid();
		}
		public Spell(MagicColor color, ResearchDiscovery disco, int powerpts)
		{
			ID = Guid.NewGuid();
			Random r = new Random();
			bool zerocolor = false;
            if (color == MagicColor.None)
			{
				zerocolor = true;
				if (r.NextDouble() > .4)
				{
					switch (r.Next(6))
					{
						case 0: color = MagicColor.Black; break;
						case 1: color = MagicColor.Red; break;
						case 2: color = MagicColor.Blue; break;
						case 3: color = MagicColor.Green; break;
						case 4: color = MagicColor.White; break;
						default: color = MagicColor.Purple; break;
					}
				}
			}
			Color = color;
			int pts = powerpts.PointsToScore(disco);
			switch (disco)
			{
				case ResearchDiscovery.Power: Power = Power = new PowerTemplate(Guid.Empty, pts, color) { Heritability = null }; break;
				case ResearchDiscovery.BeastTame:
					Beast = new Beast(powerpts.PointsToScore(disco));
					Consumables = r.Next(3) + 3; //testing
					break;
				case ResearchDiscovery.BeastHarvest: Consumables = 3; UnprocessedConsumables = 3; break;//set in Research.cs
				case ResearchDiscovery.SpiritSoldier: Spirit = new LesserSpirit(pts, r.NextDouble() < .15); break;
				case ResearchDiscovery.SpiritArtifact: SpiritArtifact = new GreaterSpirit(pts); Built = false; break;
				case ResearchDiscovery.LesserUndead:
					LesserUndead = new LesserUndead(pts);
					Consumables = r.Next(3) + 3; break;
				case ResearchDiscovery.GreaterUndead: GreaterUndead = new GreaterUndead(pts); break;
				case ResearchDiscovery.LesserDemon: LesserDemon = new LesserDemon(pts); break;
				case ResearchDiscovery.GreaterDemon: GreaterDemon = new GreaterDemon(pts); break;
				case ResearchDiscovery.Angel: Angel angel = new Angel(pts); AngelIcon = new(angel); break;
				default: break;
			}
			SetCosts(powerpts, disco.NeedsCaster(), zerocolor);
		}
		public void SetCosts(int powerpts, bool caster, bool zerocolor)
		{
			Random r = new Random();
			MoneyToCast = 1000 + powerpts/100 + r.Next(powerpts/1000);
			PowerToCast = 10000 + powerpts/100 + r.Next(powerpts/1000);
			ColorToCast = zerocolor ? 0 : 100 + 10 * (powerpts/50000) + r.Next(10);
			if (caster)
			{
				MagPowerToCast = 1500 + (powerpts/100000)*10 + r.Next(10) * 10;
				MagColorToCast = Color == MagicColor.None ? 0 : r.Next(3) + 1;
			}
		}
		public bool CanCast(Soldier soldier)
		{
			if (soldier.Magic * soldier.PowerLevel < MagPowerToCast)
				return false;
			IList<MagicColor> colors = soldier.GetColors;
			if (colors.Count == 0)
				return MagColorToCast == 0;
			if (colors.First() == this.Color)
				return colors.Count >= MagColorToCast;
			return false;
		}
        public string SpellIcon
		{
			get
			{
				switch (Color)
				{
					case MagicColor.None: return MudBlazor.Icons.Material.Filled.Cancel;
					case MagicColor.Black: return MudBlazor.Icons.Material.Filled.Nightlight;
					case MagicColor.Red: return MudBlazor.Icons.Material.Filled.LocalFireDepartment;
					case MagicColor.Blue: return MudBlazor.Icons.Material.Filled.WaterDrop;
					case MagicColor.Green: return MudBlazor.Icons.Material.Filled.LocalFlorist;
					case MagicColor.White: return MudBlazor.Icons.Material.Filled.WbSunny;
					case MagicColor.Purple: return MudBlazor.Icons.Material.Filled.AutoFixHigh;
					default: return "";
                }
			}
		}
        public MudBlazor.Color IconColor
        {
            get
            {
                switch (Color)
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
        }

    }
}

