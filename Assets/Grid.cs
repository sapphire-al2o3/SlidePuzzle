using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
	const int Width = 4;
	const int Height = 4;
	const float ScaleX = 4.0f / Width;
	const float ScaleY = 4.0f / Height;
	const int Size = Width * Height;
	const int Empty = Width * Height - 1;

	public GameObject start;
	public GameObject complete;

	Piece[] pieces = new Piece[Size];
	int[] grid = new int[Size];

	bool _moving = false;
	bool _start = false;
	float _elapesed = 0.0f;
	float _time = 0.0f;
	int _count = 0;

	public TextMesh timeText;

	void Awake()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			int x = i % Width;
			int y = i / Width;
			transform.GetChild(i).position = GetPosition(x, y);
			Piece p = transform.GetChild(i).GetComponent<Piece>();
			p.index = i;
			pieces[i] = p;
		}

		for(int i = 0; i < grid.Length; i++)
		{
			grid[i] = i;
		}
	}

	void Update()
	{
		if (_start)
		{
			if (_moving)
			{
				_elapesed += Time.deltaTime;

				if (_elapesed > 1.0f)
				{
					_moving = false;
				}
			}

			_time += Time.deltaTime;

			// 999秒でカンスト
			if (_time > 999.9f) _time = 999.0f;
			timeText.text = ((int)_time).ToString();
		}
	}

	/// <summary>
	/// マスをシャッフルする
	/// </summary>
	private void Shuffle()
	{
		for(int i = 0; i < Size * 2; i++)
		{
			int m = Random.Range(0, grid.Length - 1);
			int n = Random.Range(0, grid.Length - 1);

			if (m != n)
			{
				int t = grid[m];
				grid[m] = grid[n];
				grid[n] = t;
			}
			else
			{
				i--;
			}
		}

		for (int i = 0; i < grid.Length; i++)
		{
			int k = grid[i];
			if (k != Empty)
			{
				int x = i % Width;
				int y = i / Width;
				pieces[k].x = i % Width;
				pieces[k].y = i / Width;
				pieces[k].transform.position = GetPosition(x, y);
			}
		}
	}

	private Vector3 GetPosition(int index)
	{
		int x = index % Width - Width / 2;
		int y = -index / Width + Height / 2;
		return new Vector3(x + 0.5f, y + 0.5f, 0.0f);
	}

	/// <summary>
	/// 座標を求める
	/// </summary>
	private Vector3 GetPosition(int x, int y)
	{
		return new Vector3((x - Width / 2 + 0.5f) * 1.0f, (-y + Height / 2 - 0.5f) * 1.0f, 0.0f);
	}

	private bool IsEmpty(int x, int y)
	{
		int k = y * Width + x;
		return x >= 0 && x < Width && y >= 0 && y < Height && grid[k] == Empty;
	}

	void ReadyGame()
	{
		start.SetActive(true);
		start.SendMessage("Show");
	}

	void StartGame()
	{
		pieces[Empty].gameObject.SetActive(false);
		start.SendMessage("Hide");
		complete.SetActive(false);
		Shuffle();
		_start = true;
		_count = 0;
	}

	void FinishGame()
	{
		pieces[Empty].gameObject.SetActive(true);
		complete.SetActive(true);
//		complete.SendMessage("Show");
		_start = false;
	}

	void MovePiece(Piece piece)
	{
		if (!_start)
		{
			return;
		}

		int index = piece.index;
		
		// 周りに空いているマスがあるか調べる
		int y = piece.y;
		int x = piece.x;
		int k = y * Width + x;
		
		Debug.Log(x.ToString() + ":" + y.ToString());

		const float duration = 0.15f;

		if (IsEmpty(x, y - 1))
		{
			// 上
			grid[k - Width] = index;
//			grid[k] = Empty;
			piece.targetPosition = GetPosition(x, y - 1);
			piece.pos = k;
			piece.y = y - 1;
			Debug.Log("up");
			piece.SendMessage("Move", duration);
			_count++;
		}
		else if (IsEmpty(x, y + 1))
		{
			// 下
			grid[k + Width] = index;
//			grid[k] = Empty;
			piece.targetPosition = GetPosition(x, y + 1);
			piece.y = y + 1;
			piece.pos = k;
			Debug.Log("down");
			piece.SendMessage("Move", duration);
			_count++;
		}
		else if (IsEmpty(x - 1, y))
		{
			// 左
			grid[k - 1] = index;
//			grid[k] = Empty;
			piece.targetPosition = GetPosition(x - 1, y);
			piece.x = x - 1;
			piece.pos = k;
			Debug.Log("left");
			piece.SendMessage("Move", duration);
			_count++;
		}
		else if (IsEmpty(x + 1, y))
		{
			grid[k + 1] = index;
//			grid[k] = Empty;
			piece.targetPosition = GetPosition(x + 1, y);
			piece.x = x + 1;
			piece.pos = k;
			Debug.Log("right");
			piece.SendMessage("Move", duration);
			_count++;
		}
	}

	void StopPiece(Piece piece)
	{
		grid[piece.pos] = Empty;

		if (Check())
		{

			Debug.Log("Congratulations!");
			FinishGame();
		}
	}

	/// <summary>
	/// 揃ったか判定する
	/// </summary>
	private bool Check()
	{
		for (int i = 0; i < grid.Length; i++)
		{
			if (grid[i] != i)
			{
				return false;
			}
		}
		return true;
	}
}
