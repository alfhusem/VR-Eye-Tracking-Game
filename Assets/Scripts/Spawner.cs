using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] quadPrefabs; 
    private float baseSpawnRate = 6f; // Initial spawn rate
    private float minSpawnRate = 2f; // Minimum spawn rate
    private float spawnDistanceThreshold = 30f;
    private float spawnRadius = 20f;
    private float minSpeed = 3f;
    private float maxSpeed = 4f;
    private float maxSpeedCap = 7f; // Maximum speed cap
    private float speedIncreaseInterval = 10f; // Time in seconds to increase max speed
    private float spawnRateDecreaseDelay = 15f; // Time in seconds before starting to decrease spawn rate
    private float spawnRateDecreaseInterval = 10f; // Time in seconds to decrease spawn rate

    private float spawnCooldown;
    private float lastSpeedIncreaseTime;
    private float lastSpawnRateDecreaseTime;
    public Transform playerHead;
    public Transform target;

    void Start()
    {
        lastSpeedIncreaseTime = Time.time;
        lastSpawnRateDecreaseTime = Time.time;
    }

    void Update()
    {
        // Increase max speed every 'speedIncreaseInterval' seconds, up to the cap
        if (Time.time >= lastSpeedIncreaseTime + speedIncreaseInterval && maxSpeed < maxSpeedCap)
        {
            maxSpeed += 1f;
            lastSpeedIncreaseTime = Time.time;
        }

        // Decrease base spawn rate every 'spawnRateDecreaseInterval' seconds after 'spawnRateDecreaseDelay' has passed
        if (Time.time - lastSpawnRateDecreaseTime > spawnRateDecreaseDelay && 
            Time.time >= lastSpawnRateDecreaseTime + spawnRateDecreaseInterval && 
            baseSpawnRate > minSpawnRate)
        {
            baseSpawnRate -= 1f;
            lastSpawnRateDecreaseTime = Time.time;
        }

        Vector3 toSpawner = transform.position - playerHead.position;
        float angleToGaze = Vector3.Angle(playerHead.forward, toSpawner);

        if ((angleToGaze > spawnDistanceThreshold && Time.time >= spawnCooldown) || 
            (Input.GetKey(KeyCode.S) && Time.time >= spawnCooldown))
        {
            SpawnQuad();
            spawnCooldown = Time.time + baseSpawnRate + Random.Range(0f, 4f);
        }
    }

    void SpawnQuad()
    {
        Vector3 spawnPos = Random.onUnitSphere * spawnRadius + transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(target.position - spawnPos);
        // Choose a random prefab from the array
        GameObject selectedPrefab = quadPrefabs[Random.Range(0, quadPrefabs.Length)];
        // Rest of your spawn logic...
        GameObject quad = Instantiate(selectedPrefab, spawnPos, lookRotation);

        MoveTowardsTarget moveTowards = quad.GetComponent<MoveTowardsTarget>();

        if (moveTowards != null)
        {
            moveTowards.target = target;
            moveTowards.speed = Random.Range(minSpeed, maxSpeed); // Set a random speed between min and max
        }
    }
}
