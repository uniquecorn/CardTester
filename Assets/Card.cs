using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Castle;
using System.IO;
using System.Linq;

public class Card : CastleObject
{
	public CardData data;
	public RectTransform artBacking;
	public RectTransform leftTabs;
	public RectTransform rightTabs;
	public ToggleGroup tabs;
	public Toggle leftTabObj;
	public List<RectTransform> backgroundTabs;
	public List<RectTransform> foregroundTabs;
	public Image maskImage;
	public ArtLayer art;
	public List<ArtLayer> backgroundLayers;
	public List<ArtLayer> foregroundLayers;
	public InputField nameText;
	public float backgroundScale, foregroundScale;
	public bool isHeld;
	public Button addBackground, addForeground;
	public Toggle maskL, maskR, maskT, maskB, maskedToggle;

	public Button uploadArt, uploadLight;
	public Slider foregroundSlider, backgroundSlider;
	public Text foregroundSliderText, backgroundSliderText;
	public Texture missingTexture;

	public DirectoryViewer dirViewer;
	// Use this for initialization
	public void Load (CardData _data)
	{
		tabs.SetAllTogglesOff();
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
		art.LoadArt(_data.mainArt);
		backgroundSlider.value = backgroundScale;
		foregroundSlider.value = foregroundScale;
		backgroundSliderText.text = backgroundScale.ToString();
		foregroundSliderText.text = foregroundScale.ToString();
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
		if(tabs.AnyTogglesOn())
		{
			ArtTab currTab = tabs.ActiveToggles().First().GetComponent<ArtTab>();
			currTab.artLayer.ApplyMask(isOn);
			if (currTab.artLayer == art)
			{
				data.mainArt.masked = isOn;
				Save();
				return;
			}
			else
			{
				for(int i = 0; i < foregroundLayers.Count; i++)
				{
					if(currTab.artLayer == foregroundLayers[i])
					{
						data.foregroundLayers[i].masked = isOn;
						Save();
						return;
					}
				}
				for (int i = 0; i < backgroundLayers.Count; i++)
				{
					if (currTab.artLayer == backgroundLayers[i])
					{
						data.backgroundLayers[i].masked = isOn;
						Save();
						return;
					}
				}
			}
		}
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
		float offsetX = art.RTransform.sizeDelta.x - 96;
		float offsetY = art.RTransform.sizeDelta.y - 126;
		float maskWidth = art.RTransform.sizeDelta.x;
		float maskHeight = art.RTransform.sizeDelta.y;
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
			backgroundLayers = new List<ArtLayer>();
			for(int i = 0; i < data.backgroundLayers.Count; i++)
			{
				AddBackground();
			}
		}
		if(data.foregroundLayers != null)
		{
			foregroundLayers = new List<ArtLayer>();
			for(int i = 0; i < data.foregroundLayers.Count; i++)
			{
				AddForeground();
			}
		}
		((RectTransform)addBackground.transform).anchoredPosition = Vector2.left * 52 * (backgroundLayers.Count + 1);
		((RectTransform)addForeground.transform).anchoredPosition = Vector2.right * 52 * (foregroundLayers.Count + 1);
	}

	public void SelectTab()
	{
		if (!tabs.AnyTogglesOn())
		{
			for (int i = 0; i < backgroundLayers.Count; i++)
			{
				backgroundLayers[i].gameObject.SetActive(true);
			}
			for (int i = 0; i < foregroundLayers.Count; i++)
			{
				foregroundLayers[i].gameObject.SetActive(true);
			}
			art.gameObject.SetActive(true);
			uploadArt.gameObject.SetActive(false);
			uploadLight.gameObject.SetActive(false);
			return;
		}
		else
		{
			for (int i = 0; i < backgroundLayers.Count; i++)
			{
				backgroundLayers[i].gameObject.SetActive(false);
			}
			for (int i = 0; i < foregroundLayers.Count; i++)
			{
				foregroundLayers[i].gameObject.SetActive(false);
			}
			art.gameObject.SetActive(false);

			ArtTab currTab = tabs.ActiveToggles().First().GetComponent<ArtTab>();
			currTab.artLayer.gameObject.SetActive(true);
			maskedToggle.isOn = currTab.artLayer.art.maskable;
			uploadArt.gameObject.SetActive(true);
			uploadLight.gameObject.SetActive(true);
		}
	}

	public void AddForeground(bool newLayer = false)
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
		
		artTab.indexText.text = (foregroundLayers.Count + 1).ToString();
		((RectTransform)artTab.transform).anchoredPosition = Vector2.right * 52 * (foregroundLayers.Count + 1);
		((RectTransform)addForeground.transform).anchoredPosition = Vector2.right * 52 * (foregroundLayers.Count + 2);
		foregroundLayers.Add(Instantiate(art, maskImage.rectTransform));
		foregroundLayers[foregroundLayers.Count - 1].RTransform.SetAsLastSibling();
		artTab.artLayer = foregroundLayers[foregroundLayers.Count - 1];
		if (newLayer)
		{
			data.foregroundLayers.Add(new CardArt());
			foregroundLayers[foregroundLayers.Count - 1].LoadArt(data.foregroundLayers[foregroundLayers.Count - 1]);
		}
		else
		{
			foregroundLayers[foregroundLayers.Count - 1].LoadArt(data.foregroundLayers[foregroundLayers.Count - 1]);
		}
	}

	public void AddBackground(bool newLayer = false)
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
		artTab.indexText.text = (-backgroundLayers.Count - 1).ToString();
		((RectTransform)artTab.transform).anchoredPosition = Vector2.left * 52 * (backgroundLayers.Count + 1);
		((RectTransform)addBackground.transform).anchoredPosition = Vector2.left * 52 * (backgroundLayers.Count + 2);
		backgroundLayers.Add(Instantiate(art,maskImage.rectTransform));
		backgroundLayers[backgroundLayers.Count - 1].RTransform.SetAsFirstSibling();
		artTab.artLayer = backgroundLayers[backgroundLayers.Count - 1];
		if (newLayer)
		{
			data.backgroundLayers.Add(new CardArt());
			backgroundLayers[backgroundLayers.Count - 1].LoadArt(data.backgroundLayers[backgroundLayers.Count - 1]);
		}
		else
		{
			backgroundLayers[backgroundLayers.Count - 1].LoadArt(data.backgroundLayers[backgroundLayers.Count - 1]);
		}
	}

	public void LoadNewArt()
	{
		StartCoroutine(LoadArt());
	}
	public void LoadNewLight()
	{
		StartCoroutine(LoadArt(true));
	}

	public IEnumerator LoadArt(bool light = false)
	{
		coll.enabled = false;
		string loadedArtPath = "";
		yield return dirViewer.StartCoroutine(dirViewer.SearchForFile(CardLoader.GetSavePath(""), result => loadedArtPath =result));
		if(!string.IsNullOrEmpty(loadedArtPath))
		{
			loadedArtPath = SanitizePath(loadedArtPath);
			ArtTab currTab = tabs.ActiveToggles().First().GetComponent<ArtTab>();
			if (currTab.artLayer == art)
			{
				if (light)
				{
					data.mainArt.lightingDir = loadedArtPath;
				}
				else
				{
					data.mainArt.imageDir = loadedArtPath;
				}
				currTab.artLayer.LoadArt(data.mainArt);
			}
			else
			{
				for (int i = 0; i < foregroundLayers.Count; i++)
				{
					if (currTab.artLayer == foregroundLayers[i])
					{
						if (light)
						{
							data.foregroundLayers[i].lightingDir = loadedArtPath;
						}
						else
						{
							data.foregroundLayers[i].imageDir = loadedArtPath;
						}
						currTab.artLayer.LoadArt(data.foregroundLayers[i]);
					}
				}
				for (int i = 0; i < backgroundLayers.Count; i++)
				{
					if (currTab.artLayer == backgroundLayers[i])
					{
						if (light)
						{
							data.backgroundLayers[i].lightingDir = loadedArtPath;
						}
						else
						{
							data.backgroundLayers[i].imageDir = loadedArtPath;
						}
						currTab.artLayer.LoadArt(data.backgroundLayers[i]);
					}
				}
			}
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
		_val = Mathf.Round(_val * 100f) / 100f;
		data.foregroundScale = _val;
		foregroundScale = _val;
		foregroundSliderText.text = _val.ToString();
		Save();
	}

	public void AdjustBackground(float _val)
	{
		_val = Mathf.Round(_val * 100f) / 100f;
		data.backgroundScale = _val;
		backgroundScale = _val;
		backgroundSliderText.text = _val.ToString();
		Save();
	}

	public void SetName(string _name)
	{
		data.name = _name;
		Save();
		CardLoader.instance.cardLoaders[CardLoader.instance.loadedCard].Load(CardLoader.instance.loadedCard);
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
		if (!tabs.AnyTogglesOn())
		{
			rightTabs.anchoredPosition = Vector2.Lerp(rightTabs.anchoredPosition, Vector2.left * 30, Time.deltaTime * 15);
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Escape) && !dirViewer.visible)
			{
				tabs.SetAllTogglesOff();
			}
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
