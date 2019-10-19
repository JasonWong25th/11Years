using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GH;
public class ToggleDangerUI:GH.Event{
    public bool value;
}
public class DangerOverlay : MonoBehaviour
{
    public Image Overlay;
    private void Awake()
    {
        Overlay = gameObject.GetComponent<Image>();
        EventSystem.instance.AddListener<ToggleDangerUI>(ToggleDangerOverlay);
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<ToggleDangerUI>(ToggleDangerOverlay);
    }
    private void Start()
    {
        Overlay.enabled = false;
    }
    private void ToggleDangerOverlay(ToggleDangerUI toggle)
    {
        Overlay.enabled = toggle.value;
    }
}
