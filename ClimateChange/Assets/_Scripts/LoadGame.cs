using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public GameObject panel;
    public GameObject panelShadow;
    public GameObject loadCircle;

    private bool startLoad = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startLoad)
        {
            StartCoroutine("LoadYourAsyncScene");
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(2);
    }
    public void AsyncLoadScene()
    {
        startLoad = true;
    }

    IEnumerator LoadYourAsyncScene()
    {
        bool sceneLoaded;
        panel.SetActive(false);
        panelShadow.SetActive(false);
        loadCircle.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            
            yield return null;
        }

        if (asyncLoad.isDone)
        {
            yield return new WaitForSeconds(3);
            asyncLoad.allowSceneActivation = true;
        }
    }
}
