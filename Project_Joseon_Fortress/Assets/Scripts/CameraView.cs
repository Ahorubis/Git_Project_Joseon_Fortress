using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraView : MonoBehaviour
{
    [SerializeField] private GameObject gameRetry;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform zipsin;
    [SerializeField] private AudioClip roundSound;

    private GameObject zipsinTag;
    private GameController gameController;
    private AudioSource soundEffect;

    private void Awake()
    {
        zipsinTag = GameObject.FindWithTag("Zipsin");
        gameController = GameObject.Find("GameSetting").GetComponent<GameController>();

        soundEffect = GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().name == "Fortress" || SceneManager.GetActiveScene().name == "One Shot Fortress") gameRetry.gameObject.SetActive(false);

        soundEffect.loop = false;

        soundEffect.clip = roundSound;
        soundEffect.Play();
    }

    private void Start()
    {
        gameRetry.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Fortress") MovingView();
        else if (SceneManager.GetActiveScene().name == "One Shot Fortress") OneShotView();
        else if (SceneManager.GetActiveScene().name == "Hidden Fortress")
        {
            transform.position = new Vector3(0, 0, -10);
            Camera.main.orthographicSize = 2.5f;
        }
    }

    private void MovingView()
    {
        if (gameController.PlayerTurn == true && gameController.FlyingRock == false)
        {
            transform.position = player.transform.position + new Vector3(0, 0.25f, -10);
            Camera.main.orthographicSize = 1;
        }

        if (gameController.FlyingRock == true)
        {
            transform.position = new Vector3(0, 0, -10);
            Camera.main.orthographicSize = 2.5f;
        }

        if (gameController.EnemyTurn == true && gameController.FlyingRock == false)
        {
            transform.position = enemy.transform.position + new Vector3(0, 0.25f, -10);
            Camera.main.orthographicSize = 1;
        }

        if (gameController.GameRestart == true)
        {
            transform.position = player.transform.position + new Vector3(0, 0.25f, -10);
            Camera.main.orthographicSize = 1;

            gameRetry.gameObject.SetActive(true);
        }
    }

    private void OneShotView()
    {
        if (gameController.PlayerTurn == true && gameController.FlyingRock == false)
        {
            //(-3, -1.5, 0), (-3, -1.25, -10)
            transform.position = player.transform.position + new Vector3(0, 0.25f, -10);
            Camera.main.orthographicSize = 1;
        }

        if (gameController.FlyingRock == true)
        {
            //(0, -1.25, 0), (0, -1.25, -10)
            transform.position = new Vector3(0, -1.25f, -10);
            Camera.main.orthographicSize = 1.25f;
        }

        if (gameController.EnemyTurn == true && gameController.FlyingRock == false)
        {
            //(3, -1.5, 0), (3, -1.25, -10)
            transform.position = enemy.transform.position + new Vector3(0, 0.25f, -10);
            Camera.main.orthographicSize = 1;
        }

        if (gameController.GameRestart == true)
        {
            transform.position = new Vector3(0, -1.25f, -10);
            Camera.main.orthographicSize = 1.25f;

            gameRetry.gameObject.SetActive(true);
        }
    }
}
