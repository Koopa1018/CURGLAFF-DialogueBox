using UnityEngine;
using Unity.Mathematics;

using System.Text;

namespace Clouds.UI.TextWriting {
	/// <summary>
	/// A String Builder which can skip over characters without permanently losing them.
	/// </summary>
	internal struct SyncedStringBuilder {
		string msg;
		uint msgPos;
		StringBuilder sb;
		/// <summary>
		/// How many spaces away the string builder's cursor is from the static-msg cursor.
		/// </summary>
		int stringBuilderDelta;

		public SyncedStringBuilder (string message) {
			msg = message;
			msgPos = 0;
			sb = new StringBuilder(message);
			stringBuilderDelta = 0;
		}

		public string message => msg;
		public int messagePos => (int)msgPos;

		public bool MoveNext () {
			msgPos = math.min(msgPos + 1, (uint)msg.Length);

			return !IsAtEnd;
		}

		public void Skip (int thisMany) {
			//If we're skipping 0 many, skip skipping.
			if (thisMany == 0) {
				return;
			}

			if (math.sign(thisMany) == 1) {
				SkipForward ((uint)thisMany);
			} else {
				SkipBackward((uint)-thisMany);
			}
		}

		public void SkipForward (uint thisMany) {
			//Find length to end.
			int toMsgEnd = msg.Length - (int)msgPos;
			//Find post-move length to end.
			int toEndPostMove = toMsgEnd - (int)thisMany;
			//If the length to end is negative, reduce move dist to fit exactly.
			thisMany = (uint)(math.min(toEndPostMove, 0) + (int)thisMany);

			//Remove the skipped amount of text from the StringBuilder.
			sb.Remove((int)msgPos + stringBuilderDelta, (int)thisMany);


			//Add to the message's position.
			msgPos += thisMany;
			//Update the cursor delta.
			stringBuilderDelta -= (int)thisMany;

			//SANITY CHECK: If we're too far out, spill our guts, then crash.
			if (msgPos > msg.Length) {
				SpillGuts();
				throw new System.IndexOutOfRangeException("Synced string builder became longer than its source message.");
			}

			//Debug.Log("Skipped forward " + thisMany + " characters.");
			//SpillGuts();
		}

		public void SkipBackward (uint thisMany) {
			//Reduce move dist to never underflow past 0.
			thisMany = math.min(thisMany+1,(uint)msgPos);

			sb.Insert((int)msgPos + stringBuilderDelta, msg.Substring((int)msgPos, (int)thisMany));
			
			//Subtract from the message's position.
			msgPos -= thisMany;
			//Update the cursor delta.
			stringBuilderDelta += (int)thisMany;

			//Debug.Log("Skipped backward " + thisMany + " characters.");
			//SpillGuts();
		}

		public char Current => msg[(int)msgPos];

		public bool IsAtEnd => msgPos >= msg.Length;

		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		void SpillGuts() {
			Debug.Log("Message is: " + msg);
			Debug.Log("Out message is: " + sb.ToString());
			Debug.Log("Position is: " + msgPos);
			Debug.Log("SBDelta is: " + stringBuilderDelta);
		}

		public override string ToString () {
			return sb.ToString();
		}
	}
}