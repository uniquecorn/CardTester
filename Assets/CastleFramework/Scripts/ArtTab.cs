using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtTab : MonoBehaviour
{
	public ArtLayer artLayer;
	public Text indexText;
	public Card card;

	public void Select(bool isOn)
	{
		card.SelectTab();
	}
}