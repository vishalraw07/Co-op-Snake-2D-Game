using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MultiplayerManager : MonoBehaviour
{
    [SerializeField] PlayerMovement[] players;
    [SerializeField] MPCanvas canvas;
    [SerializeField] CollectiblesController collectibles;
    [SerializeField] int powerUpCooldownTimer = 3;
    [SerializeField] int gainerFoodScore = 20;
    [SerializeField] int burnerFoodScore = 10;
    [SerializeField] int lengthIncreasePerGainer = 1;
    [SerializeField] int lengthDecreasePerBurner = 1;
    int[] multipliers;
    int[] scores;
    void Start()
    {
        scores = new int[players.Length];
        multipliers = new int[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            scores[i] = 0;
            multipliers[i] = 1;
        }
        canvas.UpdateScore(scores);
        List<Transform> playerBodyTransforms = GetPlayerBodyTransforms();
        StartCoroutine(SpawnFood());
        StartCoroutine(SpawnPowerUp());
    }
    IEnumerator SpawnPowerUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 16));
            collectibles.SpawnPowerUp(GetPlayerBodyTransforms());
        }
    }
    IEnumerator SpawnFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));
            collectibles.SpawnFood(GetPlayerBodyTransforms());
        }
    }
    List<Transform> GetPlayerBodyTransforms()
    {
        List<Transform> playerBodyTransform = new List<Transform>();
        for (int i = 0; i < players.Length; i++)
        {
            playerBodyTransform.AddRange(players[i].GetBodiesTransform());
        }
        return playerBodyTransform;
    }
    public void FoodConsumed(Player _player, FoodType _foodType)
    {
        AudioManager.Instance.PlaySound(SoundType.Food);
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].player == _player)
            {
                if (_foodType == FoodType.Gainer || players[i].GetPlayerLength() < 6)
                {
                    for (int j = 0; j < lengthIncreasePerGainer; j++)
                        players[i].AddBody();
                    scores[i] += gainerFoodScore * multipliers[i];
                }
                else
                {
                    for (int j = 0; j < lengthIncreasePerGainer; j++)
                        players[i].RemoveBody();
                    scores[i] -= burnerFoodScore;
                }
                break;
            }
        }
        canvas.UpdateScore(scores);
    }
    public void PowerUpConsumed(Player _player, PowerUpType _powerUpType)
    {
        AudioManager.Instance.PlaySound(SoundType.PowerUp);
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].player == _player)
            {
                if (_powerUpType == PowerUpType.Multiplier)
                {
                    StartCoroutine(DoubleScore(i));
                    break;
                }
                players[i].PowerUp(_powerUpType, powerUpCooldownTimer);
            }
        }
        canvas.PowerUpModifier(_player, _powerUpType, powerUpCooldownTimer);
    }
    IEnumerator DoubleScore(int index)
    {
        multipliers[index] = 2;
        yield return new WaitForSeconds(powerUpCooldownTimer);
        multipliers[index] = 1;
    }
    public void PauseGameTrigger()
    {
        AudioManager.Instance.PlaySound(SoundType.Button);
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
        canvas.PauseScreenUI();
    }
    public void GameOver(Player player)
    {
        AudioManager.Instance.PlaySound(SoundType.GameOver);
        Time.timeScale = 0;
        canvas.GameOverUI(scores, player);
    }
    public void MainMenu()
    {
        AudioManager.Instance.PlaySound(SoundType.Button);
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        LevelManager.Instance.LoadMainMenu();
    }
    public void RestartLevel()
    {
        AudioManager.Instance.PlaySound(SoundType.Button);
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        LevelManager.Instance.ReloadCurrentLevel();
    }
    public void ExitGame()
    {
        AudioManager.Instance.PlaySound(SoundType.Button);
        LevelManager.Instance.QuitGame();
    }
}
