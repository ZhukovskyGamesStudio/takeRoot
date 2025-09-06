using System;
using UnityEngine;

public class CursorIcon : MonoBehaviour {
	[SerializeField]private SpriteRenderer spriteRenderer;
	[SerializeField]private Vector2 _offset;
	private IInputService _input;

	public void Init(IInputService input) {
		_input = input;
	}
	public Sprite Icon {
		get => spriteRenderer.sprite;
		set => spriteRenderer.sprite = value;
	}

	private void LateUpdate() {
		transform.position = _input.GetWorldMousePosition() + _offset;
	}
}
