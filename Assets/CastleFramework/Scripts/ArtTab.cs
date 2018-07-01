using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtTab : MonoBehaviour
{
	public Toggle toggle;
	public int artIndex;
	public Text indexText;
	public Card card;

	public void Select(bool isOn)
	{
		if(isOn)
		{
			card.artIndex = artIndex;
		}
		else
		{
			card.artIndex = 0;
		}
		card.SelectTab();
	}
	private void Update()
	{
		if(toggle.isOn)
		{
			((RectTransform)toggle.transform).sizeDelta = Vector2.Lerp(((RectTransform)toggle.transform).sizeDelta, (Vector2.one * 20) + (Vector2.right * 10), Time.deltaTime * 10);
		}
		else
		{
			((RectTransform)toggle.transform).sizeDelta = Vector2.Lerp(((RectTransform)toggle.transform).sizeDelta, (Vector2.one * 20) + (Vector2.right * 5), Time.deltaTime * 10);
		}
	}
}