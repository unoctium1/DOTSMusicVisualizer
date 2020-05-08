using Unity.Collections;
using Unity.Transforms;
using Unity.Entities;

[UpdateAfter(typeof(ConstantMovementSystem))]
public class ShiftTranslationWithPlayerSystem : SystemBase
{
    private EntityQuery m_LocalPlayer;

    protected override void OnCreate(){
        base.OnCreate();
        m_LocalPlayer = GetEntityQuery(  ComponentType.ReadOnly<Translation>() , ComponentType.ReadOnly<m_PlayerTag>()); 
    }

    protected override void OnUpdate()
    {
        NativeArray<Translation> target = m_LocalPlayer.ToComponentDataArray<Translation>(Allocator.TempJob);

        Entities
            .WithName("ShiftWithPlayer")
            .ForEach((ref Translation t, ref ShiftTranslationWithPlayer shift) =>
            {
                if(target.Length == 0)
                    return;

                if((target[0].Value.z - t.Value.z) > shift.shiftOffset){
                    t.Value += shift.offset;
                }
            })
            .WithDeallocateOnJobCompletion(target)
            .ScheduleParallel();
    }
}
