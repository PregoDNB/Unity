using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void SelectEasy()
    {
        GameSettings.chosenSize = new Vector2Int(5, 3);
        SceneManager.LoadScene("GameScene");
    }

    public void SelectMedium()
    {
        GameSettings.chosenSize = new Vector2Int(10, 5);
        SceneManager.LoadScene("GameScene");
    }

    public void SelectHard()
    {
        GameSettings.chosenSize = new Vector2Int(15, 7);
        SceneManager.LoadScene("GameScene");
    }
}