using AmbientSounds;
using UnityEngine;

public class NarrationTrigger : MonoBehaviour
{
    public AudioClip audioTrigger;
    private AudioSource otherSource;
    public AudioSource audioSource;
    private void Awake()
    {
        otherSource = Audiomanager.Instance.NarrationAS();
        audioSource = this.GetComponent<AudioSource>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Entered DeadZone Narration");
            otherSource.Pause();
            audioSource.clip = audioTrigger;
            audioSource.Play();
            AmbienceManager.ActivateEvent("LowerBackground");
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            otherSource.UnPause();
            audioSource.Pause();
            AmbienceManager.DeactivateEvent("LowerBackground");
        }

    }
}
