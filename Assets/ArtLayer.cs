using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ArtLayer : MonoBehaviour
{
	public RawImage art;
	public RawImage lighting;

	public RectTransform RTransform
	{
		get
		{
			return (RectTransform)transform;
		}
	}

	public void ApplyMask(bool isOn)
	{
		art.maskable = lighting.maskable = isOn;
	}

	public void LoadArt(CardArt artData)
	{
		if(string.IsNullOrEmpty(artData.imageDir))
		{
			print("no dir");
			art.color = Color.clear;
		}
		else if (!File.Exists(Path.Combine(CardLoader.GetSavePath(""), artData.imageDir)))
		{
			print("dir doesn't exist");
			art.color = Color.clear;
		}
		else
		{
			LoadArt(CastleTools.LoadImage(Path.Combine(CardLoader.GetSavePath(""), artData.imageDir)));
			art.color = Color.white;
		}
		if (string.IsNullOrEmpty(artData.lightingDir))
		{
			print("no dir");
			lighting.color = Color.clear;
		}
		else if (!File.Exists(Path.Combine(CardLoader.GetSavePath(""), artData.lightingDir)))
		{
			print("dir doesn't exist");
			lighting.color = Color.clear;
		}
		else
		{
			LoadLighting(CastleTools.LoadImage(Path.Combine(CardLoader.GetSavePath(""), artData.lightingDir)));
			lighting.color = Color.white;
		}
		ApplyMask(artData.masked);
	}
	public void LoadArt (Texture _tex)
	{
		art.texture = _tex;
		art.SetNativeSize();
		art.rectTransform.sizeDelta /= 2;
	}

	public void LoadLighting(Texture _tex)
	{
		lighting.texture = _tex;
		lighting.SetNativeSize();
		lighting.rectTransform.sizeDelta /= 2;
	}
}