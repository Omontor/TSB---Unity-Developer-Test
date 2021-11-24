using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;

public class ShootSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dT = Time.DeltaTime;
        var jobHandle = Entities
            .WithName("ShootSystem")
            .ForEach((ref PhysicsVelocity physics,
                        ref Translation position,
                        ref Rotation rotation,
                        ref Shoot shoot) =>
            {
                physics.Angular = float3.zero;
                physics.Linear += dT * shoot.speed * math.forward(rotation.Value);

            }).Schedule(inputDeps);

        jobHandle.Complete();
        return jobHandle;
    }
}
