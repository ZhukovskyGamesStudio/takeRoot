using UnityEngine;

public class Destroyer : MonoBehaviour, IDestroyer {
	[Header("Destroyer Settings")]
	public float damage = 25f;
	public float hitCooldown = 0.5f;
	
	private float _lastHitTime;
	
	public void Hit(CommandTarget target) {
		if (OnCooldown()) return;
		
		
		target.TakeDamage(damage);
		_lastHitTime = Time.time;
	}
	
	public bool OnCooldown() {
		return Time.time - _lastHitTime < hitCooldown;
	}
} 