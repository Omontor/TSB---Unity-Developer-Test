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
        public ComponentDataFromEntity<Shoot> shotGroup;
        public ComponentDataFromEntity<Asteroid> asteroridGroup;
        public ComponentDataFromEntity<ObjectID> id;

        public void Execute(CollisionEvent collisionEvent)
        {

    
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            if (id[entityA].isAsteroid == true && id[entityB].isShot == true)
            {
                
                var aliveComponent = asteroridGroup[entityA];
                aliveComponent.alive = false;
                asteroridGroup[entityA] = aliveComponent;

                var aliveComponent2 = shotGroup[entityB];
                aliveComponent2.alive = false;
                shotGroup[entityB] = aliveComponent2;

            }


        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle jobHandle = new CollisionEventImpulseJob
        {
            shotGroup = GetComponentDataFromEntity<Shoot>(),
            asteroridGroup = GetComponentDataFromEntity<Asteroid>(),
            id = GetComponentDataFromEntity<ObjectID>(),
        }.Schedule(m_StepPhysicsWorldSystem.Simulation, ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);

        jobHandle.Complete();
        return jobHandle;
    }

}
