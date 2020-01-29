using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;

    [HideInInspector] public bool playersTurn = true;


    public Text levelText;
    public GameObject levelImage;
    public GameObject player;
    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;
    private bool gameStarted;

    // Start is called before the first frame update
    void Awake()
    {
        gameStarted = false;
        print("GameManager Awake");
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
      //      InitGame();
    }

    private void SceneLoaded(int index)
    {
        print("GameManager SceneLoaded");

        InitGame();
    }

    public void InitGame()
    {

        doingSetup = true;
        level++;
        // SceneManager.UnloadSceneAsync("Main");
       // levelImage = GameObject.Find("LevelImage");

     //   levelText = levelImage.FindObject("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;

        print("GameManager initgame");

     //   Destroy(player);

     //   player = new Player();
        
        Invoke("HideLevelImage", levelStartDelay);
        levelImage.SetActive(true);
        enemies.Clear();
        boardScript.SetupScene(level);

    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " " +
            "days, you done died. That wasn't very smart.";
        levelImage.SetActive(true);
        enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
     
        if (!gameStarted && Input.GetKeyDown("space"))
        {
            gameStarted = true;
            InitGame();
        }

        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i=0; i < enemies.Count;i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }

}
