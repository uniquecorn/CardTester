using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtLayer : MonoBehaviour
{
	public RawImage art;
	public RawImage lighting;
	public bool masked;

	public void LoadArt(CardArt artData)
	{

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