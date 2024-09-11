using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lumpn.Discord{
    public class Feedback : MonoBehaviour
    {
        public WebhookData data;
        private Webhook webhook;
        public GameObject text;
        private bool goodOrBad;
        public GameObject feedbackName;
        public void Bool(bool b)
        {
            goodOrBad = b;
        }
        public void Button()
        {
            data.webhookName = feedbackName.GetComponent<TMP_InputField>().text;
            webhook = data.CreateWebhook();
            StartCoroutine(SendFeedback());
        }
        public IEnumerator SendFeedback()
        {
            StartCoroutine(webhook.Send(text.GetComponent<TMP_InputField>().text));
            yield return new WaitForSeconds(1);
            switch (goodOrBad)
            {
                case true: StartCoroutine(webhook.Send("Goed")); break;
                case false: StartCoroutine(webhook.Send("Niet Goed")); break;
            }
        }
    }
}
