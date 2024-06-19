using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private PlayerFire player;
    private AudioSource bgOST;

    private bool charaApply;
    private bool playerTurn;
    private bool enemyTurn;
    private bool flyingRock;
    private bool oneShot;
    private bool gameRestart;

    private float power = 0.2f;
    private float powerReset;

    private int charaCode = 0;
    private int enemyCode;

    public int CharaCode { get => charaCode; set => charaCode = value; }

    public int EnemyCode { get => enemyCode; set => enemyCode = value; }

    public float Power { get => power; set => power = value; }

    public float PowerReset { get => powerReset; }

    public bool PlayerTurn { get => playerTurn; set => playerTurn = value; }

    public bool EnemyTurn { get => enemyTurn; set => enemyTurn = value; }

    public bool FlyingRock { get => flyingRock; set => flyingRock = value; }

    public bool OneShotMode { get => oneShot; set => oneShot = value; }

    public bool GameRestart { get => gameRestart; set => gameRestart = value; }

    private void Awake()
    {
        charaApply  = false;
        flyingRock  = false;
        playerTurn  = false;
        enemyTurn   = false;
        oneShot     = false;
        gameRestart = false;

        powerReset  = power;

        bgOST = GetComponent<AudioSource>();

        bgOST.Play();
        bgOST.loop = true;

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnScene;
    }

    private void OnScene(Scene scene, LoadSceneMode mode)
    {
        gameRestart = false;
        power = powerReset;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main Screen") GameStart();
        else if (SceneManager.GetActiveScene().name == "Mode Choice") ModeSelect();
        else if (SceneManager.GetActiveScene().name == "Chara Choice" || SceneManager.GetActiveScene().name == "One Shot Choice") CharaSelect();
        else if (SceneManager.GetActiveScene().name == "Fortress" || SceneManager.GetActiveScene().name == "One Shot Fortress") FightStage();
        else if (SceneManager.GetActiveScene().name == "Hidden Fortress") if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("Mode Choice");
    }

    private void GameStart()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        else if (Input.anyKeyDown) SceneManager.LoadScene("Mode Choice");
    }

    private void ModeSelect()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (oneShot == false) SceneManager.LoadScene("Chara Choice");
            else SceneManager.LoadScene("One Shot Choice");
        }

        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("Main Screen");
            Destroy(gameObject);
        }

        else if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    private void CharaSelect()
    {
        if (charaCode > 3) charaCode = 0;
        else if (charaCode < 0) charaCode = 3;

        if (enemyCode > 3) enemyCode = 0;
        else if (enemyCode < 0) enemyCode = 3;

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        else if (Input.GetKeyDown(KeyCode.Backspace)) SceneManager.LoadScene("Mode Choice");

        if (oneShot == false && Input.GetKeyDown(KeyCode.Space))
        {
            charaApply = false;
            playerTurn = true;
            SceneManager.LoadScene("Fortress");
        }

        else if (oneShot == true && Input.GetKeyDown(KeyCode.Space))
        {
            charaApply = false;
            SceneManager.LoadScene("One Shot Fortress");
        }
    }

    private void FightStage()
    {
        player = GameObject.Find("Player").GetComponent<PlayerFire>();

        if (charaApply == false)
        {
            do enemyCode = Random.Range(0, 3); while (charaCode == enemyCode);

            if (oneShot == true)
            {
                if (Random.value > 0.5f)
                {
                    playerTurn = true;
                    enemyTurn = false;
                }
                else
                {
                    playerTurn = false;
                    enemyTurn = true;
                }
            }

            charaApply = true;
        }

        if (gameRestart == true && Input.GetKeyDown(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name == "Fortress")
            {
                if (player.PlayerHP > 0) SceneManager.LoadScene("Hidden Fortress");
                if (player.PlayerHP <= 0) SceneManager.LoadScene("Mode Choice");
            }

            if (SceneManager.GetActiveScene().name != "Fortress") SceneManager.LoadScene("Mode Choice");

            gameRestart = false;
        }
    }
}
