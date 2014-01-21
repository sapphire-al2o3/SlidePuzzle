using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;

public class PuzzleEditor : Editor {

	[MenuItem("Editor/Split Texture")]
	static void SplitTexture()
	{
		int k = 0;

		var r = new Regex(@"(\d+)$");
		foreach (var o in Selection.objects.OrderBy(a => r.Replace(a.name, m => int.Parse(m.Value).ToString("D3"))))
		{
			Sprite sprite = o as Sprite;
			if (o != null)
			{
				int x = k % 8 - 4;
				int y = k / 8 - 4;
				GameObject g = new GameObject(sprite.name);
				g.AddComponent<SpriteRenderer>().sprite = sprite;
				g.AddComponent<Piece>();
				g.transform.position = new Vector3(x * 0.5f, -y * 0.5f + 0.5f);
				Debug.Log(sprite.name);
				Undo.RegisterCreatedObjectUndo(g, "Split Texture");
				k++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
