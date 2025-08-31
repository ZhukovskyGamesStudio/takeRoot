using System;
using CodeBase.Services;
using UnityEngine;

namespace AI {
	public class Zombie : MonoBehaviour, IUpdatable{
		private BTRoot_Zombie _root;
		
		public ZombieData Data;
		
		public IZombieMover Mover;
		private IUpdateService _update;

		private void Start() {
			Mover = GetComponent<IZombieMover>();
			_root = new BTRoot_Zombie(this);
			_update = ServiceLocator.Container.Single<IUpdateService>();
			_update.Register(this);
		}

		public void Update() {
			_root.Evaluate();
		}
	}
}