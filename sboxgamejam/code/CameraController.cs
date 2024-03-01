using Sandbox;

public sealed class CameraController : Component
{
	protected override void OnStart()
	{
		base.OnStart();
		if ( !IsProxy )
		{
			Scene.Camera.Enabled = false;
			//GameObject.Network.SetOwnerTransfer( OwnerTransfer.Takeover );
			Log.Info( $"Owner is {Network.OwnerId}" );
		}
	}
	protected override void OnUpdate()
	{

	}
}
