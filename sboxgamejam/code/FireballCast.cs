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

	private bool canShoot = false;

    protected override void OnAwake()
    {
        base.OnAwake();
        // fireballspawnPos = GameObject.Scene.Directory.FindByName("Camera");


		if(!IsProxy)
		{
			canShoot = true;
		}

    }

    protected override void OnUpdate()
    {
        if (canShoot)
        {
            if (Input.Pressed("Attack1"))
            {
                var netFireball = FireballPrefab.Clone(FireballSpawnPos.Transform.Position + Vector3.Forward, FireballSpawnPos.Transform.Rotation);
                netFireball.NetworkSpawn();
            }
        }
        else
        {
            Log.Info("IsProxy didn't work.");
        }
    }
}
