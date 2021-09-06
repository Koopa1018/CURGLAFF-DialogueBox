using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Clouds.UI.DialogueBox {
	[CreateAssetMenu(menuName = "Channels/Message Queue")]
	public class MessageQueue : ScriptableObject {
		Queue<string> _queue = new Queue<string>(3);
		
		/// <summary>
		/// An action to be run when the queue ceases being empty. This will be run once, and then set to null.
		/// </summary>
		public System.Action onQueueStarted;
		/// <summary>
		/// An action to be run when the queue becomes empty. This will be run once, and then set to null.
		/// </summary>
		public System.Action onQueueEnded;
		
		/// <summary>
		/// Queues a message to be displayed. Does not display it; to show it, please call <see cref="TryOutputNext()"/>.
		/// </summary>
		/// <param name="messages">A raw, unprocessed message string (with escape codes, macros, etc. intact).</param>
		/// <returns>The current number of messages in the queue.</returns>
		public int AddMessage (string message) {
			_queue.Enqueue(message);

			//Start-queue event if this is the first in.
			if (_queue.Count == 1) {
				onQueueStarted?.Invoke();
				onQueueStarted = null;
			}

			return _queue.Count;
		}

		/// <summary>
		/// Queues messages to be displayed. Does not display them; to begin showing them, please call <see cref="TryOutputNext()"/>.
		/// </summary>
		/// <param name="messages">Raw, unprocessed message strings (with escape codes, macros, etc. intact).</param>
		/// <returns>The current number of messages in the queue.</returns>
		public int AddMessages (string[] messages) {
			for (int i = 0; i < messages.Length; i++) {
				_queue.Enqueue(messages[i]);
			}
			
			//Start-queue event if no others are here already.
			if (_queue.Count == messages.Length) {
				onQueueStarted?.Invoke();
				onQueueStarted = null;
			}

			return _queue.Count;
		}

		/// <summary>
		/// Pulls one message from the queue and returns it.
		/// </summary>
		/// <returns>The dequeued message.</returns>
		public string ReadMessage () {
			string returner = _queue.Dequeue();

			//End-queue event if this was the last element in.
			if (_queue.Count == 0) {
				onQueueEnded?.Invoke();
				onQueueEnded = null;
			}

			return returner;
		}

		public int Count => _queue.Count;

#if UNITY_EDITOR
		internal void ClearInEditor () {
			_queue.Clear();
		}
#endif

	}
}