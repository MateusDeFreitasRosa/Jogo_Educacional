using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    private DATA _callBetweenScenes;
    [SerializeField] private GameObject _objectCall;
    private void Start()
    {
        _callBetweenScenes = GameObject.Find("Communication_Scenes").GetComponent<DATA>();
        Debug.Log(_objectCall);
        var test = new DATA();
    }

   
    public void loadSinglePlayerMode()
    {
        Debug.Log("Loading Single Player Mode...");
        _callBetweenScenes.gameModeCoOp(false);
        DontDestroyOnLoad(_objectCall);
        SceneManager.LoadScene(1);
    }

    public void loadMultiPlayerMode()
    {
        Debug.Log("Loading Multi Player Mode");
        _callBetweenScenes.gameModeCoOp(true);
        DontDestroyOnLoad(_objectCall);
        SceneManager.LoadScene(2);
    }

    public void loadGameWithHistory()
    {
        Debug.Log("Load Game With History");
        _callBetweenScenes.gameModeCoOp(false);
        DontDestroyOnLoad(_objectCall);
        SceneManager.LoadScene(3);
    }
}
    