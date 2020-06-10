using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBoard : MonoBehaviour {

    private static int boardWidth = 28;
    private static int boardHeight = 36;

    private bool didStartDeath = false;
    private bool didStartConsumed = false;

    public int totalPellets = 0;
    public int score = 0;
    public int playerOneScore = 0;
    public int playerTwoScore = 0;
    public int pacManLives = 3;

    public bool isPlayerOneUp = true;

    public AudioClip backgroundAudioNormal;
    public AudioClip backgroundAudioFrightened;
    public AudioClip backgroundAudioDeath;
    public AudioClip consumedGhostAudioClip;

    public Text playerText;
    public Text readyText;

    public Text highScoreText;
    public Text playerOneUp;
    public Text playerTwoUp;
    public Text playerOneScoreText;
    public Text playerTwoScoreText;
    public Image playerLives2;
    public Image playerLives3;

    public Text consumedGhostScoreText;

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    void Start () {

        Object[] objects = GameObject.FindObjectsOfType (typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position;

            if (o.name != "PacMan" && o.name != "Nodes" && o.name != "NonNodes" && o.name != "Maze" && o.name != "Pellets" && o.tag != "Ghost" && o.tag != "ghostHome" && o.name != "Canvas" && o.name != "PlayerText" && o.name != "ReadyText" && o.tag != "UIElements")     
            {
                if(o.GetComponent<Tile>()!= null)
                {
                    if (o.GetComponent<Tile>().isPellet || o.GetComponent<Tile>().isSupperPellet) {
                        totalPellets++;
                    }
                }
                board[(int)pos.x, (int)pos.y] = o;
            }
            else
            {
            }
        }

        StartGame();
    }
	
    void Update()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        playerOneScoreText.text = playerOneScore.ToString();
        playerTwoScoreText.text = playerTwoScore.ToString();

        if (pacManLives == 3)
        {
            playerLives3.enabled = true;
            playerLives2.enabled = true;
        }else if (pacManLives == 2)
        {
            playerLives3.enabled = false;
            playerLives2.enabled = true;
        }
        else if (pacManLives == 1)
        {
            playerLives3.enabled = false;
            playerLives2.enabled = false;
        }
    }
    public void StartGame()
    {
        if (GameMenu.isOnePlayerGame)
        {
            playerTwoUp.GetComponent<Text>().enabled = false;
            playerTwoScoreText.GetComponent<Text>().enabled = false;
        }
        else
        {
            playerTwoUp.GetComponent<Text>().enabled = true;
            playerTwoScoreText.GetComponent<Text>().enabled = true;
        }

        if (isPlayerOneUp)
        {
            StartCoroutine(StartBlinking(playerOneUp));
        }
        else
        {
            StartCoroutine(StartBlinking(playerTwoUp));
        }
        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = false;
            ghost.transform.GetComponent<Ghost>().canMove = false;
        }
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;
        pacMan.transform.GetComponent<PacMan>().canMove = false;

        StartCoroutine(ShowObjectsAfter(2.25f));
    }
    public void StartConsumed(Ghost consumedGhost)
    {
        if (!didStartConsumed)
        {
            didStartConsumed = true;
            GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
            foreach(GameObject ghost in o)
            {

                ghost.transform.GetComponent<Ghost>().canMove = false;
            }
            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<PacMan>().canMove = false;
            pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;
            consumedGhost.transform.GetComponent<SpriteRenderer>().enabled = false;
            transform.GetComponent<AudioSource>().Stop();
            Vector2 pos = consumedGhost.transform.position;
            Vector2 viewPortPoint = Camera.main.WorldToViewportPoint(pos);

            consumedGhostScoreText.GetComponent<RectTransform>().anchorMin = viewPortPoint;
            consumedGhostScoreText.GetComponent<RectTransform>().anchorMax = viewPortPoint;

            consumedGhostScoreText.GetComponent<Text>().enabled = true;
            transform.GetComponent<AudioSource>().PlayOneShot(consumedGhostAudioClip);
            StartCoroutine(ProcessConsumedAfter(0.75f, consumedGhost));

        }
    }
    IEnumerator StartBlinking(Text blinkText)
    {
        yield return new WaitForSeconds(0.25f);

        blinkText.GetComponent<Text>().enabled = !blinkText.GetComponent<Text>().enabled;
        StartCoroutine(StartBlinking(blinkText));
    }

    IEnumerator ProcessConsumedAfter(float delay, Ghost consumedGhost)
    {
        yield return new WaitForSeconds(delay);
        consumedGhostScoreText.GetComponent<Text>().enabled = false;
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = true;
        consumedGhost.transform.GetComponent<SpriteRenderer>().enabled = true;
        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<Ghost>().canMove = true;
        }
        pacMan.transform.GetComponent<PacMan>().canMove = true;
        transform.GetComponent<AudioSource>().Play();
        didStartConsumed = false;
    }
    IEnumerator ShowObjectsAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = true;
        }
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = true;

        playerText.transform.GetComponent<Text>().enabled = false;
        StartCoroutine(StartGameAfter(2));
    }

    IEnumerator StartGameAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<Ghost>().canMove = true;
        }
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<PacMan>().canMove = true;

        readyText.transform.GetComponent<Text>().enabled = false;

        transform.GetComponent<AudioSource>().clip = backgroundAudioNormal;
        transform.GetComponent<AudioSource>().Play();
    }
    public void StartDeath()
    {

        if (!didStartDeath)
        {
            StopAllCoroutines();
            if (GameMenu.isOnePlayerGame)
            {
                playerOneUp.GetComponent<Text>().enabled = true;
            }
            else
            {
                playerOneUp.GetComponent<Text>().enabled = true;
                playerTwoUp.GetComponent<Text>().enabled = true;
            }
            didStartDeath = true;

            GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (GameObject ghost in o)
            {
                ghost.transform.GetComponent<Ghost>().canMove = false; 
            }
            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<PacMan>().canMove=false;

            pacMan.transform.GetComponent<Animator>().enabled = false;

            transform.GetComponent<AudioSource>().Stop();

            StartCoroutine(ProcessDeathAfter(2));
        }
    }

    IEnumerator ProcessDeathAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = false;
        }
        StartCoroutine(ProcessDeathAnimation(1.9f));

    }

    IEnumerator ProcessDeathAnimation(float delay)
    {
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.localScale = new Vector3(1, 1, 1);
        pacMan.transform.localRotation = Quaternion.Euler(0, 0, 0);

        pacMan.transform.GetComponent<Animator>().runtimeAnimatorController = pacMan.transform.GetComponent<PacMan>().deathAnimation;
        pacMan.transform.GetComponent<Animator>().enabled = true;

        transform.GetComponent<AudioSource>().clip = backgroundAudioDeath;
        transform.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(delay);

        StartCoroutine(ProcessRestart(1));
    }

    IEnumerator ProcessRestart(float delay)
    {
        pacManLives -= 1;

        if (pacManLives == 0)
        {
            playerText.transform.GetComponent<Text>().enabled = true;

            readyText.transform.GetComponent<Text>().text = "GAME OVER";
            readyText.transform.GetComponent<Text>().color = Color.red;

            readyText.transform.GetComponent<Text>().enabled = true;

            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

            transform.GetComponent<AudioSource>().Stop();

            StartCoroutine(ProcessGameOver(2));
        }
        else
        {
            playerText.transform.GetComponent<Text>().enabled = true;
            readyText.transform.GetComponent<Text>().enabled = true;

            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

            transform.GetComponent<AudioSource>().Stop();

            yield return new WaitForSeconds(delay);

            StartCoroutine(ProcessRestartShowObjects(1));
        }
    }

    IEnumerator ProcessGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("GameMenu");
    }
    IEnumerator ProcessRestartShowObjects(float delay)
    {
        playerText.transform.GetComponent<Text>().enabled = false;

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = true;
            ghost.transform.GetComponent<Ghost>().MoveToStartingPosition();
        }
        GameObject pacMan = GameObject.Find("PacMan");

        pacMan.transform.GetComponent<Animator>().enabled = false;
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = true;
        pacMan.transform.GetComponent<PacMan>().MoveToStartingPosition();

        yield return new WaitForSeconds(delay);

        Restart();

    }
    public void Restart()
    {
        readyText.transform.GetComponent<Text>().enabled = false;

        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<PacMan>().Restart();

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
        foreach(GameObject ghost in o)
        {
            ghost.transform.GetComponent<Ghost>().Restart();
        }

        transform.GetComponent<AudioSource>().clip = backgroundAudioNormal;
        transform.GetComponent<AudioSource>().Play();

        didStartDeath = false;
    }
}
