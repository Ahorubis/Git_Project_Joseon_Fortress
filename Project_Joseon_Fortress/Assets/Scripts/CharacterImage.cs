using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterImage : MonoBehaviour
{
    [SerializeField] private Sprite[] character;

    private GameController gameController;
    private SpriteRenderer player;

    private SpriteRenderer playerOne;
    private SpriteRenderer playerTwo;

    private void Awake()
    {
        gameController = GameObject.Find("GameSetting").GetComponent<GameController>();

        if (SceneManager.GetActiveScene().name == "One Shot Choice")
        {
            gameController.CharaCode = 0;
            gameController.EnemyCode = 3;

            playerOne = GameObject.Find("Player1").GetComponent<SpriteRenderer>();
            playerTwo = GameObject.Find("Player2").GetComponent<SpriteRenderer>();
        }

        else if (SceneManager.GetActiveScene().name == "Chara Choice") player = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (gameController.OneShotMode == false)
        {
            for (int i = 0; i < character.Length; i++)
            {
                if (i == gameController.CharaCode) player.sprite = character[i];
            }
        }

        else
        {
            for (int i = 0; i < character.Length; i++)
            {
                if (i == gameController.CharaCode) playerOne.sprite = character[i];
                if (i == gameController.EnemyCode) playerTwo.sprite = character[i];
            }
        }
    }
}
