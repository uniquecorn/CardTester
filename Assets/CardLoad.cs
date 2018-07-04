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

	public void Delete()
	{
		StartCoroutine(DeleteDelay());
	}
	public IEnumerator DeleteDelay()
	{
		bool check = false;
		yield return StartCoroutine(ConfirmPrompt.instance.RunPrompt(result => check = result));
		if(check)
		{
			CardLoader.instance.loadedData.data.RemoveAt(pos);
			CardLoader.instance.ResetLoaders();
		}
	}

	public void LoadCard()
	{
		CardLoader.instance.loadedCard = pos;
		CardLoader.instance.card.Load(CardLoader.instance.loadedData.data[pos]);
	}

	public void SetName(string cardName)
	{
		CardLoader.instance.loadedData.data[pos].name = cardName;
	}
}
