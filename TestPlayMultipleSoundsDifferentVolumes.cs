using System;
using System.Threading;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

class Program
{
    static void Main()
    {
        Console.WriteLine("Playing Sounds!");

        Thread thread1 = new Thread(() => PlaySound("1.mp3", 1.0f, false));
        Thread thread2 = new Thread(() => PlaySound("2.mp3", 0.0f, true));

        thread1.Start();
        thread2.Start();
    }

    private static float changeVolumeSpeedFactor = 0.001f;

    static void PlaySound(string path, float initialVolume, bool increaseVolume)
    {
        using (var reader = new AudioFileReader(path))
        {
            var volumeProvider = new VolumeSampleProvider(reader) { Volume = initialVolume };
            using (var output = new WaveOutEvent())
            {
                output.Init(volumeProvider);
                output.Play();

                float currentVolume = initialVolume;

                // Adjust volume in a separate loop without affecting others
                while (output.PlaybackState == PlaybackState.Playing)
                {
                    currentVolume = increaseVolume 
                        ? Math.Min(1.0f, currentVolume + changeVolumeSpeedFactor) 
                        : Math.Max(0.0f, currentVolume - changeVolumeSpeedFactor);

                    volumeProvider.Volume = currentVolume;

                    if (currentVolume >= 1.0f) increaseVolume = false;
                    else if (currentVolume <= 0.0f) increaseVolume = true;

                    Console.WriteLine($"Current volume for {path}: {currentVolume}");
                    Thread.Sleep(10); // Avoid excessive CPU usage
                }
            }
        }
    }
}
