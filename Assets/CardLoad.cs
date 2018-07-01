using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLoad : MonoBehaviour
{
	public Text textObj;
	public int pos;
	// Use this for initialization
	public void Load(int _pos)
	{
		pos = _pos;
		textObj.text = CardLoader.instance.loadedData.data[pos].name;
	}

	public void LoadCard()
	{
		CardLoader.instance.card.Load(CardLoader.instance.loadedData.data[pos]);
	}

	public void SetName(string cardName)
	{
		CardLoader.instance.loadedData.data[pos].name = cardName;
	}
}
