using UnityEngine;

public interface ISearcher {
	void Search(CommandTarget target);
	bool CanSearch();
	float GetSearchCooldownRemaining();
	void SetSearchSpeed(float speed);
	void SetSearchCooldown(float cooldown);
} 