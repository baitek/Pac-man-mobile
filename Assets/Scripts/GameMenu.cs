using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameMenu : MonoBehaviour {
    public Button start;

    public static int pacManLives =3;

    public static int pelletsConsumed = 0;

    void Start()
    {
        start.onClick.AddListener(TaskOnClickStart);
    }
    void TaskOnClickStart()
    {
        SceneManager.LoadScene("Level1");
    }
}
