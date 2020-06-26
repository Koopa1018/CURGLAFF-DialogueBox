namespace Clouds.UI.TextWriting
{
	/// <summary>
	/// When in field-by-field write mode, a command that implements this will result in pausing processing for a time.
	/// </summary>
	public interface IPauseOutputFlow : IMessageCommand {
		/// <summary>
		/// How long should this command pause processing for? (0 = use default time)
		/// </summary>
		float outputBreakTime {get;}
	}
}