using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DirectoryEntry : MonoBehaviour
{
	[HideInInspector]
	public DirectoryViewer dirViewer;
	public RectTransform entryTransform;
	private FileSystemInfo path;
	public Text directoryName;
	public bool isFile;
	public Image icon;
	public Sprite folder, file;
	// Use this for initialization
	public void Load (DirectoryViewer _dirV, FileSystemInfo _path, int pos, bool _file)
	{
		dirViewer = _dirV;
		directoryName.text = _path.Name;
		path = _path;
		isFile = _file;
		if(isFile)
		{
			icon.sprite = file;
		}
		else
		{
			icon.sprite = folder;
		}
	}

	public void Select()
	{
		if(isFile)
		{
			dirViewer.pickedFile = path.FullName;
		}
		else
		{
			dirViewer.SwitchPath(path.FullName);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
