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


	[Property] GameObject QSkill;
	[Property] GameObject ESkill;

	[Property]
	public SoundPointComponent SoundPoint;

	[Property]
	public bool CanShoot = true;

	[Property]
	public float AttackOnecooldownTime = 2.0f;
	private bool AttackOneOnCooldown = false;
	private float AttackOneTimerCooldown;

	[Property]
	public float QSkillCooldownTime = 5.0f;
	private bool QSkillOnCooldown = false;
	private float QSkillTimerCooldown;

	[Property]
	public float ESkillCooldownTime = 5.0f;
	private bool ESkillOnCooldown = false;
	private float ESkillTimerCooldown;

	protected override void OnStart()
	{
		base.OnStart();
		AttackOneTimerCooldown = AttackOnecooldownTime;
		ESkillTimerCooldown = ESkillCooldownTime;
		QSkillTimerCooldown = QSkillCooldownTime;
	}

	protected override void OnUpdate()
	{
		CooldownChecker();
		CastFireball();
		CastQSkill();
		CastESkill();
	}

	void CastFireball()
	{
		if ( !IsProxy )
		{
			if ( Input.Pressed( "Attack1" ) && !AttackOneOnCooldown )
			{
				var netFireball = FireballPrefab.Clone( FireballSpawnPos.Transform.Position + Vector3.Forward, FireballSpawnPos.Parent.Transform.Rotation );
				netFireball.Name = $"{GameObject.Name} - Fireball";
				netFireball.NetworkSpawn( GameObject.Network.OwnerConnection );
				AttackOneOnCooldown = true;

			}
		}
	}

	void CastQSkill()
	{
		if ( !IsProxy )
		{
			if ( Input.Pressed( "QUse" ) && !QSkillOnCooldown)
			{
				var netFireball = QSkill.Clone( FireballSpawnPos.Transform.Position + Vector3.Forward, FireballSpawnPos.Parent.Transform.Rotation );
				netFireball.Name = $"{GameObject.Name} - QSkill";
				netFireball.NetworkSpawn( GameObject.Network.OwnerConnection );
				QSkillOnCooldown = true;

			}
		}
	}

	void CastESkill()
	{
		if ( !IsProxy )
		{
			if ( Input.Pressed( "use" ) && !ESkillOnCooldown )
			{
				var ESkillCast = ESkill.Clone( FireballSpawnPos.Transform.Position + Vector3.Forward, FireballSpawnPos.Parent.Transform.Rotation );
				ESkillCast.Name = $"{GameObject.Name} - ESkill";
				ESkillCast.NetworkSpawn( GameObject.Network.OwnerConnection );
				ESkillOnCooldown = true;
			}
		}
	}

	void CooldownChecker()
	{
		if ( AttackOneOnCooldown )
			AttackOneCooldownTimer();

		if(QSkillOnCooldown)
			QSkillCooldownTimer();

		if ( ESkillOnCooldown )
			ESkillCooldownTimer();
	}
	

	void AttackOneCooldownTimer()
	{
		if ( AttackOneTimerCooldown > 0 )
		{
			AttackOneTimerCooldown -= Time.Delta;
		}
		else if ( AttackOneTimerCooldown < 0 ) 
		{
			SoundPoint.StartSound();
			AttackOneOnCooldown = false;
			AttackOneTimerCooldown = AttackOnecooldownTime;
		}

	}

	void QSkillCooldownTimer()
	{
		if ( QSkillTimerCooldown > 0 )
		{
			QSkillTimerCooldown -= Time.Delta;
		}
		else if ( QSkillTimerCooldown < 0 )
		{
			SoundPoint.StartSound();
			QSkillOnCooldown = false;
			QSkillTimerCooldown = QSkillCooldownTime;
		}

	}

	void ESkillCooldownTimer()
	{
		if ( ESkillTimerCooldown > 0 )
		{
			ESkillTimerCooldown -= Time.Delta;
		}
		else if ( ESkillTimerCooldown < 0 )
		{
			SoundPoint.StartSound();
			ESkillOnCooldown = false;
			ESkillTimerCooldown = ESkillCooldownTime;
		}

	}
}
