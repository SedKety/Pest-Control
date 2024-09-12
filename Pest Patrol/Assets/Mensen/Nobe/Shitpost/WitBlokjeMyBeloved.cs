using Lumpn.Discord;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitBlokjeMyBeloved : MonoBehaviour
{
    public Webhook hook;
    public WebhookData data;
    public string shitpost;
    public void Start()
    {
        hook = data.CreateWebhook();
        StartCoroutine(hook.Send(shitpost));
    }

}
