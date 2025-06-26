public class DebugCommandParams : ICommandParams {
	public string Text { get; set; }
	public float Wait { get; set; }

	public DebugCommandParams(string text, float wait) {
		Text = text;
		Wait = wait;
	}
}