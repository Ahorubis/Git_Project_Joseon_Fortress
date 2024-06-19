using UnityEngine;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour
{

    [SerializeField] private Sprite[] selectArrow;
    [SerializeField] private GameObject charaPrefab;

    private GameController gameController;
    private SpriteRenderer arrow;

    private SpriteRenderer playerOneArrow;
    private SpriteRenderer playerTwoArrow;

    private void Awake()
    {
        gameController = GameObject.Find("GameSetting").GetComponent<GameController>();

        if (SceneManager.GetActiveScene().name == "One Shot Choice")
        {
            gameController.CharaCode = 0;
            gameController.EnemyCode = 3;

            playerOneArrow = GameObject.Find("SelectArrow1").GetComponent<SpriteRenderer>();
            playerTwoArrow = GameObject.Find("SelectArrow2").GetComponent<SpriteRenderer>();
        }

        else if (SceneManager.GetActiveScene().name == "Chara Choice") arrow = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Mathf.Clamp(gameController.CharaCode, 0, 3);

        if (gameController.OneShotMode == false)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) gameController.CharaCode++;
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) gameController.CharaCode--;

            for (int i = 0; i < selectArrow.Length; i++) if (i == gameController.CharaCode) arrow.sprite = selectArrow[i];
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.D)) gameController.CharaCode++;
            else if (Input.GetKeyDown(KeyCode.A)) gameController.CharaCode--;

            for (int i = 0; i < selectArrow.Length; i++) if (i == gameController.CharaCode) playerOneArrow.sprite = selectArrow[i];

            if (Input.GetKeyDown(KeyCode.RightArrow)) gameController.EnemyCode++;
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) gameController.EnemyCode--;

            for (int i = 0; i < selectArrow.Length; i++) if (i == gameController.EnemyCode) playerTwoArrow.sprite = selectArrow[i];
        }
    }
}
