using UnityEngine;

public class Brick : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public float powerUpChance = 0.8f; // 80% kans

    public void GetHit()
    {
        if (Random.value < powerUpChance)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}