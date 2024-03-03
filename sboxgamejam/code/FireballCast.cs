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
    protected override void OnUpdate()
    {
        if (!IsProxy)
        {
            if (Input.Pressed("Attack1"))
            {
				var netFireball = FireballPrefab.Clone( FireballSpawnPos.Transform.Position + Vector3.Forward, FireballSpawnPos.Parent.Transform.Rotation );
				//netFireball.Tags.Add( $"{FireballSpawnPos.Parent.Parent.Name} - Fireball" );
				netFireball.Name = $"{FireballSpawnPos.Parent.Parent.Name} - Fireball";
				netFireball.NetworkSpawn();
            }
        }
    }
}
