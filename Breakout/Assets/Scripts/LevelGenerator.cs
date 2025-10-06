using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public Vector2Int size;
    public Vector2 offset;
    public GameObject brickPrefab;
    public Gradient gradient;

    private void Awake()
    {
        if (GameSettings.chosenSize.x > 0)
        {
            size = GameSettings.chosenSize;
        }
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                GameObject newbrick = Instantiate(brickPrefab, transform);
                newbrick.transform.position = transform.position + new Vector3((float)((size.x - 1) * 0.5f - i) * offset.x, j * offset.y, 0);
                newbrick.GetComponent<SpriteRenderer>().color = gradient.Evaluate((float)j / (size.y - 1));
            }
        }
    }

    void Start()
    { }

    void Update()
    { }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
}