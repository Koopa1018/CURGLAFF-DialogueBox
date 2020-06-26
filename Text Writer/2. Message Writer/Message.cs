namespace Clouds.UI.TextWriting
{
	/// <summary>
	/// A message to be displayed by a TextWriter or some such.
	/// Contains text and instructions for displaying it bit-by-bit.
	/// </summary>
	public struct Message {
		/// <summary>
		/// The text of the message.
		/// This should be assumed to have no escape codes or macro invocations within it.
		/// </summary>
		public string text;
		/// <summary>
		/// The instructions for displaying the text bit-by-bit.
		/// Will usually contain mostly <see cref="WriteCommand"/> instances. 
		/// </summary>
		public IMessageCommand[] instructions;

		public static implicit operator Message ((string, IMessageCommand[]) src) {
			return new Message {
				text = src.Item1,
				instructions = src.Item2
			};
		}
	}
}