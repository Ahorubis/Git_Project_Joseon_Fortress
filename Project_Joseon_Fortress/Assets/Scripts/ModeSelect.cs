using UnityEngine;

public class ModeSelect : MonoBehaviour
{
    [SerializeField] private Sprite[] modeSelect;

    private GameController gameController;
    private SpriteRenderer select;

    int index = 0;

    private void Awake()
    {
        gameController = GameObject.Find("GameSetting").GetComponent<GameController>();

        select = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Mathf.Clamp(index, 0, 1);

        if (Input.GetKeyDown(KeyCode.DownArrow)) index++;
        else if (Input.GetKeyDown(KeyCode.UpArrow)) index--;

        for (int i = 0; i < modeSelect.Length; i++) if (i == index) select.sprite = modeSelect[i];

        if (index == 0) gameController.OneShotMode = false;
        else gameController.OneShotMode = true;
    }
}
