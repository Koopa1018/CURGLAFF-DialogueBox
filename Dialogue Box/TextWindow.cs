using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Clouds.UI.DialogueBox
{
	public class TextWindow : MonoBehaviour {
		[SerializeField] UnityEvent open;
		[SerializeField] UnityEvent close;

		bool _windowIsOpen = false;
		public bool isOpen => _windowIsOpen;

		/// <summary>
		/// Opens the window, assuming it's closed. Nothing will happen if it's already open, of course.
		/// </summary>
		public void Open () {
			if (!_windowIsOpen) {
				open?.Invoke();
				_windowIsOpen = true;
			}
		}

		/// <summary>
		/// Closes the window, assuming it's open. Nothing will happen if it's already closed, of course.
		/// </summary>
		public void Close () {
			if (_windowIsOpen) {
				close?.Invoke();
				_windowIsOpen = false;
			}
		}

	}
}
