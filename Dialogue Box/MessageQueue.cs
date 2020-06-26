using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Clouds.UI.DialogueBox {
	public class MessageQueue : MonoBehaviour  {
		[SerializeField] TextWindow window;
		[SerializeField] MessageDisplayer messagePipeStart;
		
		public System.Action onQueueEnded;

		Queue<string> _queue = new Queue<string>(3);

		/// <summary>
		/// Queues a message to be displayed. Does not display it; to show it, please call <see cref="TryOutputNext()"/>.
		/// </summary>
		/// <param name="messages">A raw, unprocessed message string (with escape codes, macros, etc. intact).</param>
		/// <returns>The current number of messages in the queue.</returns>
		public int AddMessage (string message) {
			_queue.Enqueue(message);
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
			return _queue.Count;
		}

		/// <summary>
		/// Outputs one message from the queue. If the outputter is closed, open it; if the message queue is empty, close the outputter.
		/// </summary>
		/// <returns>The number of messages remaining in the queue.</returns>
		public void TryOutputNext () {
			if (_queue.Count > 0) {
				window.Open();
				messagePipeStart.Display(_queue.Dequeue());
			} else {
				window.Close();
				onQueueEnded?.Invoke();
				onQueueEnded = null;
			}
			
			//return _queue.Count;
		}

	}
}