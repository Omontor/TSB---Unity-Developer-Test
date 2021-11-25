using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class TimedDestroySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dT = Time.DeltaTime;
        Entities
            .WithoutBurst().WithStructuralChanges()
            .ForEach((Entity entity,
                        ref Translation position,
                        ref Lifetime lifetime) =>
            {

                lifetime.lifeLeft -= dT;
                if (lifetime.lifeLeft <= 0f)
                {
                    EntityManager.DestroyEntity(entity);
                }

            }).Run();

         Entities
            .WithoutBurst().WithStructuralChanges()
            .ForEach((Entity entity,
                        ref Translation position,
                        ref Asteroid asteroid) =>
            {

                if (!asteroid.alive)
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
                    GameObject.Find("Explosion").GetComponent<AudioSource>().Play();
                }

            }).Run();         
        
        
        Entities
            .WithoutBurst().WithStructuralChanges()
            .ForEach((Entity entity,
                        ref Translation position,
                        ref Shoot shot) =>
            {

                if (!shot.alive)
                {
                    EntityManager.DestroyEntity(entity);
                }

            }).Run();

        return inputDeps;
    }
}
