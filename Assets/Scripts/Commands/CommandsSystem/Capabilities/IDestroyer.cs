using UnityEngine;

public interface IDestroyer : IPerformerComponent {
	void Hit(CommandTarget target);

	void Init(WorkerAnimator animator);
} 