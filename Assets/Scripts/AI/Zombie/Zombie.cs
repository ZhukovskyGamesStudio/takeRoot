using System;
using CodeBase.Services;
using UnityEditor;
using UnityEngine;

namespace AI {
	public class Zombie : MonoBehaviour, IUpdatable{
		private BTRoot_Zombie _root;
		
		public ZombieData Data;
		
		public IZombieMover Mover;
		private IUpdateService _update;
		
		private float m_Value;

		private void Start() {
			Mover = GetComponent<IZombieMover>();
			_root = new BTRoot_Zombie(this, ServiceLocator.Container.Single<ISettlersService>());
			_update = ServiceLocator.Container.Single<IUpdateService>();
			_update.Register(this);
		}

		public void Update() {
			_root.Evaluate();
		}

		public void Dispose() {
			_update.Unregister(this);
		}
		
		Vector3 ScreenToWorld(float x, float y) {
			Camera camera = Camera.current;
			Vector3 s = camera.WorldToScreenPoint(transform.position);
			return camera.ScreenToWorldPoint(new Vector3(x, camera.pixelHeight - y, s.z));
		}

		Rect ScreenRect(int x, int y, int w, int h) {
			Vector3 tl = ScreenToWorld(x, y);
			Vector3 br = ScreenToWorld(x + w, y + h);
			return new Rect(tl.x, tl.y, br.x - tl.x, br.y - tl.y);
		}
#if UNITY_EDITOR
		void OnDrawGizmosSelected()
		{
			Rect rect = ScreenRect((int)Data.DetectArea.x, (int)Data.DetectArea.y, (int)Data.DetectArea.width, (int)Data.DetectArea.height);
			UnityEditor.Handles.DrawSolidRectangleWithOutline(Data.DetectArea, Color.black, Color.white);
		}
#endif
		
	}
}