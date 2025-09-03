using System;
using UnityEngine;

namespace AI {
	public class SimpleZombieAttacker : MonoBehaviour, IZombieAttacker {
		[SerializeField]private int _damage;
		[SerializeField]private float _cooldown;
		private float _timer;
		
		public void Attack(Settler settler) {
			if (_timer >= _cooldown) {
				settler.Data.needs.Hp -= _damage;
				_timer = 0;
			}
		}

		public void Update() {
			_timer += Time.deltaTime;
		}
	}
}