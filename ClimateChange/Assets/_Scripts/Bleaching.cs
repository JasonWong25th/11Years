using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

public class Bleaching : MonoBehaviour
{
    [SerializeField] [Range(0,40)]
    private float oceanAcidLimit = 0.0f;
    private bool m_bleached = false;
    private void Awake()
    {
        EventSystem.instance.AddListener<UpdateOceanAcidificationUI>(BleachTurnWhite);
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<UpdateOceanAcidificationUI>(BleachTurnWhite);

    }
    void BleachTurnWhite(UpdateOceanAcidificationUI acidification)
    {
        if (!m_bleached)
        {
            if (acidification.oceanAcidicfaction > oceanAcidLimit)
            {
                ChangeColor();
            }
        }

    }
    void ChangeColor()
    {
        //Attach to the parent of the renderer else comment this line out and uncomment line 
        Renderer[] renderer = gameObject.transform.GetComponentsInChildren<Renderer>();
        foreach(Renderer rend in renderer)
        {
            rend.material.shader = Shader.Find("Unlit/Color");
            rend.material.SetColor("_Color", Color.white);
        }


        //gameObject.transform.GetComponent<Renderer>().material.color = Color.white;
        m_bleached = true;
    }
}
