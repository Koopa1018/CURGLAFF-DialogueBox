using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Clouds.UI.TextWriting
{
	/// <summary>
	/// Component that performs a one-character-field*-at-a-time text writing sequence
	/// and outputs the results to a component implementing <see cref="ITextOutput"/>.\n
	/// * Character field = one character and any diacritics modifying it.
	/// </summary>
	[RequireComponent(typeof(ITextOutput))]
	public class TextWriter : MonoBehaviour {
		delegate void CommandResult(IMessageCommand runCommand, ref int msgPos, Message message, ITextOutput outputter);

		[System.Serializable]
		public class TextOutEvent : UnityEvent<string> {}

		//================//
		// * Public fields
		//================//
		[Tooltip("[TEMPORARY, MOVE TO PRESET ASSET] How long to spend (by default) between typing each character.")]
		[SerializeField] float timeBetweenLetterTypes = 0.015f;

		[Tooltip("What to do when a character is written to the output.")]
		[SerializeField] TextOutEvent onCharacterWrite = new TextOutEvent();

		[Tooltip("What to do when user input is requested, either at the start or the end of a message.")]
		[SerializeField] UnityEvent promptingInput = new UnityEvent();

		//@TODO: When Unity supports this, make this a serialized field instead of a Getted Component.
		[Tooltip("The component that will be responsible for actually displaying the text.")]
		/*[SerializeReference]*/ ITextOutput outputter;
		void Start () {
			outputter = GetComponent<ITextOutput>();
		}

		//================//
		// * Private data
		//================//
		WaitForSecondsRealtime _typerWait;

		//================//
		// ** Public Methods
		//================//
		///<summary>
		/// Begins writing a message out to a text object.
		/// </summary>
		/// <param name="message">The message text and bundled instructions to use in the writing process.</param>
		public void BeginWrite (in Message message) {
			initializeText();
			StartCoroutine(writeDirect(message, timeBetweenLetterTypes));									
		}

		/// <summary>
		/// Set the time taken between each character written (barring explicitly-defined pauses).
		/// </summary>
		/// <param name="secondsPerCharField">How much time should a single character field/graphic wait to appear after the last one?</param>
		public void SetTypeSpeed (float secondsPerCharField) {
			//Find the time the write-pause command pauses. (0 = default, remember).
			float waitTime = secondsPerCharField == 0 ? timeBetweenLetterTypes : secondsPerCharField;

			//Lazy init the wait command.
			if (_typerWait == null) {
				_typerWait = new WaitForSecondsRealtime(waitTime);
			} else {
				_typerWait.waitTime = waitTime;
			}
		}

		//================//
		// ** Private Methods
		//================//
		void initializeText() {
			outputter.Output(System.String.Empty);
		}

		uint cmdRepetitions (IMessageCommand cmd) {
			IRepeatingCommand rpt = cmd as IRepeatingCommand;
			return rpt == null ? 1 : rpt.repetitions;
		}

		IEnumerator writeDirect (Message message, float typeTime) {
			//To keep track of how many characters into the message we've gone.
			int msgPos = 0;
			//To store the characters we want to add to the message this round.
			System.Text.StringBuilder currentCharField = new System.Text.StringBuilder(1); /*(langProfile.maxCharsPerField);*/

			//To store our current write speed.
			WaitForSecondsRealtime typerWait = new WaitForSecondsRealtime(typeTime);

			CommandResult commandResult;

			//Process each message.
			foreach (IMessageCommand cmd in message.instructions) {
				//If it can convert into a write command.
				if (cmd as IWriteToBox != null ) {
					//Convert it into a WriteCommand.
					var writeCmd = (IWriteToBox)cmd;

					//Debug.Log("Write command of length: " + cmdRepetitions(cmd));

					// //TMPro hack: insert an HTML code of the text's color.
					// onCharacterWrite?.Invoke(string.Format(
					// 	"<#{0}>",
					// 	ColorUtility.ToHtmlStringRGB(
					// 		new Color(
					// 			writeCmd.color.x,
					// 			writeCmd.color.y,
					// 			writeCmd.color.z,
					// 			writeCmd.color.w
					// 		)
					// 	)
					// ) );

					commandResult = (IMessageCommand c, ref int mpos, Message msg, ITextOutput outp) => {
						(int posDisplacement, string charField) = writeCmd.WriteOneIteration(mpos, msg, outp);
						//Displace currently read message position.
						msgPos += posDisplacement;
						//Send current charfield to subscribers.
						onCharacterWrite?.Invoke(charField);
					};
				}

				//It's not a write command. Do something about it.
				else {
					//Pass it forward to a responder.
					//Would like this to be like, "list of type/event pairs" to the viewer using the editor.
					//Can't surely pull that off though, yet. Can't learn if I don't try it, yeah?

					//Run function not implemented. Spill escape code's contents to console.
					commandResult = (IMessageCommand c, ref int mpos, Message msg, ITextOutput outp) => {
						Debug.Log("Found escape code.");
						var pauseCmd = c as IPauseOutputFlow;
						if (pauseCmd != null) {
							Debug.LogFormat(
								"Code will break output flow for {0} seconds.",
								pauseCmd.outputBreakTime == 0 ? "the default number of" : pauseCmd.outputBreakTime.ToString()
							);
						}
						var writeCmd = c as IWriteToBox;
						if (writeCmd != null) {
							Debug.Log("Code writes directly to box.");
						}
						uint repeats = cmdRepetitions(cmd);
						if (repeats != 1) {
							Debug.LogFormat("Code will be executed {0} times.", repeats);
						}
					};
				}

				var repetitions = cmdRepetitions(cmd);

				//Run the command as many times as it's designed to.
				for (uint i = 0; i < repetitions; i++) {
					//Run the command.
					commandResult.Invoke(cmd, ref msgPos, message, outputter);

					IPauseOutputFlow pause = cmd as IPauseOutputFlow;

					//Break output flow for a little bit if we're supposed to.
					if (pause != null) {
						//Use a preconstructed wait cmd unless time is unusual.
						yield return pause.outputBreakTime == 0 ? typerWait : new WaitForSecondsRealtime(pause.outputBreakTime);
					}
				}
			}
		
			//Prompt for user input.
			promptingInput?.Invoke();
		}
		
	}
}