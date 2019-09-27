using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject mountainsPrefab;
    public GameObject cloudsPrefab;
    public int poolSize;
    public float SpawnRate = 5f;
    public float cloudsYoffset = 2f;
    public float mountainsYoffset = -1f;
    public float xOffset = 20f;
    public float z = 50f;

    private GameObject[] mountains;
    private GameObject[] clouds;
    private float timeSinceLastSpawned;
    private int currentBack = 0;
    private int lastBack;
    private float spawnYPosition;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastSpawned = 0;
        spawnYPosition = transform.position.y;
        lastBack = poolSize - 1;

        mountains = new GameObject[poolSize];
        clouds = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            Vector3 offsetMountains = new Vector3(xOffset * i, this.transform.position.y + mountainsYoffset, z);
            mountains[i] = (GameObject)Instantiate(mountainsPrefab, this.transform.position + offsetMountains, Quaternion.identity);
            Vector3 offsetClouds = new Vector3(xOffset * i, this.transform.position.y + cloudsYoffset, z);
            clouds[i] = (GameObject)Instantiate(cloudsPrefab, this.transform.position + offsetClouds, Quaternion.identity); ;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.IsGameOver())
        {
            timeSinceLastSpawned += Time.deltaTime;

            if (/*!GameControl.instance.gameOver &&*/ timeSinceLastSpawned >= SpawnRate)
            {
                timeSinceLastSpawned = 0;
                float lastMountainsX = mountains[lastBack].transform.position.x;
                mountains[currentBack].transform.position = new Vector3(xOffset + lastMountainsX, spawnYPosition + mountainsYoffset, z);
                float lastCloudsX = clouds[lastBack].transform.position.x;
                clouds[currentBack].transform.position = new Vector3(xOffset + lastCloudsX, spawnYPosition + cloudsYoffset, z);

                lastBack = currentBack;
                currentBack++;
                if (currentBack >= poolSize)
                {
                    currentBack = 0;
                }
            }
        }
    }
}
