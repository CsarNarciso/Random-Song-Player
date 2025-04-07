// using System;
// using System.Threading;
// using NAudio.Wave;
// using NAudio.Wave.SampleProviders;

// class Program
// {
    // static void Main()
    // {
        // Thread thread1 = new Thread(() => PlaySound("1.mp3", 1.0f));
        // Thread thread2 = new Thread(() => PlaySound("2.mp3", 0.0f));

        // thread1.Start();
        // thread2.Start();
    // }

    // static void PlaySound(string path, float initialVolume)
    // {
        // using (var reader = new AudioFileReader(path))
        // {
            // var volumeProvider = new VolumeSampleProvider(reader) { Volume = initialVolume };
            // using (var output = new WaveOutEvent())
            // {
                // output.Init(volumeProvider);
                // output.Play();

                // while (output.PlaybackState == PlaybackState.Playing)
                // {
                    // Thread.Sleep(10); // Avoid excessive CPU usage
                // }
            // }
        // }
    // }
// }
