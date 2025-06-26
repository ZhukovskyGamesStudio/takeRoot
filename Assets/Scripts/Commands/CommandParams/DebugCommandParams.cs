using UnityEngine;

public class DebugCommandParams : ICommandParams {
	public string Text { get; set; }
	public float Wait { get; set; }
	public Vector2 At { get; set; }

	public DebugCommandParams(string text, float wait, Vector2 at) {
		Text = text;
		Wait = wait;
		At = at;
	}

}