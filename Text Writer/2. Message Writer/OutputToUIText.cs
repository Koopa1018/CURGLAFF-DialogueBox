using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.UI;

namespace Clouds.UI.TextWriting
{
	/// <summary>
	/// Component that outputs text into a TextMesh Pro text field.
	/// </summary>
	public class OutputToUIText : MonoBehaviour, ITextOutput {
		[Tooltip("The text field to output into.")]
		[SerializeField] Text textField;

		public void Output (string text) {
			textField.text = text;
		}

		public void Append (string msg) {
			textField.text += msg;
		}
	}
	
}
