using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

public class Deadzone : MonoBehaviour
{
    [SerializeField]
    private float expansionRate = 1.2f;
    [SerializeField]
    private Vector3 size = new Vector3(100, 100, 100);
    private BoxCollider collider = new BoxCollider();
    private float m_currentTime;
    private float m_waitTime = 200f;
    private ParticleSystem particleSystem;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, size);
    }
    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        particleSystem = GetComponent<ParticleSystem>();
        collider.size = size;
        EventSystem.instance.AddListener<OnFinishEvent>(UpdateExpansion);
    }

    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<OnFinishEvent>(UpdateExpansion);
    }
    private void Update()
    {
        m_currentTime += Time.deltaTime;
        if (m_currentTime >= m_waitTime)
        {
            m_currentTime = 0;
            m_waitTime -= 20f;
            size *= expansionRate;
        }

        //if (m_currentTime >= m_hungerDelpetionTime)
        //{
        //    m_currentTime = 0.0f;
        //    depletionCoEff += 0.1f;
        //}

        ////
        ////Decrease health
        //if (health > 0)
        //{
        //    health -= depletionCoEff * Time.deltaTime;
        //}
        var shape = particleSystem.shape;
        shape.scale = size;
        collider.size = size;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EventSystem.instance.RaiseEvent(new PlayerDie{ causeOfDeath = CausesOfDeath.Deadzones});
        }
    }

    private void UpdateExpansion(OnFinishEvent onFinishObjective)
    {
        expansionRate += 0.2f;
    }
}
