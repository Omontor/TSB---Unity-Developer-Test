using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class CollissionSystem : JobComponentSystem
{
    BuildPhysicsWorld m_BuildPhysicsWorldSystem;
    StepPhysicsWorld m_StepPhysicsWorldSystem;

    protected override void OnCreate()
    {
        m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    struct CollisionEventImpulseJob: ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<Shoot> shotGroup;
        public ComponentDataFromEntity<Asteroid> asteroridGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isTargetA = asteroridGroup.HasComponent(entityA);
            bool istargetB = asteroridGroup.HasComponent(entityB);

            bool isbulletA = shotGroup.HasComponent(entityA);
            bool isbulletB = shotGroup.HasComponent(entityB);

            if (isbulletA && istargetB)
            {
                var aliveComponent = asteroridGroup[entityB];
                aliveComponent.alive = false;
                asteroridGroup[entityB] = aliveComponent;
            }           
            
            if (isbulletB && isTargetA)
            {
                var aliveComponent = asteroridGroup[entityA];
                aliveComponent.alive = false;
                asteroridGroup[entityA] = aliveComponent;
            }

        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle jobHandle = new CollisionEventImpulseJob
        {
            shotGroup = GetComponentDataFromEntity<Shoot>(),
            asteroridGroup = GetComponentDataFromEntity<Asteroid>()
        }.Schedule(m_StepPhysicsWorldSystem.Simulation, ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);

        jobHandle.Complete();
        return jobHandle;
    }

}
