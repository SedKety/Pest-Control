using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lumpn.Discord{
    public class Feedback : MonoBehaviour
    {
        public WebhookData data;
        public Webhook webhook;
        public GameObject text;
        private int face;
        public GameObject feedbackName;
        public bool hasSent = false;


        public void Review(int i)
        {
            face = i;
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
            if (face == 0) StopCoroutine(SendFeedback());
            hasSent = true;
            StartCoroutine(webhook.Send(text.GetComponent<TMP_InputField>().text));
            yield return new WaitForSeconds(1);
            switch (face)
            {
                case 1: StartCoroutine(webhook.Send(":smiley:")); break;
                case 2: StartCoroutine(webhook.Send("<:face:1287754864499359916>")); break;
                case 3: StartCoroutine(webhook.Send(":sob:")); break;
                default: Debug.Log("No review selected."); break;
            }
        }
    }
}
