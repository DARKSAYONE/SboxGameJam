using Sandbox;
using System.Diagnostics;

public sealed class FireballCast : Component
{
    [Property]
    public GameObject FireballPrefab;
    [Property]
    public GameObject FireballSpawnPos;
    [Property]	
    public GameObject CameraFind;
	[Property]
	public float CooldownTime = 2.0f;
	private bool OnCooldown = false;
	private float TimerCooldown;

	[Property] GameObject QSkill;

	[Property]
	public SoundPointComponent SoundPoint;

	[Property]
	public bool CanShoot = true;


	protected override void OnStart()
	{
		base.OnStart();
		TimerCooldown = CooldownTime;
	}

	protected override void OnUpdate()
    {
		if ( OnCooldown )
			CooldownTimer();

        CastFireball();
		CastQSkill();
    }

	void CastFireball()
	{
		if ( !IsProxy )
		{
			if ( Input.Pressed( "Attack1" ) && !OnCooldown )
			{
				var netFireball = FireballPrefab.Clone( FireballSpawnPos.Transform.Position + Vector3.Forward, FireballSpawnPos.Parent.Transform.Rotation );
				netFireball.Name = $"{GameObject.Name} - Fireball";
				netFireball.NetworkSpawn( GameObject.Network.OwnerConnection );
				OnCooldown = true;
				
			}
		}
	}

	void CastQSkill()
	{
		if ( !IsProxy )
		{
			if ( Input.Pressed( "QUse" ) && !OnCooldown )
			{
				var netFireball = QSkill.Clone( FireballSpawnPos.Transform.Position + Vector3.Forward, FireballSpawnPos.Parent.Transform.Rotation );
				netFireball.Name = $"{GameObject.Name} - QSkill";
				netFireball.NetworkSpawn( GameObject.Network.OwnerConnection );
				OnCooldown = true;

			}
		}
	}


	void CooldownTimer()
	{
		if ( TimerCooldown > 0 )
		{
			TimerCooldown -= Time.Delta;
		}
		else if ( TimerCooldown < 0 ) 
		{
			SoundPoint.StartSound();
			OnCooldown = false;
			TimerCooldown = CooldownTime;
		}

	}
}
