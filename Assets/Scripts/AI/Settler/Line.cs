using UnityEngine;

namespace AI {
	public class Line : MonoBehaviour {
		[SerializeField]private LineRenderer _lineRenderer;
		public void Init(Vector3 start, Vector3 end) {
			_lineRenderer.positionCount = 2;
			_lineRenderer.SetPosition(0, start);
			_lineRenderer.SetPosition(1, end);
		}
	}
}