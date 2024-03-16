using Sandbox.Citizen;
using System.Diagnostics;

[Group( "Walker" )]
[Title( "Walker - Player Controller" )]
public sealed class PlayerController : Component, IStats
{
	[Sync][Property] public int Level { get; set; }
	[Sync][Property] public int Experience { get; set; }
	[Sync][Property] public float HP { get; set; }
	[Sync][Property] public float MaxHP { get; set; }
	[Sync][Property] public int Mana { get; set; }
	[Sync][Property] public int MaxMana { get; set; }
	[Sync][Property] public float ManaRegen { get; set; }
	[Sync][Property] public int PhysicalPower { get; set; }
	[Sync][Property] public int MindPower { get; set; }
	[Sync][Property] public int Protection { get; set; }
	[Sync][Property] public int Fortitude { get; set; }
	[Sync][Property] public float MovementSpeed { get; set; }
	[Sync][Property] public float HitSpeed { get; set; }


	[Property] public CharacterController CharacterController { get; set; }
	[Property] public float CrouchMoveSpeed { get; set; } = 64.0f;
	[Property] public float WalkMoveSpeed { get; set; } = 190.0f;
	[Property] public float RunMoveSpeed { get; set; } = 190.0f;
	[Property] public float SprintMoveSpeed { get; set; } = 320.0f;
	[Property] public CameraComponent Cam;

	[Property] public CitizenAnimationHelper AnimationHelper { get; set; }

	[Sync] public bool Crouching { get; set; }
	[Sync] public Angles EyeAngles { get; set; }
	[Sync] public Vector3 WishVelocity { get; set; }

	public bool WishCrouch;
	public float EyeHeight = 64;

	public float DamageDelay = 0f;

	protected override void OnStart()
	{
		base.OnStart();
		HP = MaxHP;
		Mana = MaxMana;
	}
	protected override void OnUpdate()
	{
		if ( !IsProxy )
		{
			MouseInput();
			Transform.Rotation = new Angles( 0, EyeAngles.yaw, 0 );
		}

		UpdateAnimation();
	}

	protected override void OnFixedUpdate()
	{
		if ( IsProxy )
			return;

		CrouchingInput();
		MovementInput();
		UpdateDamageDelay();
	}



	private void MouseInput()
	{
		var e = EyeAngles;
		e += Input.AnalogLook;
		e.pitch = e.pitch.Clamp( -90, 90 );
		e.roll = 0.0f;
		EyeAngles = e;
	}

	float CurrentMoveSpeed
	{
		get
		{
			if ( Crouching ) return CrouchMoveSpeed;
			if ( Input.Down( "run" ) ) return SprintMoveSpeed;
			if ( Input.Down( "walk" ) ) return WalkMoveSpeed;

			return RunMoveSpeed;
		}
	}

	RealTimeSince lastGrounded;
	RealTimeSince lastUngrounded;
	RealTimeSince lastJump;

	float GetFriction()
	{
		if ( CharacterController.IsOnGround ) return 6.0f;

		// air friction
		return 0.2f;
	}

	private void MovementInput()
	{
		if ( CharacterController is null )
			return;

		var cc = CharacterController;

		Vector3 halfGravity = Scene.PhysicsWorld.Gravity * Time.Delta * 0.5f;

		WishVelocity = Input.AnalogMove;

		if ( lastGrounded < 0.2f && lastJump > 0.3f && Input.Pressed( "jump" ) )
		{
			lastJump = 0;
			cc.Punch( Vector3.Up * 300 );
		}

		if ( !WishVelocity.IsNearlyZero() )
		{
			WishVelocity = new Angles( 0, EyeAngles.yaw, 0 ).ToRotation() * WishVelocity;
			WishVelocity = WishVelocity.WithZ( 0 );
			WishVelocity = WishVelocity.ClampLength( 1 );
			WishVelocity *= CurrentMoveSpeed;

			if ( !cc.IsOnGround )
			{
				WishVelocity = WishVelocity.ClampLength( 50 );
			}
		}


		cc.ApplyFriction( GetFriction() );

		if ( cc.IsOnGround )
		{
			cc.Accelerate( WishVelocity );
			cc.Velocity = CharacterController.Velocity.WithZ( 0 );
		}
		else
		{
			cc.Velocity += halfGravity;
			cc.Accelerate( WishVelocity );

		}

		//
		// Don't walk through other players, let them push you out of the way
		//
		var pushVelocity = PlayerPusher.GetPushVector( Transform.Position + Vector3.Up * 40.0f, Scene, GameObject );
		if ( !pushVelocity.IsNearlyZero() )
		{
			var travelDot = cc.Velocity.Dot( pushVelocity.Normal );
			if ( travelDot < 0 )
			{
				cc.Velocity -= pushVelocity.Normal * travelDot * 0.6f;
			}

			cc.Velocity += pushVelocity * 128.0f;
		}

		cc.Move();

		if ( !cc.IsOnGround )
		{
			cc.Velocity += halfGravity;
		}
		else
		{
			cc.Velocity = cc.Velocity.WithZ( 0 );
		}

		if ( cc.IsOnGround )
		{
			lastGrounded = 0;
		}
		else
		{
			lastUngrounded = 0;
		}
	}
	float DuckHeight = (64 - 36);

	bool CanUncrouch()
	{
		if ( !Crouching ) return true;
		if ( lastUngrounded < 0.2f ) return false;

		var tr = CharacterController.TraceDirection( Vector3.Up * DuckHeight );
		return !tr.Hit; // hit nothing - we can!
	}

	public void CrouchingInput()
	{
		WishCrouch = Input.Down( "duck" );

		if ( WishCrouch == Crouching )
			return;

		// crouch
		if ( WishCrouch )
		{
			CharacterController.Height = 36;
			Crouching = WishCrouch;

			// if we're not on the ground, slide up our bbox so when we crouch
			// the bottom shrinks, instead of the top, which will mean we can reach
			// places by crouch jumping that we couldn't.
			if ( !CharacterController.IsOnGround )
			{
				CharacterController.MoveTo( Transform.Position += Vector3.Up * DuckHeight, false );
				Transform.ClearLerp();
				EyeHeight -= DuckHeight;
			}

			return;
		}

		// uncrouch
		if ( !WishCrouch )
		{
			if ( !CanUncrouch() ) return;

			CharacterController.Height = 64;
			Crouching = WishCrouch;
			return;
		}


	}

	private void UpdateCamera()	
	{
		//var camera = Scene.GetAllComponents<CameraComponent>().Where( x => x.IsMainCamera ).FirstOrDefault();
		//if ( camera is null ) return;

		var targetEyeHeight = Crouching ? 28 : 64;
		EyeHeight = EyeHeight.LerpTo( targetEyeHeight, RealTime.Delta * 10.0f );

		var targetCameraPos = Transform.Position + new Vector3( 0, 0, EyeHeight );

		// smooth view z, so when going up and down stairs or ducking, it's smooth af
		if ( lastUngrounded > 0.2f )
		{
			targetCameraPos.z = Cam.Transform.Position.z.LerpTo( targetCameraPos.z, RealTime.Delta * 25.0f );
		}

		Cam.Transform.Position = targetCameraPos;
		Cam.Transform.Rotation = EyeAngles;
		Cam.FieldOfView = Preferences.FieldOfView;
	}

	protected override void OnPreRender()
	{
		UpdateBodyVisibility();

		if ( IsProxy )
			return;
		
		
		UpdateCamera();
	}

	private void UpdateAnimation()
	{
		if ( AnimationHelper is null ) return;

		var wv = WishVelocity.Length;

		AnimationHelper.WithWishVelocity( WishVelocity );
		AnimationHelper.WithVelocity( CharacterController.Velocity );
		AnimationHelper.IsGrounded = CharacterController.IsOnGround;
		AnimationHelper.DuckLevel = Crouching ? 1.0f : 0.0f;

		AnimationHelper.MoveStyle = wv < 160f ? CitizenAnimationHelper.MoveStyles.Walk : CitizenAnimationHelper.MoveStyles.Run;

		var lookDir = EyeAngles.ToRotation().Forward * 1024;
		AnimationHelper.WithLook( lookDir, 1, 0.5f, 0.25f );
	}

	private void UpdateBodyVisibility()
	{
		if ( AnimationHelper is null )
			return;

		var renderMode = ModelRenderer.ShadowRenderType.On;
		if ( !IsProxy ) renderMode = ModelRenderer.ShadowRenderType.ShadowsOnly;

		AnimationHelper.Target.RenderType = renderMode;

		foreach ( var clothing in AnimationHelper.Target.Components.GetAll<ModelRenderer>( FindMode.InChildren ) )
		{
			if ( !clothing.Tags.Has( "clothing" ) )
				continue;

			clothing.RenderType = renderMode;
		}
	}

	private void UpdateDamageDelay()
	{
		if (DamageDelay > 0)
		{
			DamageDelay -= Time.Delta;
			//Log.Info($"{DamageDelay}");
		}
	}

	// This method is called twice for some reason. Don't ask why.
	[Broadcast]
	public void TakeDamage(Guid attackerGUID)
	{
		PlayerController attackerController = Scene.Directory.FindByGuid( attackerGUID ).Components.Get<PlayerController>();
		//Log.Info( "hi lol" );
		if ( !IsProxy /*attackerController.DamageDelay <= 0 */)
		{
			Log.Info( "The fireball hit its target!" );
			Log.Info( "Damage delay is set to 2." );
			attackerController.DamageDelay = 0.5f;
			Log.Info( $"Target HP before reduction: {HP}" );
			var playerDamage = attackerController.MindPower * 5;
			HP = MathF.Max( HP - playerDamage, 0f );
			HP -= playerDamage;
			Log.Info( $"Damage dealt: {playerDamage}" );
			Log.Info( $"Target REAL HP: {HP}" );

			if ( HP <= 0f )
			{
				Log.Info( $"Oh damn it! {GameObject} died! :<" );
				ActivateDeathState(attackerGUID);
			}
		}
	}

	[Broadcast]
	public void Debuff(Guid attackerGUID)
	{
		//Player debuff
		Log.Info( "Player debuff" );
	}

	public void ActivateDeathState(Guid attackerGUID)
	{
		/*Rigidbody rigidbody = GameObject.Children[0].Components.GetAll<Rigidbody>().FirstOrDefault();
		Log.Info( rigidbody );
		*/
		Rigidbody rigidbody = GameObject.Components.Create<Rigidbody>();
		PlayerController playerController = GameObject.Components.Get<PlayerController>();
		Log.Info( $"{GameObject.Name}" );
		if ( playerController != null )
		{
			GameObject.Children.FirstOrDefault<GameObject>().Components.Get<CitizenAnimationHelper>().Destroy();
			playerController.Destroy();
		}
	}
}
