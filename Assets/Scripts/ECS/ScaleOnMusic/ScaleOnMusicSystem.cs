using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public class ScaleOnMusicSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float amplitude = AudioSpectrum.Instance.AmplitudeBuffer;
        NativeArray<float> levels = new NativeArray<float>(AudioSpectrum.Instance.PeakLevels, Allocator.TempJob);

        Entities
            .WithName("Levels_NoiseFromMusic")
            .ForEach((ref ScaleValue noise, in LevelComponent band) =>
            {
                noise = GetScaleValue(band.Value <= levels.Length ? levels[band.Value] : 0);
            }).WithDeallocateOnJobCompletion(levels)
            .ScheduleParallel();

        Entities
            .WithName("Amplitude_NoiseFromMusic")
            .WithAll<AmplitudeTagComponent>()
            .ForEach((ref ScaleValue noise) =>
            {
                noise = GetScaleValue(amplitude);
            }).ScheduleParallel();
    }

    private static ScaleValue GetScaleValue(float val)
    {
        return new ScaleValue
        {
            Value = math.lerp(0.9f, 1.8f, val)
        };
    }
}
