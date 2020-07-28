using Unity.Collections;
using Unity.Entities;

public class EmmisionFromMusicSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float amplitude = AudioSpectrum.Instance.AmplitudeBuffer;
        NativeArray<float> levels = new NativeArray<float>(AudioSpectrum.Instance.Levels, Allocator.TempJob);

        Entities
            .WithName("Levels_EmmissionFromMusic")
            .ForEach((ref EmissionComponent emission, in LevelComponent band) =>
            {
                emission = GetEmissionValue(band.Value <= levels.Length ? levels[band.Value] : 0);
            }).WithDeallocateOnJobCompletion(levels)
            .ScheduleParallel();

        Entities
            .WithName("Amplitude_EmmissionFromMusic")
            .WithAll<AmplitudeTagComponent>()
            .ForEach((ref EmissionComponent emission) =>
            {
                emission = GetEmissionValue(amplitude);
            }).ScheduleParallel();
    }

    private static EmissionComponent GetEmissionValue(float val)
    {
        return new EmissionComponent
        {
            Value = val
        };
    }
}
