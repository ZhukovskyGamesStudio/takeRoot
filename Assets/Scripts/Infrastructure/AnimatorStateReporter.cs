using UnityEngine;

public class AnimatorStateReporter : StateMachineBehaviour {
	
	private IAnimationStateReader _stateReader;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		base.OnStateEnter(animator, stateInfo, layerIndex);
		FindReader(animator);

		_stateReader?.EnteredState(stateInfo.shortNameHash); //TODO: fix nre
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		FindReader(animator);

		_stateReader.ExitedState(stateInfo.shortNameHash); //TODO: fix nre
	}
	
	private void FindReader(Animator animator) {
		if (_stateReader != null) 
			return;
		
		_stateReader = animator.gameObject.GetComponent<IAnimationStateReader>();
	}
}