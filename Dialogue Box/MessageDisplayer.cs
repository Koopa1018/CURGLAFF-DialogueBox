using UnityEngine;

using Clouds.UI.TextWriting;

namespace Clouds.UI.DialogueBox {
	/// <summary>
	/// Component that starts a message being written to the screen.
	/// </summary>
	public class MessageDisplayer : MonoBehaviour {
		[Tooltip("The escape code responder detects and processed escape codes in passed messages.")]
		[SerializeField] EscapeCodeResponder codeResponder;
		[Tooltip("The Text Writer handles the actual process of writing the message to an output of some kind.")]
		[SerializeField] TextWriter writer;
		
		/// <summary>
		/// Begins a message writing on the screen, assuming this.writer is set.
		/// </summary>
		/// <param name="message">The message to be displayed. Self-explanatory, really.</param>
		public void Display (string message) {
			Message tokenized = MessageTokenizer.TokenizeString(message, codeResponder);
			writer.BeginWrite(tokenized);
		}
	}
}