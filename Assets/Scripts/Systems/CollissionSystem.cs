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
        public ComponentDataFromEntity<Health> healthGroup;
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

            if (id[entityA].isAsteroid == true && id[entityB].isPlayer == true)
            {

                var aliveComponent2 = healthGroup[entityB];
                aliveComponent2.ishit = true;
                healthGroup[entityB] = aliveComponent2;
            }

            if (id[entityA].isChunk == true && id[entityB].isPlayer == true)
            {


                var aliveComponent2 = healthGroup[entityB];
                aliveComponent2.ishit = true;
                healthGroup[entityB] = aliveComponent2;
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
            healthGroup = GetComponentDataFromEntity<Health>(),
        }.Schedule(m_StepPhysicsWorldSystem.Simulation, ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);

        jobHandle.Complete();
        return jobHandle;
    }

}
