using System.Collections.Generic;

namespace Clouds.UI.TextWriting {
	public static class MessageTokenizer {
		public static Message TokenizeString (
			string message,
			EscapeCodeResponder cResp//,
			//LanguageDefinition languageDefinition
		) {
//			Debug.Log("Message pre-macros: " + message);
			//For the time being, begin by expanding the string's macros.
			message = Clouds.Strings.MacroExpander.EvaluateMacros(
				message, 	// The message to evaluate macros in--fairly self-explanatory
				null,		// Macro dictionary--null is fine, just does nothing
				cResp.invokeMacroCharacter //Char to invoke macros
			);
//			Debug.Log("Message post-macros: " + message);

			//Copy the message into a StringBuilder for efficient parsing.
			SyncedStringBuilder builder = new SyncedStringBuilder(message);
			//Create a list to contain the cakkalated message commands.
			List<IMessageCommand> cmds = new List<IMessageCommand>(2);

			//Create a new WriteCommand.
			WriteCommand write = new WriteCommand();

			for (; !builder.IsAtEnd; builder.MoveNext()) {
				//If the current character is the escape-invocation character....
				if (builder.Current == cResp.invokeFunctionCharacter) {
					if (!cResp.customHandleEscapeInvokers) {
						/*Debug.Log( string.Format(
@"Tokenizer skipping over escape char.
Position is: {0}
Message is: {1}
Current char is: {2}", builder.messagePos, builder.ToString(), builder.Current));*/
						//Increment the read head by one extra, to position us at the code.
						builder.SkipForward(1);
						/*Debug.Log( string.Format(
@"Tokenizer done skipping.
Position is: {0}
Message is: {1}
Current char is: {2}", builder.messagePos, builder.ToString(), builder.Current));*/
					}

					//Check: is this character also the escape character?
					if (builder.Current != cResp.invokeFunctionCharacter) {
						//If not, we're in an escape code.

						//Commit the current write code.
						cmds.Add(write);

						//Eval. the escape code to find out how far forward to skip.
						int skipAmt = EvalEscapeCode(cResp, builder.message, builder.messagePos, cmds, out write);

						//Jump to next chars to be evaluated.
						builder.Skip(skipAmt);
						//Debug.Log("Escape eval finished.\nPosition is: " + builder.messagePos + "\ntext is: " + builder.ToString());
						continue;
					}
					//If the escape character was reduplicated, we shall fall through to...
				}
				//Normal character processing

				//It's simple...just add one to WriteCommand's length.
				write.Lengthen();
			}

			//Commit the final write code, if any.
			if (write.Length > 0) {
				cmds.Add(write);
			}

			//Convert our workspace variables into solid forms for reading!
			return (builder.ToString(), cmds.ToArray());
		}

		static int EvalEscapeCode(
			EscapeCodeResponder cResp,
			string message,
			int messagePos,
			List<IMessageCommand> cmds,
			out WriteCommand write
		) {
			//Have code responder parse the following sequence.
			(IMessageCommand evalCode, int skipDist) = cResp.EvaluateEscapeCode(message, messagePos, out write);

			//If the evaluated code is invalid, respond as the parser demands.
			if (evalCode == null) {
				switch (cResp.badEscapeCodeResponse) {
					case BadEscapeCodeResponse.Skip:
						//Parser demands we skip printing the code.
						//We've already set the skip distance past the code,
						//so let's just let it be.
						break;
					case BadEscapeCodeResponse.Print:
						//Parser demands we print the code, implicitly including the invocation character.
						if (!cResp.customHandleEscapeInvokers) {
							//Skip back to include the slash, rather'n forward past the code.
							skipDist = -1;
						}
						break;
					case BadEscapeCodeResponse.ThrowException:
						//Parser demands we panic!
						throw new System.InvalidOperationException(
@"Attempted to parse a bad escape code."
						);
				}
			}
			//Otherwise, commit it.
			else {
				//Commit the code into the list.
				cmds.Add(evalCode);
			}

			//Tell the outputter where to skip the cursor to.
			return skipDist;
		}
	}
}
