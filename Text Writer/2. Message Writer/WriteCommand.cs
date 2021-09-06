using UnityEngine;
using Unity.Mathematics;

namespace Clouds.UI.TextWriting
{
	/// <summary>
	/// Command that writes a specific number of character fields to the box.
	/// (character + Unicode-style diacritics = 1 character field; should account for languages like Vietnamese)
	/// </summary>
	public struct WriteCommand : IMessageCommand, IRepeatingCommand, IPauseOutputFlow, IWriteToBox {
		/// <summary>
		/// How many character fields will be written?
		/// (character + Unicode-style diacritics = 1 character field; should account for languages like Vietnamese)
		/// </summary>
		public uint Length;

		/// <summary>
		/// What color should this sequence of characters be written in?
		/// </summary>
		public float4 color;

		/// <summary>
		/// WriteCommand repeats once for each written character field.
		/// </summary>
		public uint repetitions => Length;

		/// <summary>
		/// WriteCommand will break for the default amount of time.
		/// </summary>
		public float outputBreakTime => 0;

		/// <summary>
		/// WriteCommand does directly add graphics to the display box.
		/// </summary>
		public bool writesDirectlyToBox => true;

		public void Lengthen (uint howMany = 1) {
			Length += howMany;
		}

		public (int, string) WriteOneIteration (int msgPos, in Message message, ITextOutput outputter) {
			//To store the characters we want to add to the message this round.
			System.Text.StringBuilder currentCharField = new System.Text.StringBuilder(1); /*(langProfile.maxCharsPerField);*/

			int distanceWeMoved = 0;

			//Add characters into the current field until we hit a non-diacritic.
			do {
				//@TODO: What to do when somebody puts diacritics on an escape code?
				//Add them onto the previous character afterwards.
				//Except, obvs parse'm if they're in the escape sequence.

				//Append the current character.
				currentCharField.Append(message.text[msgPos + distanceWeMoved]);

				//Bump the read head forward so we get the next character next time.
				distanceWeMoved++;

				//Break loop if we've just hit the end.
				if (msgPos + distanceWeMoved >= message.text.Length) {
					break;
				}
			}
			//Language profiles not implemented, English only (so no diacritics to handle).
			while (false); /*(!langProfile.nonDiacritics.Contains(message.text[msgPos]));*/

			//Convert current charfield to a workable string for now.
			string charFieldOut = currentCharField.ToString();

			//Add the charfield to the message. (No use StringBuilderizing this--just gonna become a string anyway.)
			outputter.Append(charFieldOut);

			return (distanceWeMoved, charFieldOut);
			
			//Consume current charfield, since we just used it.
			//currentCharField.Clear();
		}
	}
}