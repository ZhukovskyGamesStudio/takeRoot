using UnityEngine;

public interface IDestroyer {
	void Hit(CommandTarget target);
	bool CanHit();
	float GetHitCooldownRemaining();
	void SetHitDamage(float damage);
	void SetHitCooldown(float cooldown);
} 