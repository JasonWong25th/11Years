﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

public class Fishnet : MonoBehaviour
{
    private Renderer m_fishnetRenderer = null;
    public readonly float rangeDistance = 100;
    private float opacity = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, rangeDistance);
    }
    private void OnEnable()
    {
        EventSystem.instance.AddListener<PlayerPosition>(ChangeOpacity);
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<PlayerPosition>(ChangeOpacity);
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(m_fishnetRenderer == null)
        {
            m_fishnetRenderer = this.gameObject.GetComponent<MeshRenderer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Do stuff
        //Enter a button mashing part of the game
        Debug.Log("Got caught");
        EventSystem.instance.RaiseEvent(new PlayerDie() { causeOfDeath = CausesOfDeath.Fishnets});
    }

    void ChangeOpacity(PlayerPosition playerPosition)
    {
        Vector3 playerpos = playerPosition.position;
        Color fishnetColor = m_fishnetRenderer.material.color;
        if (Vector3.Distance(playerpos, this.transform.position) > rangeDistance)
        {
            opacity = 0;
        }
        else if(Vector3.Distance(playerpos, this.transform.position)>rangeDistance *0.75f)
        {
            opacity = 0.25f;
        }else if(Vector3.Distance(playerpos, this.transform.position)>rangeDistance *0.5f)
        {
            opacity = 0.50f;
        }else if(Vector3.Distance(playerpos, this.transform.position)>rangeDistance *0.25f)
        {
            opacity = 0.75f;
        }
        else
        {
            opacity = 1;
        }
        fishnetColor.a = opacity;
        m_fishnetRenderer.material.SetColor("_Color", fishnetColor);
        //Vector3.Distance(playerpos, this.transform.position)/rangeDistance;
        //Debug.Log(fishnetColor.a);
    }

 }
