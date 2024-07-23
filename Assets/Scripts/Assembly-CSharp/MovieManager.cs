using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x020001A3 RID: 419
public class MovieManager
{
	// Token: 0x06000DED RID: 3565 RVA: 0x00054AE0 File Offset: 0x00052CE0
	public MovieManager()
	{
		this.unlocked = new HashSet<int>();
		this.movies = new Dictionary<int, MovieInfo>();
		this.LoadMoviesFromSpread();
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x00054B1C File Offset: 0x00052D1C
	public MovieInfo GetMovieInfoById(int id)
	{
		return this.movies[id];
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x00054B2C File Offset: 0x00052D2C
	public void UnlockMovie(int id)
	{
		TFUtils.Assert(this.movies.ContainsKey(id), "Unlocking an unknown movie!");
		this.unlocked.Add(id);
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x00054B54 File Offset: 0x00052D54
	public HashSet<int> UnlockedMovies
	{
		get
		{
			return new HashSet<int>(this.unlocked);
		}
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x00054B64 File Offset: 0x00052D64
	public void UnlockAllMovies()
	{
		foreach (KeyValuePair<int, MovieInfo> keyValuePair in this.movies)
		{
			int key = keyValuePair.Key;
			if (!this.unlocked.Contains(key))
			{
				this.UnlockMovie(key);
			}
		}
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x00054BE4 File Offset: 0x00052DE4
	public void UnlockAllMoviesToGamestate(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey("movies"))
		{
			dictionary["movies"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["movies"];
		foreach (KeyValuePair<int, MovieInfo> keyValuePair in this.movies)
		{
			int key = keyValuePair.Key;
			if (!list.Contains(key))
			{
				list.Add(key);
			}
		}
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x00054CB0 File Offset: 0x00052EB0
	private string[] GetFilesToLoad()
	{
		return Config.MOVIE_PATH;
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x00054CB8 File Offset: 0x00052EB8
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x00054CBC File Offset: 0x00052EBC
	private void LoadMovies()
	{
		string[] filesToLoad = this.GetFilesToLoad();
		foreach (string filePath in filesToLoad)
		{
			string filePathFromString = this.GetFilePathFromString(filePath);
			TFUtils.DebugLog("Loading movie info: " + filePathFromString);
			string json = TFUtils.ReadAllText(filePathFromString);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
			string text = (string)dictionary["type"];
			if (text != null)
			{
				if (MovieManager.<>f__switch$mapE == null)
				{
					MovieManager.<>f__switch$mapE = new Dictionary<string, int>(1)
					{
						{
							"movie",
							0
						}
					};
				}
				int num;
				if (MovieManager.<>f__switch$mapE.TryGetValue(text, out num))
				{
					if (num == 0)
					{
						MovieInfo movieInfo = new MovieInfo(dictionary);
						this.movies.Add(movieInfo.Did, movieInfo);
					}
				}
			}
		}
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x00054D9C File Offset: 0x00052F9C
	private void LoadMoviesFromSpread()
	{
		string text = "Movies";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				dictionary.Clear();
				dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
				dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
				dictionary.Add("description", instance.GetStringCell(sheetIndex, rowIndex, "description"));
				dictionary.Add("collect_name", instance.GetStringCell(sheetIndex, rowIndex, "collect name"));
				dictionary.Add("movie", instance.GetStringCell(sheetIndex, rowIndex, "movie"));
				dictionary.Add("texture", instance.GetStringCell(sheetIndex, rowIndex, "texture"));
				MovieInfo movieInfo = new MovieInfo(dictionary);
				this.movies.Add(movieInfo.Did, movieInfo);
			}
		}
	}

	// Token: 0x04000938 RID: 2360
	private static readonly string MOVIE_PATH = "Video";

	// Token: 0x04000939 RID: 2361
	private HashSet<int> unlocked;

	// Token: 0x0400093A RID: 2362
	private Dictionary<int, MovieInfo> movies;
}
