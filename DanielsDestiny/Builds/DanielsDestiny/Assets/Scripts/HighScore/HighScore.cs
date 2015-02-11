using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;  

public class HighScore : MonoBehaviour {
	public static HighScore instance;
	int Days = 0, Hours = 0, Minutes = 0, Seconds = 0;

	int[] SurviveTime = new int[5];
	string path = "Assets/Files/HighScore.xml";
	// Use this for initialization

	public SaveHighscore save;
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		if(instance == null)
			instance = this as HighScore;
		else
			Destroy(this.gameObject);
	}
	public void SaveScore(int SecondsPassedBy)
	{
		Load();

		Minutes = SecondsPassedBy / 60;
		Seconds = SecondsPassedBy - (Minutes*60);
		Hours = Minutes/60;
		Minutes = Minutes - (Hours*60);
		Days = Hours/24;
		Hours = Hours - (Days*24);

		int[] tmpInt = new int[5];

		tmpInt[0] = Days;
		tmpInt[1] = Hours;
		tmpInt[2] = Minutes;
		tmpInt[3] = Seconds;
		tmpInt[4] = SecondsPassedBy;

		SurviveTime = tmpInt;

		if(save.TimeSurvived != null){
			if(save.TimeSurvived.Length == 10)
			{
				for(int i = 0; i < save.TimeSurvived.Length; i++)
				{
					if(SurviveTime[4] > save.TimeSurvived[i][4])
					{
						for(int x = 0; x < 5; x++)
						{
							save.TimeSurvived[9][x] = SurviveTime[x];
						}
					}
				}
			}
			else
			{
				int[][] tmpintarr = new int[save.TimeSurvived.Length+1][];
				for (int k = 0; k < save.TimeSurvived.Length+1; k++)
				{
					if(k<save.TimeSurvived.Length)
						tmpintarr[k] = save.TimeSurvived[k];
					else
						tmpintarr[k] = new int[5];
				}

				for (int d = 0; d < 5; d++)
				{
					tmpintarr[save.TimeSurvived.Length][d] = SurviveTime[d];
				}

				save.TimeSurvived = tmpintarr;

			}
		}
		else
		{
			//write this:
			int[][] arr = new int[1][];
			for (int a = 0; a < 1; a++)
			{
				arr[a] = new int[5];
			}
			
			for (int b = 0; b < 1; b++)
			{
				for (int j = 0; j < 5; j++)
				{
					arr[b][j] = SurviveTime[j];
				}
			}
			save.TimeSurvived = arr;
		}
		SortSave();
	}
	void SortSave()
	{
		int s = save.TimeSurvived.Length-1;
		int tmp = 999999999;
		int savedX = 0;
		List<int> takenInts = new List<int>();
		int[][] tmpHigh = new int[save.TimeSurvived.Length][];
		
		for (int c = 0; c < save.TimeSurvived.Length; c++)
		{
			tmpHigh[c] = new int[5];
		}
		
		for(int x = 0; x < save.TimeSurvived.Length; x++)
		{
			for(int y = 0; y < save.TimeSurvived.Length; y++)
			{
				if(takenInts.Count > 0)
				{
					int used = 0;
					for(int t = 0; t < takenInts.Count; t++)
					{
						if(y == takenInts[t])
						{
							used++;
						}
					}
					if(used == 0)
					{
						if(save.TimeSurvived[y][4] < tmp)
						{
							tmp = save.TimeSurvived[y][4];
							savedX = y;
						}
					}
				}
				else
				{
					if(save.TimeSurvived[y][4] < tmp)
					{
						tmp = save.TimeSurvived[y][4];
						savedX = y;
					}
				}
			}
			takenInts.Add(savedX);
			for(int a = 0; a < 5; a++)
			{
				tmpHigh[s][a] = save.TimeSurvived[savedX][a];
			}
			s--;
			tmp = 999999999;
		}
		save.TimeSurvived = tmpHigh;
		Save();
	}
	void Save()
	{
		StreamWriter sw = new StreamWriter(path);
		XmlSerializer x = new System.Xml.Serialization.XmlSerializer(save.GetType());
		x.Serialize(sw, save);
		sw.Close();
	}
	void Load()
	{
		if (File.Exists(path))
		{
			StreamReader sr = new StreamReader(path);
			XmlSerializer x = new System.Xml.Serialization.XmlSerializer(save.GetType());
			save = x.Deserialize(sr) as SaveHighscore;
			sr.Close();
		}
	}
	public int[][] GetScore()
	{
		Load();
		return save.TimeSurvived;
	}
}
[System.Serializable]
public class SaveHighscore
{
	public int[][] TimeSurvived;
}
