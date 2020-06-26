using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;

namespace Clouds.UI.TextWriting
{
	/// <summary>
	/// Options for what to do when an invalid escape code is found.
	/// </summary>
	public enum BadEscapeCodeResponse {Skip = 0, Print, ThrowException}

	public abstract class EscapeCodeResponder : ScriptableObject {
		[SerializeField] BadEscapeCodeResponse _badEscapeCodeResponse;

		public abstract char invokeMacroCharacter {get;}
		public abstract char invokeFunctionCharacter {get;}
		public abstract bool customHandleEscapeInvokers {get;}

		public BadEscapeCodeResponse badEscapeCodeResponse {get => _badEscapeCodeResponse;}

		//bool IsThisAnEscapeSequence (string sequence);

		//bool IsThisAnEscapeSequence (string sequence);

		//Gee walla-wanca, it's a good thing this method isn't used for security critical things,
		//otherwise it could be dangerous!
		//(If an attacker can jump to it from somewhere else...then?)
		/// <summary>
		/// Attempts to parse and evaluate an escape code at the given position within a message.
		/// Unless this.customHandleEscapeInvokers == true, the escape invoker character will already have been skipped over.
		/// </summary>
		/// <param name="message">The message to check.</param>
		/// <param name="currentPosition">The position of the cursor when a command is located.
		/// Unless this.customHandleEscapeInvokers == true, the escape invoker character will already have been skipped over.</param>
		/// <param name="nextWrite">The write command that will come after this command. Now's your chance to modify it!</param>
		/// <returns>A message command and a number of characters skipped, if the code is valid; null and 0 otherwise.</returns>
		public abstract (IMessageCommand, int) EvaluateEscapeCode (
			in string message,
			int currentPosition,
			out WriteCommand nextWrite
		);
	}
}