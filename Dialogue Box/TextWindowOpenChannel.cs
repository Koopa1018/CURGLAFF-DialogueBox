using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Clouds.UI.DialogueBox {
	[CreateAssetMenu(menuName="Channels/Open Text Window")]
	public class TextWindowOpenChannel : ScriptableObject {
		System.Action onBoxOpened;
		System.Action onBoxClosed;
		System.Action onceOnBoxOpened;
		System.Action onceOnBoxClosed;

		public void SubscribeOpenPermanent (System.Action action) {
			onBoxOpened += action;
		}
		public void SubscribeClosedPermanent (System.Action action) {
			onBoxClosed += action;
		}
		public void SubscribeOpenTemporary (System.Action action) {
			onceOnBoxOpened += action;
		}
		public void SubscribeClosedTemporary (System.Action action) {
			onceOnBoxClosed += action;
		}

		public void OpenWindow () {
			onBoxOpened?.Invoke();
			
			onceOnBoxOpened?.Invoke();
			onceOnBoxOpened = null;
		}

		public void CloseWindow () {
			onBoxClosed?.Invoke();

			onceOnBoxClosed?.Invoke();
			onceOnBoxClosed = null;
		}
	}
}