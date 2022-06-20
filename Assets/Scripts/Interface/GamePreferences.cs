using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePreferences : MonoBehaviour
{
    public static GamePreferences Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

}
