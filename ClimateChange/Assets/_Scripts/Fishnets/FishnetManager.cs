using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

public class FishnetManager : MonoBehaviour
{
    private float m_currentTime;

    private bool timerStarted = false;
    private float m_waitTime = 100;
    private float m_spawnTime = 2;

    public GameObject posistionOne =null;
    public GameObject posistionTwice = null;
    public GameObject posistionThird = null;
    public GameObject posistionFourth = null;

    private List<GameObject> fishnets = new List<GameObject>();
    [SerializeField]
    private Vector3 volume;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(posistionOne.transform.position, volume);
        Gizmos.DrawWireCube(posistionTwice.transform.position, volume);
        Gizmos.DrawWireCube(posistionThird.transform.position, volume);
        Gizmos.DrawWireCube(posistionFourth.transform.position, volume);
    }

    private void Update()
    {
        if (timerStarted)
        {
            m_currentTime += Time.deltaTime;
            if (m_currentTime > m_waitTime)
            {
                timerStarted = false;
                m_currentTime = 0;
            }
        }
        else
        {
            m_currentTime += Time.deltaTime;
            if (m_currentTime > m_spawnTime)
            {
                timerStarted = true;
                m_currentTime = 0;
            }
        }
        //Ideally we use the event system but that's tough
        if (!timerStarted)
        {
//            Debug.Log("Spawning");
            GameObject objectSpawned = ObjectPooler.Instance.SpawnFromPool("Fishnets", posistionOne.transform.position, Quaternion.Euler(0f, 0f, 90f), volume);
            objectSpawned.transform.SetParent(posistionOne.transform);
            GameObject objectSpawned2 = ObjectPooler.Instance.SpawnFromPool("Fishnets", posistionTwice.transform.position, Quaternion.Euler(0f, 0f, 90f), volume);
            objectSpawned2.transform.SetParent(posistionTwice.transform);
            GameObject objectSpawned3 = ObjectPooler.Instance.SpawnFromPool("Fishnets", posistionThird.transform.position, Quaternion.Euler(0f, 0f, 90f), volume);
            objectSpawned3.transform.SetParent(posistionThird.transform);

            GameObject objectSpawned4 = ObjectPooler.Instance.SpawnFromPool("Fishnets", posistionFourth.transform.position, Quaternion.Euler(0f, 0f, 90f), volume);
            objectSpawned4.transform.SetParent(posistionFourth.transform);
            fishnets.Add(objectSpawned);
            fishnets.Add(objectSpawned2);
            fishnets.Add(objectSpawned3);
            fishnets.Add(objectSpawned4);

        }
    }

}
