using UnityEngine;
using System.Text;
using System.Globalization; //used for NumberStyles (float parsing)

namespace Clouds.UI.TextWriting {
	public static class EscapeCodeUtils {
		static StringBuilder n;
		
		public static string readString (in string message, ref int position, int maxChars = 0) {
			position += 2;//Skip over character and starting bracket.

			//Write all bracketed number data into a substring
			if (maxChars > 0) {
				n = new StringBuilder(4, maxChars);
			} else {
				n = new StringBuilder(4);
			}
			for (; message[position] != ']'; position++) { //Continue iterating until end-of-number bracket
				//If we've hit the max length of the delay number,
				//print an error and exit the loop.
				if (maxChars > 0 && n.Length == n.MaxCapacity) {
					Debug.LogError(
"Attempted to pass more characters to the delay code than " + maxChars + " maximum."
					);
					break;
				}
				n.Append(message[position]);
			}
			//Jump over ending bracket.
			position++;

			return n.ToString();
		}

		public static float readNumber (in string message, ref int position, int maxChars = 0) {
			//Read in the string first.
			string readMeString = readString (in message, ref position, maxChars);

			//If the string is empty, return NaN.
			if (readMeString.Length == 0) {
				return float.NaN;
			}

			//If the string isn't empty, attempt to parse it out. (Exception on text found though.)
			return float.Parse(
				readMeString,
				NumberStyles.AllowDecimalPoint | 
				NumberStyles.AllowLeadingWhite |
				NumberStyles.AllowTrailingWhite
			);
		}
	}

	

}