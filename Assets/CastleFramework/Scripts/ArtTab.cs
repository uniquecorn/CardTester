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
			if (isOn)
			{
				card.artIndex = artIndex;
			}
			else
			{
				card.artIndex = 0;
			}
			card.SelectTab();
	}
}