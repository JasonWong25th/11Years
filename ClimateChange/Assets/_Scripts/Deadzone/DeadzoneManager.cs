using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadzoneManager : MonoBehaviour
{
    [SerializeField]
    private int amt = 10;

    private GameObject[] deadzones;
    [SerializeField]
    private GameObject deadzone;

    public Transform[] spawnPositions;

    private void Start()
    {
        deadzones = new GameObject[amt];
        if (spawnPositions.Length < 2)
        {
            Debug.LogError("Need at least 2 Transforms");
        }
        else
        {
            Vector3 randPosition, randPosition1;
//            Debug.Log("The Deadzone Start method");
            for (int i = 0; i < amt; i++)
            {
                randPosition = spawnPositions[0].position;
                randPosition.x += Random.Range(-100, 100);
                randPosition.y += Random.Range(-100, 100);
                randPosition.z += Random.Range(-100, 100);
                randPosition1 = spawnPositions[1].position;
                randPosition1.x += Random.Range(-100, 100);
                randPosition1.y += Random.Range(-100, 100);
                randPosition.z += Random.Range(-100, 100);
                //Debug.Log(i);
                if (i % 2 == 0)
                {
                    deadzones[i] = Instantiate(deadzone, randPosition, Quaternion.identity, spawnPositions[0]);

                }
                else
                {
                    deadzones[i] = Instantiate(deadzone, randPosition1, Quaternion.identity, spawnPositions[1]);
                }
            }
        }
    }
}
