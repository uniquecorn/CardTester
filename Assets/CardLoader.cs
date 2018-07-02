using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CardLoader : MonoBehaviour
{
	public Card card;
	public int loadedIndex = 0;
	public CardHolder loadedData;
	public CardLoad cardLoadPrefab;

	public RectTransform cardLoaderContent;
	public RectTransform addCardButton;
	private List<CardLoad> cardLoaders;
	public static CardLoader instance;
	// Use this for initialization
	void Start ()
	{
		instance = this;
		Screen.SetResolution(720, 720, false);
		Load();
		cardLoaders = new List<CardLoad>();
		CreateCardLoaders();
		card.Load(loadedData.data[loadedIndex]);
	}
	public void CreateCardLoaders()
	{
		for(int i = cardLoaders.Count - 1; i >= 0; i--)
		{
			Destroy(cardLoaders[i].gameObject);
		}
		cardLoaders.Clear();
		for(int i = 0; i < loadedData.data.Count; i++)
		{
			cardLoaders.Add(Instantiate(cardLoadPrefab,cardLoaderContent));
			cardLoaders[i].Load(i);
			((RectTransform)cardLoaders[i].transform).anchoredPosition = (Vector2.down * 5) + (Vector2.down * 45 * i);
			addCardButton.anchoredPosition = (Vector2.down * 5) + (Vector2.down * 45 * (i+1));
			cardLoaderContent.sizeDelta = new Vector2(cardLoaderContent.sizeDelta.x,5 + ((i+2) * 45));
		}
	}
	public void AddCard()
	{
		loadedData.data.Add(new CardData());
		Save();
		Load();
		CreateCardLoaders();
	}
	public void Save()
	{
		string json = JsonUtility.ToJson(loadedData, true);
		File.WriteAllText(GetSavePath(), json);
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
				data = new List<CardData>
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
