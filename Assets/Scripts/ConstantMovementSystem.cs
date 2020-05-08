using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class ConstantMovementSystem : SystemBase
{
    
    
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;

        Entities
            .WithName("ConstantMovementSystem")
            .ForEach((ref Translation t, in ConstantMovementComponent movement) =>
            {
                t.Value += movement.movement * dt;
            }).ScheduleParallel();
    }
}