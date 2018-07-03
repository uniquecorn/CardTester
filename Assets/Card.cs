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
	public List<RectTransform> backgroundTabs;
	public List<RectTransform> foregroundTabs;
	public Image maskImage;
	public RawImage art;
	public List<RawImage> backgroundLayers;
	public List<RawImage> foregroundLayers;
	public InputField nameText;
	public float backgroundScale, foregroundScale;
	public bool isHeld;
	public Button addBackground, addForeground;
	public Toggle maskL, maskR, maskT, maskB, maskedToggle;

	public Button uploadArt;
	public Slider foregroundSlider, backgroundSlider;
	public Text foregroundSliderText, backgroundSliderText;

	public int artIndex = -1;

	public DirectoryViewer dirViewer;
	// Use this for initialization
	public void Load (CardData _data)
	{
		tabs.SetAllTogglesOff();
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
			art.transform.position = artBacking.transform.position;
		}
		backgroundSlider.value = backgroundScale;
		foregroundSlider.value = foregroundScale;
		AdjustBackground(backgroundScale);
		AdjustForeground(foregroundScale);
		SelectTab();
		CreateTabs();
		ApplyMask();
	}

	public void Save()
	{
		CardLoader.instance.loadedData.data[CardLoader.instance.loadedCard] = data;
		CardLoader.instance.Save();
	}

	public void MaskGraphic(bool isOn)
	{
		if(artIndex == 1)
		{
			art.maskable = isOn;
			data.mainArt.masked = isOn;
		}
		else if(artIndex < 0)
		{
			backgroundLayers[Mathf.Abs(artIndex) - 1].maskable = isOn;
			data.backgroundLayers[Mathf.Abs(artIndex) - 1].masked = isOn;
		}
		else if (artIndex > 0)
		{
			foregroundLayers[artIndex - 2].maskable = isOn;
			data.foregroundLayers[artIndex - 2].masked = isOn;
		}
		Save();
	}

	public void MaskTop(bool isOn)
	{
		data.maskTop = isOn;
		ApplyMask();
		Save();
	}
	public void MaskLeft(bool isOn)
	{
		data.maskLeft = isOn;
		ApplyMask();
		Save();
	}
	public void MaskRight(bool isOn)
	{
		data.maskRight = isOn;
		ApplyMask();
		Save();
	}
	public void MaskBottom(bool isOn)
	{
		data.maskBottom = isOn;
		ApplyMask();
		Save();
	}

	public void ApplyMask()
	{
		float offsetX = art.rectTransform.sizeDelta.x - 96;
		float offsetY = art.rectTransform.sizeDelta.y - 126;
		float maskWidth = art.rectTransform.sizeDelta.x;
		float maskHeight = art.rectTransform.sizeDelta.y;
		float maskOffsetX = 0;
		float maskOffsetY = 0;

		maskL.isOn = data.maskLeft;
		maskR.isOn = data.maskRight;
		maskT.isOn = data.maskTop;
		maskB.isOn = data.maskBottom;

		if (data.maskLeft)
		{
			maskWidth -= offsetX / 2;
			maskOffsetX += offsetX / 4;
		}
		if (data.maskRight)
		{
			maskWidth -= offsetX / 2;
			maskOffsetX -= offsetX / 4;
		}
		if (data.maskTop)
		{
			maskHeight -= offsetY / 2;
			maskOffsetY -= offsetY / 4;
		}
		if (data.maskBottom)
		{
			maskHeight -= offsetY / 2;
			maskOffsetY += offsetY / 4;
		}
		maskImage.rectTransform.sizeDelta = new Vector2(maskWidth, maskHeight);
		maskImage.rectTransform.anchoredPosition = new Vector2(maskOffsetX, maskOffsetY);
		art.transform.position = artBacking.transform.position;
		for(int i = 0; i < backgroundLayers.Count; i++)
		{
			backgroundLayers[i].transform.position = artBacking.transform.position;
		}
		for (int i = 0; i < foregroundLayers.Count; i++)
		{
			foregroundLayers[i].transform.position = artBacking.transform.position;
		}
	}

	public void CreateTabs()
	{
		if(backgroundTabs != null)
		{
			for(int i = 0; i < backgroundTabs.Count; i++)
			{
				backgroundTabs[i].gameObject.SetActive(false);
			}
		}
		if (foregroundTabs != null)
		{
			for (int i = 0; i < foregroundTabs.Count; i++)
			{
				foregroundTabs[i].gameObject.SetActive(false);
			}
		}
		if (data.backgroundLayers != null)
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
			maskedToggle.isOn = data.mainArt.masked;
		}
		else if(artIndex > 1)
		{
			for (int i = 0; i < backgroundLayers.Count; i++)
			{
				backgroundLayers[i].gameObject.SetActive(false);
			}
			for (int i = 0; i < foregroundLayers.Count; i++)
			{
				if(i == (artIndex - 2))
				{
					foregroundLayers[i].gameObject.SetActive(true);
					maskedToggle.isOn = data.foregroundLayers[i].masked;
				}
				else
				{
					foregroundLayers[i].gameObject.SetActive(false);
				}
			}
			art.gameObject.SetActive(false);
		}
		else if(artIndex < 0)
		{
			for (int i = 0; i < backgroundLayers.Count; i++)
			{
				if (i == (Mathf.Abs(artIndex) - 1))
				{
					backgroundLayers[i].gameObject.SetActive(true);
					maskedToggle.isOn = data.backgroundLayers[i].masked;
				}
				else
				{
					backgroundLayers[i].gameObject.SetActive(false);
				}
			}
			for (int i = 0; i < foregroundLayers.Count; i++)
			{
				foregroundLayers[i].gameObject.SetActive(false);
			}
			art.gameObject.SetActive(false);
		}
		uploadArt.gameObject.SetActive(true);
	}

	public void AddForeground(string path = "")
	{
		ArtTab artTab = null;
		if(foregroundTabs != null)
		{
			if(foregroundLayers.Count < foregroundTabs.Count)
			{
				artTab = foregroundTabs[foregroundLayers.Count].GetComponent<ArtTab>();
				foregroundTabs[foregroundLayers.Count].gameObject.SetActive(true);
			}
			else
			{
				artTab = Instantiate(leftTabObj,leftTabs).GetComponent<ArtTab>();
				foregroundTabs.Add((RectTransform)artTab.transform);
			}
		}
		else
		{
			foregroundTabs = new List<RectTransform>();
		}
		
		artTab.artIndex = foregroundLayers.Count + 2;
		artTab.indexText.text = (artTab.artIndex - 1).ToString();
		((RectTransform)artTab.transform).anchoredPosition = Vector2.right * 22 * (foregroundLayers.Count + 1);
		((RectTransform)addForeground.transform).anchoredPosition = Vector2.right * 22 * (foregroundLayers.Count + 2);
		foregroundLayers.Add(Instantiate(art, maskImage.rectTransform));
		foregroundLayers[foregroundLayers.Count - 1].rectTransform.SetAsLastSibling();
		if (string.IsNullOrEmpty(path))
		{
			data.foregroundLayers.Add(new CardArt());
		}
		else
		{
			if (!data.foregroundLayers[foregroundLayers.Count - 1].masked)
			{
				foregroundLayers[foregroundLayers.Count - 1].maskable = false;
			}
			else
			{
				foregroundLayers[foregroundLayers.Count - 1].maskable = true;
			}
			foregroundLayers[foregroundLayers.Count - 1].texture = CastleTools.LoadImage(Path.Combine(CardLoader.GetSavePath(""), data.foregroundLayers[foregroundLayers.Count - 1].imageDir));
			foregroundLayers[foregroundLayers.Count - 1].SetNativeSize();
			foregroundLayers[foregroundLayers.Count - 1].rectTransform.sizeDelta = foregroundLayers[foregroundLayers.Count - 1].rectTransform.sizeDelta / 2;
			foregroundLayers[foregroundLayers.Count - 1].transform.position = artBacking.transform.position;
		}
	}

	public void AddBackground(string path = "")
	{
		ArtTab artTab = null;
		if (foregroundTabs != null)
		{
			if (backgroundLayers.Count < backgroundTabs.Count)
			{
				artTab = backgroundTabs[backgroundLayers.Count].GetComponent<ArtTab>();
				backgroundTabs[backgroundLayers.Count].gameObject.SetActive(true);
			}
			else
			{
				artTab = Instantiate(leftTabObj, leftTabs).GetComponent<ArtTab>();
				backgroundTabs.Add((RectTransform)artTab.transform);
			}
		}
		else
		{
			backgroundTabs = new List<RectTransform>();
		}
		artTab.artIndex = -backgroundLayers.Count - 1;
		artTab.indexText.text = artTab.artIndex.ToString();
		((RectTransform)artTab.transform).anchoredPosition = Vector2.left * 22 * (backgroundLayers.Count + 1);
		((RectTransform)addBackground.transform).anchoredPosition = Vector2.left * 22 * (backgroundLayers.Count + 2);
		backgroundLayers.Add(Instantiate(art,maskImage.rectTransform));
		backgroundLayers[backgroundLayers.Count - 1].rectTransform.SetAsFirstSibling();
		if (string.IsNullOrEmpty(path))
		{
			data.backgroundLayers.Add(new CardArt());
		}
		else
		{
			if (!data.backgroundLayers[backgroundLayers.Count - 1].masked)
			{
				backgroundLayers[backgroundLayers.Count - 1].maskable = false;
			}
			else
			{
				backgroundLayers[backgroundLayers.Count - 1].maskable = true;
			}
			backgroundLayers[backgroundLayers.Count - 1].texture = CastleTools.LoadImage(Path.Combine(CardLoader.GetSavePath(""), data.backgroundLayers[backgroundLayers.Count - 1].imageDir));
			backgroundLayers[backgroundLayers.Count - 1].SetNativeSize();
			backgroundLayers[backgroundLayers.Count - 1].rectTransform.sizeDelta = backgroundLayers[backgroundLayers.Count - 1].rectTransform.sizeDelta / 2;
			backgroundLayers[backgroundLayers.Count - 1].transform.position = artBacking.transform.position;
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
		print(SanitizePath(loadedArtPath));
		if (artIndex == 1)
		{
			art.texture = CastleTools.LoadImage(loadedArtPath);
			art.SetNativeSize();
			art.rectTransform.sizeDelta = art.rectTransform.sizeDelta / 2;
			data.mainArt.imageDir = SanitizePath(loadedArtPath);
		}
		else if (artIndex < 0)
		{
			backgroundLayers[Mathf.Abs(artIndex) - 1].texture = CastleTools.LoadImage(loadedArtPath);
			backgroundLayers[Mathf.Abs(artIndex) - 1].SetNativeSize();
			backgroundLayers[Mathf.Abs(artIndex) - 1].rectTransform.sizeDelta = backgroundLayers[Mathf.Abs(artIndex) - 1].rectTransform.sizeDelta / 2;
			data.backgroundLayers[Mathf.Abs(artIndex) - 1].imageDir = SanitizePath(loadedArtPath);
		}
		else if (artIndex > 0)
		{
			foregroundLayers[artIndex - 2].texture = CastleTools.LoadImage(loadedArtPath);
			foregroundLayers[artIndex - 2].SetNativeSize();
			foregroundLayers[artIndex - 2].rectTransform.sizeDelta = foregroundLayers[artIndex - 2].rectTransform.sizeDelta / 2;
			data.foregroundLayers[artIndex - 2].imageDir = SanitizePath(loadedArtPath);
		}
		coll.enabled = true;
		Save();
	}
	public string SanitizePath(string path)
	{
		path = path.Remove(0, CardLoader.GetSavePath("").Length + 1);
		return path;
	}

	public void AdjustForeground(float _val)
	{
		data.foregroundScale = _val;
		foregroundScale = _val;
		foregroundSliderText.text = _val.ToString();
		Save();
	}

	public void AdjustBackground(float _val)
	{
		data.backgroundScale = _val;
		backgroundScale = _val;
		backgroundSliderText.text = _val.ToString();
		Save();
	}

	public void SetName(string _name)
	{
		data.name = _name;
		Save();
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
	void Update()
	{
		if (artIndex == 0)
		{
			rightTabs.anchoredPosition = Vector2.Lerp(rightTabs.anchoredPosition, Vector2.left * 30, Time.deltaTime * 15);
		}
		else
		{
			rightTabs.anchoredPosition = Vector2.Lerp(rightTabs.anchoredPosition, Vector2.left * 8, Time.deltaTime * 15);
		}
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
