using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] CharacterIndex;
    [SerializeField] private GameObject fireArrow;
    [SerializeField] private GameObject rock;
    [SerializeField] private AnimationCurve curve;

    [SerializeField] private float playerHP = 10;
    [SerializeField] private float fireSpeed;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip throwing;
    [SerializeField] private AudioClip slap;

    private GameController gameController;
    private Animator animator;
    private AudioSource soundEffect;

    private float fireAngle = 0;

    private bool gloriousPoop; //발사 횟수 제한값

    public float PlayerHP { get => playerHP; set => playerHP = value; }

    public bool GloriousPoop { get => gloriousPoop; set => gloriousPoop = value; }

    private void Awake()
    {
        gameController = GameObject.Find("GameSetting").GetComponent<GameController>();

        animator = GetComponent<Animator>();
        soundEffect = GetComponent<AudioSource>();

        for (int i = 0; i < CharacterIndex.Length; i++) if (i == gameController.CharaCode) animator.runtimeAnimatorController = CharacterIndex[i];

        if (SceneManager.GetActiveScene().name == "One Shot Fortress") playerHP = 1;

        gloriousPoop = false;
        soundEffect.loop = false;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Hidden Fortress") DevelopFire();

        if (gameController.PlayerTurn == true)
        {
            if (gameController.OneShotMode == false) PlayerOnFire();
            else Player1();
        }

        if (playerHP <= 0)
        {
            gameController.PlayerTurn = false;
            gameController.GameRestart = true;
        }
    }

    private void Player1()
    {
        if (Input.GetKey(KeyCode.W))
        {
            fireAngle += Time.deltaTime * fireSpeed;
            float rad = fireAngle * Mathf.Deg2Rad;

            fireArrow.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            fireArrow.transform.eulerAngles = new Vector3(0, 0, fireAngle);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            fireAngle -= Time.deltaTime * fireSpeed;
            float rad = fireAngle * Mathf.Deg2Rad;

            fireArrow.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            fireArrow.transform.eulerAngles = new Vector3(0, 0, fireAngle);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (gloriousPoop == false) gameController.Power += Time.deltaTime;

            float recentColor = curve.Evaluate(gameController.Power);

            fireArrow.GetComponent<SpriteRenderer>().material.color = new Color(recentColor, 1, recentColor);
        }

        else if (Input.GetKeyUp(KeyCode.Space))
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

    private void PlayerOnFire()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            fireAngle += Time.deltaTime * fireSpeed;
            float rad = fireAngle * Mathf.Deg2Rad;

            fireArrow.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            fireArrow.transform.eulerAngles = new Vector3(0, 0, fireAngle);
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            fireAngle -= Time.deltaTime * fireSpeed;
            float rad = fireAngle * Mathf.Deg2Rad;

            fireArrow.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            fireArrow.transform.eulerAngles = new Vector3(0, 0, fireAngle);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (gloriousPoop == false) gameController.Power += Time.deltaTime;

            float recentColor = curve.Evaluate(gameController.Power);

            fireArrow.GetComponent<SpriteRenderer>().material.color = new Color(recentColor, 1, recentColor);
        }

        else if (Input.GetKeyUp(KeyCode.Space))
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

    private void DevelopFire()
    {
        gloriousPoop = false;
        gameController.PlayerTurn = true;
        gameController.Power = 30;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            fireAngle += Time.deltaTime * fireSpeed;
            float rad = fireAngle * Mathf.Deg2Rad;

            fireArrow.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            fireArrow.transform.eulerAngles = new Vector3(0, 0, fireAngle);
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            fireAngle -= Time.deltaTime * fireSpeed;
            float rad = fireAngle * Mathf.Deg2Rad;

            fireArrow.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            fireArrow.transform.eulerAngles = new Vector3(0, 0, fireAngle);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            GameObject bullet = Instantiate(rock) as GameObject;
            bullet.transform.parent = transform;
            bullet.transform.position = fireArrow.transform.position;
        }
    }

    private void DelayFire()
    {
        GameObject bullet = Instantiate(rock) as GameObject;
        bullet.transform.parent = transform;
        bullet.transform.position = fireArrow.transform.position;
        gameController.FlyingRock = true;

        soundEffect.clip = throwing;
        soundEffect.Play();
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

                gameController.PlayerTurn = false;
                gameController.EnemyTurn = true;
            }
        }
    }
}
