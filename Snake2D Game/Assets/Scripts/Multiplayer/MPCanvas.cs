using UnityEngine;
using TMPro;
using System.Collections;

public class MPCanvas : MonoBehaviour
{
    [SerializeField] TMP_Text[] scores;
    [SerializeField] TMP_Text[] gameOverScore;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject pauseScreenUI;
    [SerializeField] TMP_Text winnerText;
    [SerializeField] GameObject[] helpTips;
    [SerializeField] GameObject[] PowerUpUI;
    CanvasGroup[] P1_powers;
    CanvasGroup[] P2_powers;
    void Start()
    {
        StartCoroutine(HelpFlash());
        P1_powers = PowerUpUI[0].GetComponentsInChildren<CanvasGroup>();
        P2_powers = PowerUpUI[1].GetComponentsInChildren<CanvasGroup>();
    }
    public void UpdateScore(int[] _scores)
    {
        for (int i = 0; i < _scores.Length; i++)
        {
            scores[i].text = _scores[i].ToString();
        }
    }
    public void GameOverUI(int[] _scores, Player player)
    {
        int maxScore = 0;
        int playerid = 0;
        for (int i = 0; i < _scores.Length; i++)
        {
            gameOverScore[i].text = _scores[i].ToString();
            if (_scores[i] > maxScore)
            {
                maxScore = _scores[i];
                playerid = i + 1;
            }
        }
        if (_scores.Length > 1)
        {
            if (player == Player.Player1)
                winnerText.text = "Player " + 2 + " is the winner!";
            else
                winnerText.text = "Player " + 1 + " is the winner!";
            winnerText.gameObject.SetActive(true);
        }
        gameOverUI.SetActive(true);
    }
    public void PauseScreenUI()
    {
        if (pauseScreenUI.activeInHierarchy)
            pauseScreenUI.SetActive(false);
        else
            pauseScreenUI.SetActive(true);
    }
    IEnumerator HelpFlash()
    {
        foreach (var item in helpTips)
        {
            item.SetActive(true);
        }
        yield return new WaitForSeconds(5f);
        foreach (var item in helpTips)
        {
            item.SetActive(false);
        }
    }
    IEnumerator PowerUpEnabled(CanvasGroup canvas, int timer)
    {
        Debug.Log("Timer start");
        canvas.alpha = 1f;
        yield return new WaitForSeconds(timer);
        Debug.Log("Timer over");
        canvas.alpha = 0.3f;
    }
    public void PowerUpModifier(Player _player, PowerUpType _powerUpType, int timer)
    {
        if (_player == Player.Player1)
        {
            switch (_powerUpType)
            {
                case PowerUpType.Shield:
                    StartCoroutine(PowerUpEnabled(P1_powers[0], timer));
                    break;
                case PowerUpType.Multiplier:
                    StartCoroutine(PowerUpEnabled(P1_powers[1], timer));
                    break;
                case PowerUpType.SpeedBoost:
                    StartCoroutine(PowerUpEnabled(P1_powers[2], timer));
                    break;
            }
        }
        else
        {
            switch (_powerUpType)
            {
                case PowerUpType.Shield:
                    StartCoroutine(PowerUpEnabled(P2_powers[0], timer));
                    break;
                case PowerUpType.Multiplier:
                    StartCoroutine(PowerUpEnabled(P2_powers[1], timer));
                    break;
                case PowerUpType.SpeedBoost:
                    StartCoroutine(PowerUpEnabled(P2_powers[2], timer));
                    break;
            }
        }
    }
}
