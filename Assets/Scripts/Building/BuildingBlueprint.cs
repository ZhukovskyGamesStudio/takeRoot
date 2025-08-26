using System.Collections.Generic;
using CodeBase.Services;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BuildingBlueprint : MonoBehaviour, IUpdatable {
	public Dictionary<ResourceType, int> RequiredResources;
	public Dictionary<ResourceType, int> ReservedRequiredResources;
	public Dictionary<ResourceType, int> ResourceStorage;
	
	public bool IsPlaced;
	private bool _canPlace;
	
	[SerializeField]private SpriteRenderer _sprite;
	private IUpdateService _update;
	private Camera _camera;
	private IGridService _grid;
	private GridObject _gridObject;

	public void Init(BuildingRecipeConfig config) {
		_camera = Camera.main;
		IsPlaced = false;
		_update = ServiceLocator.Container.Single<IUpdateService>();
		_grid = ServiceLocator.Container.Single<IGridService>();
		_gridObject = GetComponent<GridObject>();

		_gridObject.MultiplyGridOffset.x = config.Footprint.x - 1;
		_gridObject.MultiplyGridOffset.y = config.Footprint.y - 1;
		_sprite.sprite = config.Icon;
		_update.Register(this);
	}

	private void Place() {
		_update.Unregister(this);
		IsPlaced = true;
	}
	public void Update() {
		if (IsPlaced) return;
		BuildingShadowMouseFollow();
		CheckObstacles();
	}
	private void BuildingShadowMouseFollow() {
		Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
		Vector3 newPosition = AdjustPositionToGrid(mousePosition);
		newPosition.z = transform.position.z;
		transform.position = newPosition;
		_gridObject.UpdatePosition();
	}
	private Vector3 AdjustPositionToGrid(Vector3 position) {
		return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), position.z);
	}
	
	private void CheckObstacles() {
		List<Vector3> occupied = _gridObject.GetObjectPositions();
		foreach (Vector3 cell in occupied) {
			if (_grid.OnMap(cell) && !_grid.IsOccupiedPos(cell)) {
				continue;
			}

			_canPlace = false;
			_sprite.color = new Color(255, 0, 0, 100);
			return;
		}

		_canPlace = true;
		_sprite.color = new Color(0, 0, 255, 100);
	}
}