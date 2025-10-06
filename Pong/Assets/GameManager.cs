using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI computerScoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI countdownText;

    public Ball ball;
    public int winningScore = 10;
    public int countdownTime = 10;

    private int playerScore;
    private int computerScore;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    public void PlayerScores()
    {
        playerScore++;
        playerScoreText.text = playerScore.ToString();
        CheckForWinner();
    }

    public void ComputerScores()
    {
        computerScore++;
        computerScoreText.text = computerScore.ToString();
        CheckForWinner();
    }

    private void CheckForWinner()
    {
        if (playerScore >= winningScore)
        {
            EndGame("Player 1 Wins!");
        }
        else if (computerScore >= winningScore)
        {
            EndGame("Computer Wins!");
        }
        else
        {
            ball.ResetAndLaunch();
        }
    }

    private void EndGame(string winnerMessage)
    {
        gameOverText.text = winnerMessage;
        gameOverText.gameObject.SetActive(true);

        ball.gameObject.SetActive(false);

        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        countdownText.gameObject.SetActive(true);

        int remainingTime = countdownTime;
        while (remainingTime > 0)
        {
            countdownText.text = remainingTime.ToString();
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        countdownText.text = "0";
        yield return new WaitForSeconds(1f);
        RestartGame();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}