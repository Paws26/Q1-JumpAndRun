using UnityEngine;

public class GameOverUI : MonoBehaviour
{

    public void OnRespawnButtonClicked()
    {
        UIManager.Instance.Respawn();
    }

    public void OnQuitButtonClicked()
    {
        UIManager.Instance.Quit();
    }
}
