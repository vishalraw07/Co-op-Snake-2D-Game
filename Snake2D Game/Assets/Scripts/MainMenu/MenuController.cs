using UnityEngine;
public class MenuController : MonoBehaviour
{
    [SerializeField] CanvasGroup MainMenu;
    [SerializeField] GameObject PlayMenu;
    [SerializeField] GameObject SettingsMenu;
    public void StartClicked()
    {
        AudioManager.Instance.PlaySound(SoundType.Button);
        MainMenu.alpha = 0.5f;
        MainMenu.interactable = false;
        if (!PlayMenu.activeInHierarchy)
            PlayMenu.SetActive(true);
    }
    public void SettingsClicked()
    {
        AudioManager.Instance.PlaySound(SoundType.Button);
        MainMenu.alpha = 0.5f;
        MainMenu.interactable = false;
        if (!SettingsMenu.activeInHierarchy)
            SettingsMenu.SetActive(true);
    }
    public void BackClicked()
    {
        AudioManager.Instance.PlaySound(SoundType.Button);
        MainMenu.alpha = 1f;
        MainMenu.interactable = true;
        if (SettingsMenu.activeInHierarchy)
            SettingsMenu.SetActive(false);
        if (PlayMenu.activeInHierarchy)
            PlayMenu.SetActive(false);
    }
    public void SinglePlayer()
    {
        AudioManager.Instance.PlaySound(SoundType.Button);
        LevelManager.Instance.LoadSceneAtIndex(1);
    }
    public void MultiPlayer()
    {
        AudioManager.Instance.PlaySound(SoundType.Button);
        LevelManager.Instance.LoadSceneAtIndex(2);
    }
    public void ExitClicked()
    {
        AudioManager.Instance.PlaySound(SoundType.Button);
        LevelManager.Instance.QuitGame();
    }
    public void SetSfxVolume(float _volume)
    {
        AudioManager.Instance.SetSfxVolume(_volume);
    }
    public void SetMenuVolume(float _volume)
    {
        AudioManager.Instance.SetMenuVolume(_volume);
    }
}
