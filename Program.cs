using System;
using System.Threading;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

class Test
{

	private static int foundSongs = 0;
	private static bool increaseVolume = false;
	private static float volume = 1.5f;
	private static float changeVolumeSpeedFactor = 0.00005f;
	private static Random rnd = new Random();
	private static Queue<string> lastPlayedSongs = new Queue<string>();
	
	static void Main()
	{
		while(true)
		{
			string song = getRandomSongFilePath(Directory.GetCurrentDirectory());
			
			if(song == null)
			{
				Console.WriteLine("No files found in this directory!");
				Thread.Sleep(4000);
				break;
			}
			playSound(song);
		}
	}
	
	static void playSound(string path)
	{
		try
		{
			using (var reader = new AudioFileReader(path))
			using (var waveOut = new WaveOutEvent())
			{
				var resampler = new WdlResamplingSampleProvider(reader.ToSampleProvider(), reader.WaveFormat.SampleRate);
				
				// Set playback
				waveOut.Init(resampler);
				
				//Play sound
				waveOut.Play();
				
				//And wait to finish
				while(waveOut.PlaybackState == PlaybackState.Playing)
				{
					//Change volume
					volume = (increaseVolume) ? Math.Min(1.0f, volume + changeVolumeSpeedFactor) : Math.Max(0.0f, volume - changeVolumeSpeedFactor);
					waveOut.Volume = Math.Min(1.0f, volume);
					
					if(volume >= 1.0f)
					{
						increaseVolume = false;
					}
					else if(volume <= 0.0f)
					{
						increaseVolume = true;
						waveOut.Stop();
					}
					
					Console.Clear();
					Console.WriteLine($"Found {foundSongs} songs");
					Console.WriteLine($"Playing {path}");
					Console.WriteLine($"Enjoy!!");
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error: " + ex.Message);
		}
	}
	
	static string getRandomSongFilePath(string dirPath)
	{
		if(!Directory.Exists(dirPath))
			return null;
		
		string[] files = Directory.GetFiles(dirPath, "*.*")
							.Where(f => f.EndsWith(".mp3") || f.EndsWith(".wav"))
							.ToArray();
		
		if(files.Length == 0)
			return null;
		
		foundSongs = files.Length;
		
		if(files.Length != 1)
		{
			string selectedSong;
			
			do
			{
				selectedSong = files[rnd.Next(files.Length)];
			}
			while(lastPlayedSongs.Contains(selectedSong));
			
			
			lastPlayedSongs.Enqueue(selectedSong);
			
			if(lastPlayedSongs.Count >= files.Length / 2)
			{
				lastPlayedSongs.Dequeue();
			}
			
			return selectedSong;
		}
		
		return files[0];
	}
}