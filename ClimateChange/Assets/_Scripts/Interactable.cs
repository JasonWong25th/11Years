using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;
using System;
using AmbientSounds;

public class CollectableEvent : GH.Event
{
    public string subtitleText;
    public AudioClip recording;
}

public class Interactable : MonoBehaviour {
    public float radius = 2.0f;
    [SerializeField]
    private string subtitleText;
    [SerializeField]
    private AudioClip recording;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            EventSystem.instance.RaiseEvent(new CollectableEvent()
            {
                subtitleText = this.subtitleText,
                recording = this.recording
            });
            AmbienceManager.ActivateEvent("LowerBackground");
            Destroy(gameObject);
        }

    }
}
