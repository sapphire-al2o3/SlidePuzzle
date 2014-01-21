using UnityEngine;
using System.Collections;

public class Complete : MonoBehaviour
{
	void Hide()
	{
		GameObject.Find("Grid").SendMessage("ReadyGame");
		//gameObject.SetActive(false);
	}
}
