using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

[RequireComponent (typeof(CapsuleCollider))]
public class Anemone : MonoBehaviour
{
    Vector3 initScale;
    public float verticalAdjustment;
    // Start is called before the first frame update
    void Start()
    {
        initScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
//        Debug.Log("detecte collision");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Entered the anemone");
            EventSystem.instance.RaiseEvent(new PullCamera { closeToPlayer = false});
            transform.localScale *= 1.5f;
            transform.position = new Vector3(transform.position.x, transform.position.y - verticalAdjustment, transform.position.z);
            //transform.localScale *= 1.5f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EventSystem.instance.RaiseEvent(new PullCamera { closeToPlayer = true });
            transform.localScale = initScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + verticalAdjustment, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
