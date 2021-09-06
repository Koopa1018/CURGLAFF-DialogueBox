using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Clouds.UI.DialogueBox {
	public class MessageQueueReader : MonoBehaviour  {
		[SerializeField] MessageQueue _queue;
		[SerializeField] TextWindowOpenChannel windowOpenerChannel;
		[SerializeField] TextWindow window;
		[SerializeField] MessageDisplayer messagePipeStart;

		void Awake () {
			windowOpenerChannel.SubscribeOpenPermanent(TryOutputNext);
		}

		/// <summary>
		/// Outputs one message from the queue. If the outputter is closed, open it; if the message queue is empty, close the outputter.
		/// </summary>
		/// <returns>The number of messages remaining in the queue.</returns>
		public void TryOutputNext () {
			if (_queue.Count > 0) {
				window.Open();
				messagePipeStart.Display(_queue.ReadMessage());
			} else {
				window.Close();
				windowOpenerChannel.CloseWindow();
			}
			
			//return _queue.Count;
		}

	}
}