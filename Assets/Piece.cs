using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Piece : MonoBehaviour
{
	public int index;
	public int pos;
	public int x;
	public int y;
	public Vector3 targetPosition;
	
	bool _moving = false;
	Vector3 _start;
	float _time = 0.0f;
	float _duration;
	
	void OnMouseDown()
	{
		if (!_moving)
		{
			transform.parent.SendMessage("MovePiece", this);
		}
	}
	
	void Move(float duration)
	{
		_moving = true;
		_time = 0.0f;
		_start = transform.position;
		_duration = duration;
	}

	float Ease(float x)
	{
		return x * x;
	}

	void Update()
	{
		if (_moving)
		{
			_time += Time.deltaTime;
			float t = Mathf.Min(_time / _duration, 1.0f);
			Vector3 pos = Vector3.Lerp(_start, targetPosition, Ease(t));
			transform.position = pos;

			if (_duration < _time)
			{
				_moving = false;
				transform.parent.SendMessage("StopPiece", this);
			}
		}
	}
}
