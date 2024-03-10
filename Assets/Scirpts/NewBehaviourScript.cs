using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeFruitGame : MonoBehaviour
{
    public GameObject[] fruitPrefabs; // Array of different fruit prefabs
    public Transform spawnPoint; // Point where fruits are spawned
    public float spawnInterval = 2f; // Time interval between fruit spawns

    private void Start()
    {
        StartCoroutine(SpawnFruits());
    }

    IEnumerator SpawnFruits()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Randomly select a fruit prefab
            GameObject fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            // Spawn the selected fruit at the spawn point
            GameObject fruit = Instantiate(fruitPrefab, spawnPoint.position, Quaternion.identity);

            // Let the fruit know it's merged
            fruit.GetComponent<Fruit>().OnMerge += HandleMerge;
        }
    }

    void HandleMerge(GameObject mergedFruit)
    {
        // Handle merge logic here
        Debug.Log("Merged: " + mergedFruit.name);
    }
}

public class Fruit : MonoBehaviour
{
    public delegate void MergeAction(GameObject mergedFruit);
    public event MergeAction OnMerge;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is also a fruit
        Fruit otherFruit = collision.GetComponent<Fruit>();
        if (otherFruit != null)
        {
            // If two fruits of the same kind collide, trigger merge event
            if (otherFruit.gameObject.tag == this.gameObject.tag)
            {
                Merge();
            }
        }
    }

    void Merge()
    {
        // Trigger merge event
        OnMerge?.Invoke(gameObject);

        // Destroy the current fruit
        Destroy(gameObject);
    }
}
