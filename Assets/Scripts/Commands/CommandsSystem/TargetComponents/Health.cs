using UnityEngine;
using System;
using System.Collections.Generic;
using CodeBase.Services;
using GameResources;
using Settlers.Infrastructure;

public class Health : MonoBehaviour {
	[Header("Health Settings")]
	public float maxHealth = 100f;
	public float currentHealth;
	private IResourceManager _resources;
	[SerializeField] private List<ResourcesData> _drop;
	public event Action OnDeath;
	public event Action<float> OnHealthChanged;
	
	public bool IsDead {
		get => currentHealth <= 0f;
	}

	private void Start() {
		currentHealth = maxHealth;
		_resources = ServiceLocator.Container.Single<IResourceManager>();
	}
	
	public void TakeDamage(float damage) {
		currentHealth = Mathf.Max(0f, currentHealth - damage);
		OnHealthChanged?.Invoke(currentHealth);
	}
	public void Die() {
		OnDeath?.Invoke();
		foreach (ResourcesData drop in _drop) {
			_resources.SpawnResource(transform.position, drop.type, drop.amount);
		}
		Destroy(gameObject);
	}
} 