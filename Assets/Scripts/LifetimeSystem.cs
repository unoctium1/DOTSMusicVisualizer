using Unity.Entities;

[GenerateAuthoringComponent]
public struct LifetimeComponent : IComponentData
{
    public float Value;
}

public class LifetimeSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_Barrier;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;
        var ecb = m_Barrier.CreateCommandBuffer().ToConcurrent();

        Entities
            .WithName("LifetimeSystem")
            .ForEach((Entity e, int entityInQueryIndex, ref LifetimeComponent lifetime) => 
            {
                lifetime.Value -= dt;

                if(lifetime.Value <= 0){
                    ecb.DestroyEntity(entityInQueryIndex, e);
                }
            }).ScheduleParallel();
        
        m_Barrier.AddJobHandleForProducer(Dependency);
    }
}
