using UnityEngine;

namespace AI {
	public class SimpleAttacker : MonoBehaviour, IAttacker {
		[SerializeField]private int _damage = 25;
		[SerializeField]private float _cooldown = 5;
		private float _timer;
		
		private WorkerAnimator _animator;
		public void Init(WorkerAnimator animator) {
			_animator = animator;
		}
		
		public void Attack(Zombie zombie) {
			if (_timer >= _cooldown) {
				zombie.TakeDamage(_damage);
				_timer = 0;
			}
		}

		public void Update() {
			_timer += Time.deltaTime;
		}

		public void Cancel() {
			throw new System.NotImplementedException();
		}
	}
}