using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPrompt : MonoBehaviour
{
	public bool running;
	public CanvasGroup group;
	public static ConfirmPrompt instance;
	public enum ConfirmState
	{
		UNCONFIRM,
		YES,
		NO
	}
	public ConfirmState state;

	private void Start()
	{
		instance = this;
	}

	public void Yes()
	{
		state = ConfirmState.YES;
	}

	public void No()
	{
		state = ConfirmState.NO;
	}

	public IEnumerator RunPrompt(System.Action<bool> result)
	{
		running = true;
		state = ConfirmState.UNCONFIRM;
		while (state == ConfirmState.UNCONFIRM)
		{
			yield return null;
		}
		if(state == ConfirmState.YES)
		{
			result(true);
		}
		else
		{
			result(false);
		}
		running = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(running)
		{
			group.interactable = group.blocksRaycasts = true;
			group.alpha = Mathf.Lerp(group.alpha, 1, Time.deltaTime * 15);
		}
		else
		{
			group.interactable = group.blocksRaycasts = false;
			group.alpha = Mathf.Lerp(group.alpha, 0, Time.deltaTime * 15);
		}
	}
}
