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
        public Webhook webhook;
        public GameObject text;
        private bool goodOrBad;
        public GameObject feedbackName;
        public bool hasSent = false;
        public void Bool(bool b)
        {
            goodOrBad = b;
        }
        public void Button()
        {
            if (!hasSent)
            {
                data.webhookName = feedbackName.GetComponent<TMP_InputField>().text;
                webhook = data.CreateWebhook();
                StartCoroutine(SendFeedback());
            }
            else
            {
                text.GetComponent<TMP_InputField>().text = "Je kan maar 1x feedback sturen";
            }
        }
        public IEnumerator SendFeedback()
        {
            hasSent = true;
            StartCoroutine(webhook.Send(text.GetComponent<TMP_InputField>().text));
            yield return new WaitForSeconds(1);
            switch (goodOrBad)
            {
                case true: StartCoroutine(webhook.Send(":thumbsup:")); break;
                case false: StartCoroutine(webhook.Send(":thumbsdown:")); break;
            }
        }
    }
}
