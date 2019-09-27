using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public GameObject bonusPrefab;
    public int platformPoolSize = 5;
    public GameObject platformPrefab;
    public float SpawnRate = 1f;
    public float platformMin = -1f;
    public float platformMax = 3.5f;

    private GameObject[] platforms;
    private Vector2 objectPoolPosition = new Vector2(-15f, -25f);
    private float timeSinceLastSpawned;
    private float spawnXPosition = 9f;
    private int currentPlatform = 0;
    private int lastPlatform;

    // Start is called before the first frame update
    void Start()
    {
        lastPlatform = platformPoolSize - 1;
        platforms = new GameObject[platformPoolSize];

        for (int i = 0; i < platformPoolSize; i++)
        {
            platforms[i] = (GameObject)Instantiate(platformPrefab, this.transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.IsGameOver())
        {
            timeSinceLastSpawned += Time.deltaTime;

            if (!GameController.instance.IsGameOver() && timeSinceLastSpawned >= SpawnRate)
            {
                timeSinceLastSpawned = 0;
                float spawnYPosition = Random.Range(platformMin, platformMax);
                float lastPlatformX = platforms[lastPlatform].transform.position.x;
                platforms[currentPlatform].transform.position = new Vector2(spawnXPosition + lastPlatformX, spawnYPosition);
                SetBonus(platforms[currentPlatform]);
                lastPlatform = currentPlatform;
                currentPlatform++;
                if (currentPlatform >= platformPoolSize)
                {
                    currentPlatform = 0;
                }
            }
        }
    }

    void SetBonus(GameObject platform)
    {
        Transform left = platform.transform.GetChild(1);
        Transform right= platform.transform.GetChild(2);

        instantiateRandom(left);
        instantiateRandom(right);
    }

    void instantiateRandom(Transform position)
    {
        int coin = Random.Range(0, 2);
        if (coin == 0)
        {
            Instantiate(bonusPrefab, position.position, Quaternion.identity);
        }
    }

}
