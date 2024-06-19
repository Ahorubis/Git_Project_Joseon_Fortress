using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField] private Image playerHP;
    [SerializeField] private Image enemyHP;

    private GameController gameController;
    private PlayerFire player;
    private EnemyFire enemy;

    private float playerHealthReset;
    private float enemyHealthReset;

    private void Awake()
    {
        gameController  = GameObject.Find("GameSetting").GetComponent<GameController>();
        player          = GameObject.Find("Player").GetComponent<PlayerFire>();
        enemy           = GameObject.Find("Enemy").GetComponent<EnemyFire>();

        playerHealthReset = player.PlayerHP;
        enemyHealthReset = enemy.EnemyHP;
    }

    private void Update()
    {
        PlayerHealth();
        EnemyHealth();
    }

    private void PlayerHealth()
    {
        float result = player.PlayerHP / playerHealthReset;
        playerHP.rectTransform.sizeDelta = new Vector2(180 * result, 20);
    }

    private void EnemyHealth()
    {
        float result = enemy.EnemyHP / enemyHealthReset;
        enemyHP.rectTransform.sizeDelta = new Vector2(180 * result, 20);
    }
}
