using UnityEngine;
using UnityEngine.SceneManagement;

public class ThrowRock : MonoBehaviour
{
    [SerializeField] private float rockSpeed = 8;

    private GameController gameController;
    private PlayerFire player;
    private EnemyFire enemy;
    private Rigidbody2D rigid;

    private float speedDefault;
    private float x;

    private void Start()
    {
        player          = GameObject.Find("Player").GetComponent<PlayerFire>();
        enemy           = GameObject.Find("Enemy").GetComponent<EnemyFire>();
        gameController  = GameObject.Find("GameSetting").GetComponent<GameController>();

        rigid = GetComponent<Rigidbody2D>();

        speedDefault    = rockSpeed;
        if (gameController.PlayerTurn == true)
        {
            rigid.velocity = transform.localPosition * rockSpeed * Mathf.Clamp(gameController.Power, 0.2f, 1);
            x = rigid.velocity.x;
            rigid.velocity = new Vector2(x, rigid.velocity.y);
        }

        else if (gameController.EnemyTurn == true)
        {
            rigid.velocity = transform.localPosition * rockSpeed * Mathf.Clamp(gameController.Power, 0.2f, 1.5f);
            x = rigid.velocity.x;
            rigid.velocity = new Vector2(-x, rigid.velocity.y);
        }


    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "Hidden Fortress")
        {
            if (gameController.PlayerTurn == true)
            {
                float angle = Mathf.Atan2(rigid.velocity.y, rigid.velocity.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle);
            }

            else if (gameController.EnemyTurn == true)
            {
                float angle = Mathf.Atan2(rigid.velocity.y, -rigid.velocity.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, -angle);
            }
        }
    }

    private void OnBecameInvisible()
    {
        if (SceneManager.GetActiveScene().name != "Hidden Fortress")
        {
            gameController.Power = gameController.PowerReset;

            if (gameController.PlayerTurn == true)
            {
                gameController.PlayerTurn = false;
                gameController.EnemyTurn = true;

                if (gameController.OneShotMode == false) enemy.DdongOnline = true;
                else enemy.GloriousPoop = false;
            }

            else if (gameController.EnemyTurn == true)
            {
                gameController.EnemyTurn = false;
                gameController.PlayerTurn = true;
                player.GloriousPoop = false;
            }
        }

        gameController.FlyingRock = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            gameController.FlyingRock = false;

            if (SceneManager.GetActiveScene().name != "Hidden Fortress")
            {
                gameController.Power = gameController.PowerReset;
                player.GloriousPoop = false;
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            gameController.FlyingRock = false;

            if (SceneManager.GetActiveScene().name != "Hidden Fortress")
            {
                gameController.Power = gameController.PowerReset;
                player.PlayerHP--;
                player.GloriousPoop = false;
            }

            if (gameController.OneShotMode == true) enemy.GloriousPoop = false;

            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            gameController.FlyingRock = false;

            if (SceneManager.GetActiveScene().name != "Hidden Fortress")
            {
                gameController.Power = gameController.PowerReset;
                enemy.EnemyHP--;
                player.GloriousPoop = false;
            }

            if (gameController.OneShotMode == true) enemy.GloriousPoop = false;

            Destroy(gameObject);
        }
    }
}
