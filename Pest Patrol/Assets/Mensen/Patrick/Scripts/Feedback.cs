using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace Lumpn.Discord{
    public class Feedback : MonoBehaviour
    {
        public WebhookData data;
        public Webhook webhook;
        public GameObject text;
        private int face;
        public GameObject feedbackName;
        private bool hasSent = false;
        private List<string> filterWords = new();
        public float scaleFactor;
        private GameObject currentButton;
        public GameObject sentText;
        
        public void Review(int i)
        {
            if (currentButton != null)
            {
                if (currentButton != EventSystem.current.currentSelectedGameObject)
                {
                    currentButton.transform.localScale /= scaleFactor;
                }
            }
            if (currentButton == EventSystem.current.currentSelectedGameObject) return;
            currentButton = EventSystem.current.currentSelectedGameObject;
            currentButton.transform.localScale *= scaleFactor;
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
                text.GetComponent<TMP_InputField>().text = "You can only send feedback once.";
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
                default: print("No review selected."); break;
            }
            sentText.SetActive(true);
        } 
    }
}
