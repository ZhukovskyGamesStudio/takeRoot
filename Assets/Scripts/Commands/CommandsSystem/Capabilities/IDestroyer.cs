using UnityEngine;

public interface IDestroyer : IPerformerComponent {
	void Hit(CommandTarget target);
} 