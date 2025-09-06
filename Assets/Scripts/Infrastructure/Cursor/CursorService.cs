using UnityEngine;

public class CursorService : ICursorService {
	private readonly IUpdateService _update;
	private readonly IInputService _input;
	private readonly CursorIcon _cursorIcon;

	public CursorService(IInputService input, CursorIcon cursorIcon) {
		_input = input;
		_cursorIcon = cursorIcon;
		_cursorIcon.Init(_input);
	}
	public void SetCursorIcon(Sprite icon) {
		_cursorIcon.Icon = icon;
	}

	public void SetDefaultCursorIcon() {
		_cursorIcon.Icon = null;
	}
	

	public void Dispose() {
	}
}