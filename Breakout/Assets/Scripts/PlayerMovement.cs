using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float maxX = 7.5f;

    private float movementHorizontal;
    private Vector3 originalScale;
    private Coroutine biggerPaddleRoutine;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        movementHorizontal = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * movementHorizontal * speed * Time.deltaTime;

        // Clamp de positie binnen de grenzen
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, -maxX, maxX);
        transform.position = clampedPos;
    }

    public void ActivateBiggerPaddle(float duration = 10f)
    {
        if (biggerPaddleRoutine != null)
            StopCoroutine(biggerPaddleRoutine);

        biggerPaddleRoutine = StartCoroutine(BiggerPaddleRoutine(duration));
    }

    private IEnumerator BiggerPaddleRoutine(float duration)
    {
        // Maak breder (alleen X), hoogte blijft gelijk
        transform.localScale = new Vector3(originalScale.x * 1.5f, originalScale.y, originalScale.z);

        yield return new WaitForSeconds(duration);

        // Reset terug
        transform.localScale = originalScale;
        biggerPaddleRoutine = null;
    }

    public void ResetPaddleSize()
    {
        transform.localScale = originalScale;

        if (biggerPaddleRoutine != null)
        {
            StopCoroutine(biggerPaddleRoutine);
            biggerPaddleRoutine = null;
        }
    }
}