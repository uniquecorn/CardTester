using System.Collections.Generic;

[System.Serializable]
public class CardData
{
	public string name;
	public CardArt mainArt;
	public List<CardArt> backgroundLayers;
	public List<CardArt> foregroundLayers;
	public float backgroundScale;
	public float foregroundScale;
	public bool maskLeft, maskRight, maskTop, maskBottom;
}
