using UnityEngine;

public class MoveCommandParams : ICommandParams{
	
	public CommandPerformer Performer {get; set;}
	public Vector2 TargetPosition { get; set; }

	public MoveCommandParams(CommandPerformer performer, Vector2 targetPosition) {
		Performer = performer;
		TargetPosition = targetPosition;
	}
}