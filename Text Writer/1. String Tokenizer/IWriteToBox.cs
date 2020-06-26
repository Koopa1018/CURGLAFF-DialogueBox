namespace Clouds.UI.TextWriting
{
	/// <summary>
	/// Message commands which implement this will be able to write visual elements to the output box.
	/// </summary>
	public interface IWriteToBox : IMessageCommand {
		//No idea what goes here, but it probably starts something like this:
		
		/// <summary>
		/// A command that implements this will directly add a graphic or character to a message box.
		/// Altering existing graphics, e.g. portrait setting commands, also counts.
		/// </summary>
		/// <param name="msgPos">The position within the message that this command is invoked at.</param>
		/// <param name="message">The message to be written from.</param>
		/// <param name="outputter">The object which will be accepting the outputs.</param>
		/// <returns>How many characters were moved through, and what text was written.
		/// Characters moved through will be added to calling code's msgPos,
		/// and will be factored into initial message position with later calls to this function.
		/// If text was not written, return "".</returns>
		(int posDisplace, string writtenText) WriteOneIteration (int msgPos, in Message message, ITextOutput outputter);
	}
}