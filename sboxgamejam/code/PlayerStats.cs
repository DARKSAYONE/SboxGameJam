using Sandbox;

public sealed class PlayerStats : Component
{
	[Sync] [Property] public int Level { get; set; }
	[Sync] [Property] public int Experience { get; set; }
	[Sync] [Property] public int HP { get; set; }
	[Sync] [Property] public int Mana { get; set; }
	[Sync] [Property] public float ManaRegen { get; set; }
	[Sync] [Property] public int PhysicalPower { get; set; }
	[Sync] [Property] public int MindPower { get; set; }
	[Sync] [Property] public int Protection { get; set; }
	[Sync] [Property] public int Fortitude { get; set; }
	[Sync] [Property] public float MovementSpeed { get; set; }
	[Sync] [Property] public float HitSpeed { get; set; }
}
