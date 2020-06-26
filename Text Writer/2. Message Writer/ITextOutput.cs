using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Clouds.UI.TextWriting
{
	/// <summary>
	/// Implementors can be called upon to output messages to some final location.
	/// </summary>
	public interface ITextOutput {
		void Output (string text);
		void Append (string text);
	}
}