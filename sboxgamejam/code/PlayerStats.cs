using Sandbox;

public sealed class PlayerStats : Component
{
	[Property] public int Level;
	[Property] public int Experience;
	[Property] public int HP;
	[Property] public int Mana;
	[Property] public float ManaRegen;
	[Property] public int PhysicalPower;
	[Property] public int MindPower;
	[Property] public int Protection;
	[Property] public int Fortitude;
	[Property] public float MovementSpeed;
	[Property] public float HitSpeed;
}
