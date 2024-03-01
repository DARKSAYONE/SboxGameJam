using Sandbox;

public sealed class PlayerStats : Component
{
	[Sync] public int Level { get; set; }
	[Sync] public int Experience { get; set; }
	[Sync] public int HP { get; set; }
	[Sync] public int Mana { get; set; }
	[Sync] public float ManaRegen { get; set; }
	[Sync] public int PhysicalPower { get; set; }
	[Sync] public int MindPower { get; set; }
	[Sync] public int Protection { get; set; }
	[Sync] public int Fortitude { get; set; }
	[Sync] public float MovementSpeed { get; set; }
	[Sync] public float HitSpeed { get; set; }
}
