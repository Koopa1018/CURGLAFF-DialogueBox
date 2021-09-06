using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;

namespace Clouds.UI.TextWriting
{
	/// <summary>
	/// Component that outputs text into a TextMesh Pro text field.
	/// </summary>
	public class OutputToTMPro : MonoBehaviour, ITextOutput {
		[Tooltip("The text field to output into.")]
		[SerializeField] TMP_Text textField;

		public void Output (string text) {
			textField.text = text;
		}

		public void Append (string textSnippet) {
			textField.text += textSnippet;
		}
	}
	
}
