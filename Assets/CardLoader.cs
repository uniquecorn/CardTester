using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CardLoader : MonoBehaviour
{
	public Card card;
	public int loadedIndex = 0;
	public CardHolder loadedData;
	// Use this for initialization
	void Start ()
	{
		Screen.SetResolution(720, 720, false);
		Load();
		card.Load(loadedData.data[loadedIndex]);
	}
	public void Load()
	{
		if (File.Exists(GetSavePath()))
		{
			string json = File.ReadAllText(GetSavePath());
			loadedData = JsonUtility.FromJson<CardHolder>(json);
			
		}
		else
		{
			CardHolder tempData = new CardHolder()
			{
				data = new CardData[]
				{
					new CardData()
				}
			};
			string json = JsonUtility.ToJson(tempData, true);
			File.WriteAllText(GetSavePath(), json);
			print(json);
		}
	}
	public static string GetSavePath(string name = "data.json")
	{
#if UNITY_EDITOR
		return Path.Combine(Application.persistentDataPath, name);
#else
		return Path.Combine(Application.dataPath,name);
#endif
	}
	// Update is called once per frame
	void Update ()
	{
		Castle.CastleManager.CastleUpdate();
		if(Input.GetKeyDown("r"))
		{
			Load();
			card.Load(loadedData.data[loadedIndex]);
		}
		// if(Input.GetAxis("Mouse ScrollWheel") > 0)
		// {
		// 	Load();
		// 	loadedIndex++;
		// 	if(loadedIndex >= loadedData.data.Length)
		// 	{
		// 		loadedIndex = 0;
		// 	}
		// 	card.Load(loadedData.data[loadedIndex]);
		// }
		// else if(Input.GetAxis("Mouse ScrollWheel") < 0)
		// {
		// 	Load();
		// 	loadedIndex--;
		// 	if(loadedIndex < 0)
		// 	{
		// 		loadedIndex = loadedData.data.Length - 1;
		// 	}
		// 	card.Load(loadedData.data[loadedIndex]);
		// }
	}
}
