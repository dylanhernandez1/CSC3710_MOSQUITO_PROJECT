using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        // Singleton setup
        if (Instance)
            DestroyImmediate(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    /// <summary>
    /// Run to load the game from the first level
    /// </summary>
    public void OnPlayButtonPressed() => SwitchScenes("Level 1");

    /// <summary>
    /// Abstraction for loading levels
    /// </summary>
    public void SwitchScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        Debug.Log("Loading scene: " + sceneName);
    }
}
