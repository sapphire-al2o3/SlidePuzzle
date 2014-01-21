using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class StartButton : MonoBehaviour
{
	bool _disable = false;

	void OnMouseDown()
	{
		if (_disable)
		{
			return;
		}
		GameObject g = GameObject.Find("Grid");
		g.SendMessage("StartGame");
	}

	void Show()
	{
		//_disable = true;
	}

	void Hide()
	{
		gameObject.SetActive(false);
	}
}
