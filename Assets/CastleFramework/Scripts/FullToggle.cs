using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullToggle : MonoBehaviour
{
	public Toggle toggleObj;
	public Image fillImage;
	public Text textObj;
	
	// Update is called once per frame
	void Update ()
	{
		if(toggleObj.isOn)
		{
			fillImage.color = Color.Lerp(fillImage.color, CastleTools.Full(fillImage.color), Time.deltaTime * 15);
			textObj.color = Color.Lerp(textObj.color, Color.white, Time.deltaTime * 15);
		}
		else
		{
			fillImage.color = Color.Lerp(fillImage.color, CastleTools.Clear(fillImage.color), Time.deltaTime * 15);
			textObj.color = Color.Lerp(textObj.color, CastleTools.Full(fillImage.color), Time.deltaTime * 15);
		}
	}
}
