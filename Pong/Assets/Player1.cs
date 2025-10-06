using JetBrains.Annotations;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Rigidbody2D rb;

    public float minY;

    public float maxY;

    private Vector2 moveInput;

    void Update()
    {
        float moveY = 0f;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
        }
        moveInput = new Vector2(0, moveY);
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime;

        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        rb.MovePosition(newPosition);
    }
}