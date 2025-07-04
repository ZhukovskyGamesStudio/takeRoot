using UnityEngine;

public class Waterer : MonoBehaviour, IWaterer {
	[Header("Waterer Settings")]
	public float waterAmount = 15f;
	public float waterCooldown = 0.5f;
	
	private float _lastWaterTime;
	
	public void Water(CommandTarget target) {
		if (OnCooldown()) return;
		
		
		target.Water(waterAmount);
		_lastWaterTime = Time.time;
	}
	
	public bool OnCooldown() {
		return Time.time - _lastWaterTime < waterCooldown;
	}
}