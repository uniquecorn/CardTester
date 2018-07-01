using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectoryViewer : MonoBehaviour
{
	public RectTransform viewerTransform;
	public CanvasGroup viewerCanvas;
	public RectTransform dirContent;
	public List<DirectoryEntry> entries;
	public DirectoryEntry entryPrefab;
	public Text dirPath;
	[HideInInspector]
	public bool cancel;
	[HideInInspector]
	public string pickedFile;
	public DirectoryInfo currentPath;
	DirectoryInfo[] dirInfo;
	FileInfo[] filesInfo;

	public Button blocker;

	public bool visible;

	public IEnumerator SearchForFile(string path, System.Action<string> result)
	{
		blocker.gameObject.SetActive(true);
		visible = true;
		pickedFile = "";
		SwitchPath(path);
		while(string.IsNullOrEmpty(pickedFile))
		{
			if(cancel)
			{
				yield break;
			}
			else
			{
				yield return null;
			}
		}
		result(pickedFile);
		blocker.gameObject.SetActive(false);
		visible = false;
	}

	public void UpDir()
	{
		SwitchPath(currentPath.Parent.FullName);
	}

	public void SwitchPath(string path)
	{
		dirPath.text = path;
		currentPath = new DirectoryInfo(path);
		for(int i = 0; i < entries.Count; i++)
		{
			entries[i].gameObject.SetActive(false);
		}
		DirectoryInfo directoryInfo = new DirectoryInfo(path);
		dirInfo = directoryInfo.GetDirectories();
		for (int i = 0; i < dirInfo.Length; i++)
		{
			CreateEntry(dirInfo[i], i, false);
		}
		filesInfo = directoryInfo.GetFiles("*.png");
		for (int i = 0; i < filesInfo.Length; i++)
		{
			CreateEntry(filesInfo[i], i + dirInfo.Length, true);
		}
		dirContent.sizeDelta = new Vector2(0, (filesInfo.Length + dirInfo.Length) * (entryPrefab.entryTransform.sizeDelta.y + 2));
		//if (filesInfo.Length == 0 && dirInfo.Length == 0)
		//{
		//	dirEmpty.SetActive(true);
		//}
		//else
		//{
		//	dirEmpty.SetActive(false);
		//}
	}

	public void CreateEntry(FileSystemInfo path,int pos, bool file)
	{
		DirectoryEntry entry = GetEntry();
		entry.gameObject.SetActive(true);
		entry.Load(this,path, pos, file);
		entry.entryTransform.anchoredPosition = new Vector2(0, -2 + (-pos * (entry.entryTransform.sizeDelta.y + 2)));
	}

	public DirectoryEntry GetEntry()
	{
		for (int i = 0; i < entries.Count; i++)
		{
			if (!entries[i].gameObject.activeSelf)
			{
				return entries[i];
			}
		}
		DirectoryEntry newEntry = Instantiate(entryPrefab, dirContent).GetComponent<DirectoryEntry>();
		entries.Add(newEntry);
		return newEntry;
	}

	// Update is called once per frame
	void Update ()
	{
		if(visible)
		{
			viewerTransform.anchoredPosition = Vector2.Lerp(viewerTransform.anchoredPosition, Vector2.up * 5, Time.deltaTime * 10);
			viewerCanvas.alpha = Mathf.Lerp(viewerCanvas.alpha, 1, Time.deltaTime * 10);
		}
		else
		{
			viewerTransform.anchoredPosition = Vector2.Lerp(viewerTransform.anchoredPosition, Vector2.zero, Time.deltaTime * 10);
			viewerCanvas.alpha = Mathf.Lerp(viewerCanvas.alpha, 0, Time.deltaTime * 10);
		}
	}
}
