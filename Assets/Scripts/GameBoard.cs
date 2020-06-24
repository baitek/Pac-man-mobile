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

    public static int level = 1;

    public int totalPellets = 0;
    public static int score = 0;

    public static int ghostConsumedRunningScore;

    public bool shouldBlink = false;

    public float blinkIntervalTime = 0.1f;
    private float blinkIntervalTimer = 0;

    public AudioClip backgroundAudioNormal;
    public AudioClip backgroundAudioFrightened;
    public AudioClip backgroundAudioDeath;
    public AudioClip consumedGhostAudioClip;

    public Sprite mazeBlue;
    public Sprite mazeWhite;

    public Text playerText;
    public Text readyText;

    public Text highScoreText;  
    public Text playerScoreText;
    public Image playerLives2;
    public Image playerLives3;

    public Text consumedGhostScoreText;

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    public Image[] levelImages;

    private bool didIncrementLevel = false;

    bool didSpawnBonusItem1;
    bool didSpawnBonusItem2;

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

            if (level == 1)
            {
                GetComponent<AudioSource>().Play();
            }
      
        
        StartGame();
    }
	
    void Update()
    {
        UpdateUI();

        CheckPelletsConsumed();

        CheckShouldBlink();

        BonusItems();
    }

    void BonusItems()
    {

            SpawnBonusItemForPlayer();
    }

    void SpawnBonusItemForPlayer()
    {

            if(GameMenu.pelletsConsumed >= 70 && GameMenu.pelletsConsumed < 170)
            {
                if (!didSpawnBonusItem1)
                {
                    didSpawnBonusItem1 = true;
                    SpawnBonusItemForLevel(level);
                }
            }else if (GameMenu.pelletsConsumed >= 170)
            {
                if (!didSpawnBonusItem2)
                {
                    didSpawnBonusItem2 = true;
                    SpawnBonusItemForLevel(level);
                }
            }       

    }

    void SpawnBonusItemForLevel(int level)
    {
        GameObject bonusitem = null;
        if (level == 1)
        {
            bonusitem = Resources.Load("Prefabs/bonus_cherries", typeof(GameObject)) as GameObject;

        } else if (level == 2)
        {
            bonusitem = Resources.Load("Prefabs/bonus_strawberry", typeof(GameObject)) as GameObject;
        }
        else if (level == 3)
        {
            bonusitem = Resources.Load("Prefabs/bonus_peach", typeof(GameObject)) as GameObject;
        }
        else if (level == 4)
        {
            bonusitem = Resources.Load("Prefabs/bonus_peach", typeof(GameObject)) as GameObject;
        }
        else if (level == 5)
        {
            bonusitem = Resources.Load("Prefabs/bonus_apple", typeof(GameObject)) as GameObject;
        }
        else if (level == 6)
        {
            bonusitem = Resources.Load("Prefabs/bonus_apple", typeof(GameObject)) as GameObject;
        }
        else if (level == 7)
        {
            bonusitem = Resources.Load("Prefabs/bonus_grapes", typeof(GameObject)) as GameObject;
        }
        else if (level == 8)
        {
            bonusitem = Resources.Load("Prefabs/bonus_grapes", typeof(GameObject)) as GameObject;
        }
        else if (level == 9)
        {
            bonusitem = Resources.Load("Prefabs/bonus_galaxian", typeof(GameObject)) as GameObject;
        }
        else if (level == 10)
        {
            bonusitem = Resources.Load("Prefabs/bonus_galaxian", typeof(GameObject)) as GameObject;
        }
        else if (level == 11)
        {
            bonusitem = Resources.Load("Prefabs/bonus_bell", typeof(GameObject)) as GameObject;
        }
        else if (level == 12)
        {
            bonusitem = Resources.Load("Prefabs/bonus_bell", typeof(GameObject)) as GameObject;
        }
        else
        {
            bonusitem = Resources.Load("Prefabs/bonus_key", typeof(GameObject)) as GameObject;
        }

        Instantiate(bonusitem);
        
    }
    void UpdateUI()
    {
        playerScoreText.text = score.ToString();

            if (GameMenu.pacManLives == 3)
            {
                playerLives3.enabled = true;
                playerLives2.enabled = true;
            }
            else if (GameMenu.pacManLives == 2)
            {
                playerLives3.enabled = false;
                playerLives2.enabled = true;
            }
            else if (GameMenu.pacManLives == 1)
            {
                playerLives3.enabled = false;
                playerLives2.enabled = false;
            }
     
 
     /*   for(int i = 0; i < levelImages.Length; i++)
        {
            Image li = levelImages[i];
            li.enabled = false;
        }
         
        for(int i = 1; i < levelImages.Length + 1; i++)
        {
            if (currentLevel >= i)
            {
                Image li = levelImages[i - 1];
               // li.enabled = true;
            }
        } */
    }

    void CheckPelletsConsumed()
    {
            if (totalPellets == GameMenu.pelletsConsumed)
            {
                PlayerWin();
            }

    }

    void PlayerWin()
    {

            if (!didIncrementLevel)
            {
                didIncrementLevel = true;
                level++;
                StartCoroutine(ProcessWin(2));
            }

    }
     

    IEnumerator ProcessWin(float delay)
    {
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<PacMan>().canMove = false;
        pacMan.transform.GetComponent<Animator>().enabled = false;

        transform.GetComponent<AudioSource>().Stop();

        GameObject[] o =GameObject.FindGameObjectsWithTag("Ghost");

        foreach(GameObject ghost in o)
        {
            ghost.transform.GetComponent<Ghost>().canMove = false;
            ghost.transform.GetComponent<Animator>().enabled = false;

        }
        yield return new WaitForSeconds(delay);
        StartCoroutine(BlinkBoard(2));
    }

    IEnumerator BlinkBoard(float delay)
    {
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = false;

        }

        shouldBlink = true;

        yield return new WaitForSeconds(delay);

        shouldBlink = false;
        StartNextLevel();
    }

    private void StartNextLevel()
    {
        StopAllCoroutines();

     
            ResetPelletsForPlayer();
            GameMenu.pelletsConsumed = 0;
            didSpawnBonusItem1 = false;
            didSpawnBonusItem2 = false;
  

        GameObject.Find("Maze").transform.GetComponent<SpriteRenderer>().sprite = mazeBlue;

        didIncrementLevel = false;

        StartCoroutine(ProcessStartNextLevel(1));
    }

    IEnumerator ProcessStartNextLevel(float delay)
    {
        playerText.transform.GetComponent<Text>().enabled = true;
        readyText.transform.GetComponent<Text>().enabled = true;

        yield return new WaitForSeconds(delay);

        StartCoroutine(ProcessRestartShowObjects(1));
    }

    private void CheckShouldBlink()
    {
        if (shouldBlink)
        {
            if (blinkIntervalTimer < blinkIntervalTime)
            {
                blinkIntervalTimer += Time.deltaTime;
            }
            else
            {
                blinkIntervalTimer = 0;

                if (GameObject.Find("Maze").transform.GetComponent<SpriteRenderer>().sprite == mazeBlue)
                {
                    GameObject.Find("Maze").transform.GetComponent<SpriteRenderer>().sprite = mazeWhite;
                }
                else
                {
                    GameObject.Find("Maze").transform.GetComponent<SpriteRenderer>().sprite = mazeBlue;
                }

            }
        }
    }
    public void StartGame()
    {

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

            consumedGhostScoreText.text = ghostConsumedRunningScore.ToString();

            consumedGhostScoreText.GetComponent<Text>().enabled = true;
            transform.GetComponent<AudioSource>().PlayOneShot(consumedGhostAudioClip);
            StartCoroutine(ProcessConsumedAfter(0.75f, consumedGhost));

        }
    }

    public void StartConsumedBonusItem(GameObject bonusItem,int scoreValue)
    {
        Vector2 pos = bonusItem.transform.position;
        Vector2 viewPortPoint = Camera.main.WorldToViewportPoint(pos);

        consumedGhostScoreText.GetComponent<RectTransform>().anchorMin = viewPortPoint;
        consumedGhostScoreText.GetComponent<RectTransform>().anchorMax = viewPortPoint;

        consumedGhostScoreText.text = scoreValue.ToString();

        consumedGhostScoreText.GetComponent<Text>().enabled = true;

        Destroy(bonusItem.gameObject);

        StartCoroutine(ProcessConsumedBonusItem(0.75f));
    }

    IEnumerator ProcessConsumedBonusItem(float delay)
    {
        yield return new WaitForSeconds(delay);
        consumedGhostScoreText.GetComponent<Text>().enabled = false;
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

            GameObject bonusItem = GameObject.Find("bonusItem");
            if (bonusItem)
                Destroy(bonusItem.gameObject);
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
            GameMenu.pacManLives -= 1;

           
        if (GameMenu.pacManLives == 0 )
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
            ghost.transform.GetComponent<Animator>().enabled = true;
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
        int playerLevel = 0;

            playerLevel = level;

        GameObject.Find("PacMan").GetComponent<PacMan>().SetDifficultyForLevel(playerLevel);

        GameObject[] obj = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in obj)
        {
            ghost.transform.GetComponent<Ghost>().SetDifficultyForLevel(playerLevel);
        }

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

    void ResetPelletsForPlayer()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach(GameObject o in objects)
        {
            if (o.GetComponent<Tile>() != null)
            {
                if (o.GetComponent<Tile>().isPellet || o.GetComponent<Tile>().isSupperPellet)
                {                 
                        o.GetComponent<Tile>().didConsume = false;
             
                }
            }
        }
    }
}


