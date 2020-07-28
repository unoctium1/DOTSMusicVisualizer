using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class NoiseFromMusicSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float amplitude = AudioSpectrum.Instance.AmplitudeBuffer;
        NativeArray<float> levels = new NativeArray<float>(AudioSpectrum.Instance.PeakLevels, Allocator.TempJob);

        Entities
            .WithName("Levels_NoiseFromMusic")
            .ForEach((ref VertexNoiseComponent noise, in LevelComponent band) =>
            {
                noise = GetNoiseValue(band.Value <= levels.Length ? levels[band.Value] : 0);
            }).WithDeallocateOnJobCompletion(levels)
            .ScheduleParallel();

        Entities
            .WithName("Amplitude_NoiseFromMusic")
            .WithAll<AmplitudeTagComponent>()
            .ForEach((ref VertexNoiseComponent noise) =>
            {
                noise = GetNoiseValue(amplitude);
            }).ScheduleParallel();
    }

    private static VertexNoiseComponent GetNoiseValue(float val)
    {
        return new VertexNoiseComponent
        {
            Value = math.lerp(0.3f, 2.0f, val)
        };
    }
}
