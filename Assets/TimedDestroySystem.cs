using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

public class TimedDestroySystem1 : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dt = Time.DeltaTime;
        Entities.WithoutBurst().WithStructuralChanges()
            .ForEach((Entity entity, ref Lifetime lifetimeData) =>
            {
                lifetimeData.lifeLeft -= dt;
                if (lifetimeData.lifeLeft <= 0f)
                    EntityManager.DestroyEntity(entity);
            })
        .Run();

        Entities.WithoutBurst().WithStructuralChanges()
            .ForEach((Entity entity, ref Translation position, ref VirusData virusData) =>
            {
                if (!virusData.alive)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        float3 offset = (float3)UnityEngine.Random.insideUnitSphere * 2.0f;
                        var splat = ECSManager.manager.Instantiate(ECSManager.chunk);
                        float3 randomDir = new float3(UnityEngine.Random.Range(-1, 1),
                                                UnityEngine.Random.Range(-1, 1),
                                                UnityEngine.Random.Range(-1, 1));
                        ECSManager.manager.SetComponentData<Translation>(splat, new Translation { Value = position.Value + offset });
                        ECSManager.manager.SetComponentData<PhysicsVelocity>(splat, new PhysicsVelocity { Linear = randomDir * 2 });
                    }
                   
                    EntityManager.DestroyEntity(entity);
                }
            })
        .Run();

        return inputDeps;
    }
}