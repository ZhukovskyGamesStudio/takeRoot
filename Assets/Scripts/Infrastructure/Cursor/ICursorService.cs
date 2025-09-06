using UnityEngine;

public interface ICursorService : IService {
	void SetCursorIcon(Sprite icon);
	void SetDefaultCursorIcon();
}