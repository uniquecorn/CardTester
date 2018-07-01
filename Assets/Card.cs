using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Castle;
using System.IO;

public class Card : CastleObject
{
	public CardData data;
	public RectTransform artBacking;
	public RectTransform leftTabs;
	public RectTransform rightTabs;
	public ToggleGroup tabs;
	public Toggle leftTabObj;
	public Toggle rightTabObj;
	public List<RectTransform> leftTabToggles;
	public List<RectTransform> rightTabToggles;
	public Image maskImage;
	public RawImage art;
	public List<RawImage> backgroundLayers;
	public List<RawImage> foregroundLayers;
	public Text nameText;
	public float backgroundScale, foregroundScale;
	public bool isHeld;

	public Button uploadArt;

	public int artIndex = -1;

	public DirectoryViewer dirViewer;
	// Use this for initialization
	public void Load (CardData _data)
	{
		artIndex = 0;
		data = _data;
		nameText.text = _data.name;
		if(backgroundLayers != null)
		{
			for (int i = backgroundLayers.Count - 1; i >= 0; i--)
			{
				Destroy(backgroundLayers[i].gameObject);
			}
			backgroundLayers.Clear();
		}
		if(foregroundLayers != null)
		{
			for (int i = foregroundLayers.Count - 1; i >= 0; i--)
			{
				Destroy(foregroundLayers[i].gameObject);
			}
			foregroundLayers.Clear();
		}
		backgroundScale = _data.backgroundScale;
		foregroundScale = _data.foregroundScale;
		if (_data.mainArt.imageDir != "" && !string.IsNullOrEmpty(_data.mainArt.imageDir))
		{
			art.texture = CastleTools.LoadImage(Path.Combine(CardLoader.GetSavePath(""), _data.mainArt.imageDir));
			art.SetNativeSize();
			art.rectTransform.sizeDelta = art.rectTransform.sizeDelta / 2;
			if (!_data.mainArt.masked)
			{
				art.maskable = false;
			}
			else
			{
				art.maskable = true;
			}
			float offsetX = art.rectTransform.sizeDelta.x - 96;
			float offsetY = art.rectTransform.sizeDelta.y - 126;
			float maskWidth = art.rectTransform.sizeDelta.x;
			float maskHeight = art.rectTransform.sizeDelta.y;
			float maskOffsetX = 0;
			float maskOffsetY = 0;
			if (_data.maskLeft)
			{
				maskWidth -= offsetX / 2;
				maskOffsetX += offsetX / 4;
			}
			if(_data.maskRight)
			{
				maskWidth -= offsetX / 2;
				maskOffsetX -= offsetX / 4;
			}
			if(_data.maskTop)
			{
				maskHeight -= offsetY / 2;
				maskOffsetY -= offsetY / 4;
			}
			if(_data.maskBottom)
			{
				maskHeight -= offsetY / 2;
				maskOffsetY += offsetY / 4;
			}
			maskImage.rectTransform.sizeDelta = new Vector2(maskWidth, maskHeight);
			maskImage.rectTransform.anchoredPosition = new Vector2(maskOffsetX, maskOffsetY);
			art.transform.position = artBacking.transform.position;
		}
		// if (_data.backgroundLayers != null)
		// {
		// 	backgroundLayers = new List<RawImage>();
		// 	for (int i = 0; i < _data.backgroundLayers.Length; i++)
		// 	{
				
		// 		backgroundLayers.Add(Instantiate(art, maskImage.rectTransform));
		// 		if (!_data.backgroundLayers[i].masked)
		// 		{
		// 			backgroundLayers[i].maskable = false;
		// 		}
		// 		else
		// 		{
		// 			backgroundLayers[i].maskable = true;
		// 		}
		// 		backgroundLayers[i].rectTransform.SetAsFirstSibling();
		// 		backgroundLayers[i].texture = CastleTools.LoadImage(Path.Combine(CardLoader.GetSavePath(""), _data.backgroundLayers[i].imageDir));
		// 		backgroundLayers[i].SetNativeSize();
		// 		backgroundLayers[i].rectTransform.sizeDelta = backgroundLayers[i].rectTransform.sizeDelta / 2;
		// 		backgroundLayers[i].transform.position = artBacking.transform.position;
		// 	}
		// }
		// if (_data.foregroundLayers != null)
		// {
		// 	foregroundLayers = new List<RawImage>();
		// 	for (int i = 0; i < _data.foregroundLayers.Length; i++)
		// 	{
		// 		foregroundLayers.Add(Instantiate(art, maskImage.rectTransform));
		// 		if (!_data.foregroundLayers[i].masked)
		// 		{
		// 			foregroundLayers[i].maskable = false;
		// 		}
		// 		else
		// 		{
		// 			foregroundLayers[i].maskable = true;
		// 		}
		// 		foregroundLayers[i].rectTransform.SetAsLastSibling();
		// 		foregroundLayers[i].texture = CastleTools.LoadImage(Path.Combine(CardLoader.GetSavePath(""), _data.foregroundLayers[i].imageDir));
		// 		foregroundLayers[i].SetNativeSize();
		// 		foregroundLayers[i].rectTransform.sizeDelta = foregroundLayers[i].rectTransform.sizeDelta / 2;
		// 		foregroundLayers[i].transform.position = artBacking.transform.position;
				
		// 	}
		// }
		SelectTab();
		CreateTabs();
	}

	public void CreateTabs()
	{
		if(data.backgroundLayers != null)
		{
			backgroundLayers = new List<RawImage>();
			for(int i = 0; i < data.backgroundLayers.Count; i++)
			{
				AddBackground(data.backgroundLayers[i].imageDir);
			}
		}
		if(data.foregroundLayers != null)
		{
			foregroundLayers = new List<RawImage>();
			for(int i = 0; i < data.foregroundLayers.Count; i++)
			{
				AddForeground(data.foregroundLayers[i].imageDir);
			}
		}
	}

	public void SelectTab()
	{
		if(artIndex == 0)
		{
			for(int i = 0; i < backgroundLayers.Count; i++)
			{
				backgroundLayers[i].gameObject.SetActive(true);
			}
			for(int i = 0; i < foregroundLayers.Count; i++)
			{
				foregroundLayers[i].gameObject.SetActive(true);
			}
			art.gameObject.SetActive(true);
			uploadArt.gameObject.SetActive(false);
			return;
		}
		else if(artIndex == 1)
		{
			for (int i = 0; i < backgroundLayers.Count; i++)
			{
				backgroundLayers[i].gameObject.SetActive(false);
			}
			for (int i = 0; i < foregroundLayers.Count; i++)
			{
				foregroundLayers[i].gameObject.SetActive(false);
			}
			art.gameObject.SetActive(true);
		}
		else if(artIndex > 1)
		{
			for (int i = 0; i < foregroundLayers.Count; i++)
			{
				if(i == (artIndex - 2))
				{
					foregroundLayers[i].gameObject.SetActive(true);
				}
				else
				{
					foregroundLayers[i].gameObject.SetActive(false);
				}
			}
		}
		else if(artIndex < 0)
		{
			for (int i = 0; i < backgroundLayers.Count; i++)
			{
				if (i == (Mathf.Abs(artIndex) - 1))
				{
					backgroundLayers[i].gameObject.SetActive(true);
				}
				else
				{
					backgroundLayers[i].gameObject.SetActive(false);
				}
			}
		}
		uploadArt.gameObject.SetActive(true);
	}

	public void AddForeground(string path = "")
	{
		ArtTab artTab = Instantiate(leftTabObj,leftTabs).GetComponent<ArtTab>();
		artTab.artIndex = foregroundLayers.Count + 2;
		artTab.indexText.text = artTab.artIndex.ToString();
		((RectTransform)artTab.transform).anchoredPosition = Vector2.down * 20 * (foregroundLayers.Count + 1);

		foregroundLayers.Add(Instantiate(art, maskImage.rectTransform));
		if(string.IsNullOrEmpty(path))
		{

		}
		else
		{

		}
	}

	public void AddBackground(string path = "")
	{
		ArtTab artTab = Instantiate(leftTabObj,leftTabs).GetComponent<ArtTab>();
		artTab.artIndex = backgroundLayers.Count - 1;
		artTab.indexText.text = artTab.artIndex.ToString();
		((RectTransform)artTab.transform).anchoredPosition = Vector2.up * 20 * (backgroundLayers.Count + 1);

		backgroundLayers.Add(Instantiate(art,maskImage.rectTransform));
		if (string.IsNullOrEmpty(path))
		{

		}
	}

	public void LoadNewArt()
	{
		StartCoroutine(LoadArt());
	}

	public IEnumerator LoadArt()
	{
		coll.enabled = false;
		string loadedArtPath = "";
		yield return dirViewer.StartCoroutine(dirViewer.SearchForFile(CardLoader.GetSavePath(""), result => loadedArtPath =result));
		if(artIndex == 0)
		{
			art.texture = CastleTools.LoadImage(loadedArtPath);
			art.SetNativeSize();
		}
		coll.enabled = true;
	}

	public override void Tap()
	{
		base.Tap();
		isHeld = true;
	}
	public override void Release()
	{
		base.Release();
		isHeld = false;
	}
	// Update is called once per frame
	void Update ()
	{
		if (isHeld)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.6f, Time.deltaTime * 15);
			if (backgroundLayers != null)
			{
				for (int i = 0; i < backgroundLayers.Count; i++)
				{
					backgroundLayers[i].transform.localScale = Vector3.Lerp(backgroundLayers[i].transform.localScale, Vector3.one * (1 - (backgroundScale * ((float)(i + 1) / backgroundLayers.Count))), Time.deltaTime * 15);
				}
			}
			if (foregroundLayers != null)
			{
				for (int i = 0; i < foregroundLayers.Count; i++)
				{
					foregroundLayers[i].transform.localScale = Vector3.Lerp(foregroundLayers[i].transform.localScale, Vector3.one * (1 + (foregroundScale * ((float)(i + 1) / foregroundLayers.Count))), Time.deltaTime * 15);
				}
			}
		}
		else
		{
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.5f, Time.deltaTime * 15);
			if (backgroundLayers != null)
			{
				for (int i = 0; i < backgroundLayers.Count; i++)
				{
					backgroundLayers[i].transform.localScale = Vector3.Lerp(backgroundLayers[i].transform.localScale, Vector3.one, Time.deltaTime * 15);
				}
			}
			if (foregroundLayers != null)
			{
				for (int i = 0; i < foregroundLayers.Count; i++)
				{
					foregroundLayers[i].transform.localScale = Vector3.Lerp(foregroundLayers[i].transform.localScale, Vector3.one, Time.deltaTime * 15);
				}
			}
		}
	}
}
