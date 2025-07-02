using UnityEngine;
using System;

public class Health : MonoBehaviour {
	[Header("Health Settings")]
	public float maxHealth = 100f;
	public float currentHealth;
	
	public event Action OnDeath;
	public event Action<float> OnHealthChanged;
	
	public bool IsDead {
		get => currentHealth <= 0f;
		set => throw new NotImplementedException();
	}

	private void Start() {
		currentHealth = maxHealth;
	}
	
	public void TakeDamage(float damage) {
		if (IsDead) return;
		
		currentHealth = Mathf.Max(0f, currentHealth - damage);
		OnHealthChanged?.Invoke(currentHealth);
	}
	public void Die() {
		OnDeath?.Invoke();
		Destroy(gameObject);
	}
} 