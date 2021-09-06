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
		public void UnsubscribeOpenPermanent (System.Action action) {
			onBoxOpened -= action;
		}
		public void UnsubscribeClosedPermanent (System.Action action) {
			onBoxClosed -= action;
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
#if UNITY_EDITOR
		//This state persists past playmode end--so we need to clear it ourselves!~
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		void DoClearOnPlayEnd () {
			UnityEditor.EditorApplication.playModeStateChanged += ClearOnPlayEnd;
		}

		void ClearOnPlayEnd (UnityEditor.PlayModeStateChange state) {
			if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode) {
				onBoxOpened = onBoxClosed = onceOnBoxClosed = onceOnBoxOpened = null;
			}
			UnityEditor.EditorApplication.playModeStateChanged -= ClearOnPlayEnd;
		}
#endif
	}
}