namespace Clouds.UI.TextWriting
{
	/// <summary>
	/// Message commands which implement this will be executed multiple times, rather than the default 1 time.
	/// </summary>
	public interface IRepeatingCommand : IMessageCommand {
		/// <summary>
		/// How many times should this command be run?
		/// </summary>
		uint repetitions {get;}
	}
}