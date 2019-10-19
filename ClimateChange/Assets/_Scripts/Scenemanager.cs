using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GH;

public class Pause: GH.Event{


}

public class Restart: GH.Event
{

}

public class Scenemanager : MonoBehaviour
{
    #region Singleton
    private static Scenemanager _instance;
    public static Scenemanager Instance //Ensures that this is the only instance in the class
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Scenemanager();
            }
            return _instance;
        }
    }
    #endregion

    public int sceneIndex;

    private void Awake()
    {
        EventSystem.instance.AddListener<CollectableEvent>(pauseTime);
        EventSystem.instance.AddListener<Restart>(RestartGame);
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<CollectableEvent>(pauseTime);

    }

    private void RestartGame(Restart restart)
    {
        SceneManager.LoadScene(0);
        sceneIndex = 1;
    }

    private void pauseTime(CollectableEvent collectableEvent)
    {
        //float curTime = 0;
        //while (curTime < collectableEvent.recording.length)
        //{
        //    curTime += Time.deltaTime;
        //    Time.timeScale = 0;
        //}
        //Time.timeScale = 1;
    }
    public void NextLevel()
    {
        int temp = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("LoadNextScene");
        SceneManager.LoadScene(temp);
        sceneIndex++;
    }
}
