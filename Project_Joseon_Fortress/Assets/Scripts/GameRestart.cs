using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    [SerializeField] private Sprite[] single;
    [SerializeField] private Sprite bamboo;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip defeat;
    [SerializeField] private AudioClip KO;

    private GameController gameController;
    private PlayerFire player;
    private EnemyFire enemy;
    private SpriteRenderer gameResult;
    private AudioSource soundEffect;

    private void Awake()
    {
        gameController = GameObject.Find("GameSetting").GetComponent<GameController>();
        player = GameObject.Find("Player").GetComponent<PlayerFire>();
        enemy = GameObject.Find("Enemy").GetComponent<EnemyFire>();

        gameResult = GetComponent<SpriteRenderer>();
        soundEffect = GetComponent<AudioSource>();

        soundEffect.loop = false;

        if (SceneManager.GetActiveScene().name == "Fortress")
        {
            if (player.PlayerHP <= 0)
            {
                soundEffect.clip = defeat;
                soundEffect.Play();
            }

            else if (enemy.EnemyHP <= 0)
            {
                soundEffect.clip = victory;
                soundEffect.Play();
            }
        }

        else if (SceneManager.GetActiveScene().name == "One Shot Fortress")
        {
            soundEffect.clip = KO;
            soundEffect.Play();
        }
    }

    private void Update()
    {
        if (gameController.OneShotMode == true)
            if (player.PlayerHP <= 0 || enemy.EnemyHP <= 0) gameResult.sprite = bamboo;

        if (gameController.OneShotMode == false)
        {
            if (player.PlayerHP <= 0) gameResult.sprite = single[0];
            else if (enemy.EnemyHP <= 0) gameResult.sprite = single[1];
        }
    }
}
