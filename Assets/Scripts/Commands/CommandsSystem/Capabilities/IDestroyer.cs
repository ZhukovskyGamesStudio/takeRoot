using UnityEngine;

public interface IDestroyer {
	void StartHit(CommandTarget target);

	void Init(WorkerAnimator animator);
} 