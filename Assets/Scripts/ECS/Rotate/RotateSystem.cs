using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct RotateSpeed : IComponentData
{
    public float Value;
}

public class RotateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;

        Entities
            .WithName("RotationSystem")
            .ForEach(
                (ref Rotation rot, in RotateSpeed speed) =>
                {
                    rot = new Rotation
                    {
                        Value = math.mul(math.normalize(rot.Value),
                        quaternion.AxisAngle(math.up(), speed.Value * dt))
                    };
                }).ScheduleParallel();
    }
}