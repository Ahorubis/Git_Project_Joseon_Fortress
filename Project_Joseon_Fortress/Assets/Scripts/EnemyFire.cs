using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] CharacterIndex;
    [SerializeField] private GameObject fireArrow;
    [SerializeField] private GameObject rock;
    [SerializeField] private AnimationCurve curve;

    [SerializeField] private float enemyHP = 10;
    [SerializeField] private float fireSpeed;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip throwing;
    [SerializeField] private AudioClip slap;

    private GameController gameController;
    private Animator animator;
    private AudioSource soundEffect;

    private float fireAngle = 0;
    private float angleReset;

    private bool ddongOnline; //AI 각도 조절 값
    private bool holyShit; //AI 발사 조절 값
    private bool gloriousPoop; //발사 횟수 제한값

    public float EnemyHP { get => enemyHP; set => enemyHP = value; }

    public bool DdongOnline { get => ddongOnline; set => ddongOnline = value; }

    public bool GloriousPoop { get => gloriousPoop; set => gloriousPoop = value; }

    private void Awake()
    {
        gameController = GameObject.Find("GameSetting").GetComponent<GameController>();

        animator = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "One Shot Fortress") enemyHP = 1;

        else if (SceneManager.GetActiveScene().name != "One Shot Fortress" && SceneManager.GetActiveScene().name == "Hidden Fortress")
        {
            enemyHP = 1;
            angleReset = fireAngle;
            transform.position = new Vector3(Random.Range(1, 3.2f), -1.5f, 0);
            ddongOnline = false;
            holyShit = false;
        }

        soundEffect = GetComponent<AudioSource>();
        soundEffect.loop = false;

        gloriousPoop = false;
    }

    private void Update()
    {
        for (int i = 0; i < CharacterIndex.Length; i++) if (i == gameController.EnemyCode) animator.runtimeAnimatorController = CharacterIndex[i];

        if (enemyHP <= 0)
        {
            gameController.EnemyTurn = false;
            gameController.GameRestart = true;
        }

        if (gameController.OneShotMode == true && gameController.EnemyTurn == true && gameController.GameRestart == false) Player2();

        else if (gameController.OneShotMode == false && gameController.GameRestart == false)
        {
            if (gameController.EnemyTurn == true)
            {
                if (SceneManager.GetActiveScene().name != "Hidden Fortress")
                    if (gameController.GameRestart == false) EnemyOnAngle();
            }

            else fireAngle = angleReset;
        }
    }

    private void Player2()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            fireAngle += Time.deltaTime * fireSpeed;
            float rad = fireAngle * Mathf.Deg2Rad;

            fireArrow.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            fireArrow.transform.eulerAngles = new Vector3(0, 0, -fireAngle);
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            fireAngle -= Time.deltaTime * fireSpeed;
            float rad = fireAngle * Mathf.Deg2Rad;

            fireArrow.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            fireArrow.transform.eulerAngles = new Vector3(0, 0, -fireAngle);
        }

        if (SceneManager.GetActiveScene().name == "One Shot Fortress")
        {
            if (Input.GetKey(KeyCode.Return))
            {
                if (gloriousPoop == false) gameController.Power += Time.deltaTime;

                float recentColor = curve.Evaluate(gameController.Power);

                fireArrow.GetComponent<SpriteRenderer>().material.color = new Color(recentColor, 1, recentColor);
            }

            else if (Input.GetKeyUp(KeyCode.Return))
            {
                fireArrow.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1);

                if (gloriousPoop == false)
                {
                    gloriousPoop = true;

                    animator.SetTrigger("Attack");

                    Invoke("DelayFire", 1);
                }
            }
        }
    }

    private void EnemyOnAngle()
    {
        if (ddongOnline == true)
        {
            transform.position = new Vector3(Random.Range(1, 3.2f), -1.5f, 0);
            fireAngle = Random.Range(0, 75);
            holyShit = true;
            ddongOnline = false;
        }

        float rad = fireAngle * Mathf.Deg2Rad;

        fireArrow.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        fireArrow.transform.eulerAngles = new Vector3(0, 0, -fireAngle);

        gameController.Power += Time.deltaTime * Random.Range(0.1f, 0.25f);

        Invoke("EnemyOnFire", 2);
    }

    private void EnemyOnFire()
    {
        if (holyShit == true)
        {
            animator.SetTrigger("Attack");

            Invoke("DelayFire", 1);
            holyShit = false;
        }
    }

    private void DelayFire()
    {
        GameObject bullet = Instantiate(rock) as GameObject;
        bullet.transform.parent = transform;
        bullet.transform.position = fireArrow.transform.position;
        bullet.transform.localScale = new Vector3(1, 1, 1);

        soundEffect.clip = throwing;
        soundEffect.Play();
        gameController.FlyingRock = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zipsin"))
        {
            animator.SetTrigger("Hit");

            if (SceneManager.GetActiveScene().name != "Hidden Fortress")
            {
                soundEffect.clip = slap;
                soundEffect.Play();

                gameController.EnemyTurn = false;
                gameController.PlayerTurn = true;
            }
        }
    }
}
