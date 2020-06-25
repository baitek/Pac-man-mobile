using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameMenu : MonoBehaviour {
    public Button start;

    public static int pacManLives;

    public static int pelletsConsumed;

    void Start()
    {
        start.onClick.AddListener(TaskOnClickStart);
    }
    void TaskOnClickStart()
    {
        pacManLives = 3;
        pelletsConsumed = 0;
        SceneManager.LoadScene("Level1");
    }
}
