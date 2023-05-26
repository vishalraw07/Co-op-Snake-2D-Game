using UnityEngine;
public enum PowerUpType
{
    Shield, Multiplier, SpeedBoost
}
public class PowerUpController : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] public PowerUpType powerUpType;
    [SerializeField] Sprite shieldImg;
    [SerializeField] Sprite multiplierImg;
    [SerializeField] Sprite speedBoostImg;
    void Awake()
    {
        powerUpType = (PowerUpType)Random.Range(0, 3);
        switch (powerUpType)
        {
            case PowerUpType.Shield:
                spriteRenderer.sprite = shieldImg;
                break;
            case PowerUpType.Multiplier:
                spriteRenderer.sprite = multiplierImg;
                break;
            case PowerUpType.SpeedBoost:
                spriteRenderer.sprite = speedBoostImg;
                break;
        }
        spriteRenderer.color = Color.green;
    }
}
