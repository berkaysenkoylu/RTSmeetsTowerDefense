using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager enemySpawnManager;

    public Transform[] spawnTransforms;
    public GameObject zombieContainer;
    public float initialSpawnCooldown = 3f;
    public GameObject zombieDeathEffect;
    Queue<GameObject> zombieQueue = new Queue<GameObject>();

    [SerializeField]
    bool canSpawn = false;
    float spawningCooldown;
    float lastTimeSpawned;
    float rateOfChange = 0.1f;

    public static EnemySpawnManager instance
    {
        get
        {
            if (!enemySpawnManager)
            {
                enemySpawnManager = FindObjectOfType(typeof(EnemySpawnManager)) as EnemySpawnManager;

                if (!enemySpawnManager)
                {
                    Debug.LogError("There needs to be one active EnemySpawnManager script on a GameObject in your scene.");
                }
            }

            return enemySpawnManager;
        }
    }

    private void Awake()
    {
        InitializeZombiePool();
    }

    void Start()
    {
        spawningCooldown = initialSpawnCooldown;

        Sun.canEnemiesBeSpawned += SetCanSpawn;

        Sun.dayCountIncreased += UpdateSpawnCooldown;

        Zombie.zombieIsDead += AddZombieToPool;
    }

    private void OnDestroy()
    {
        Sun.canEnemiesBeSpawned -= SetCanSpawn;

        Sun.dayCountIncreased -= UpdateSpawnCooldown;

        Zombie.zombieIsDead -= AddZombieToPool;
    }

    void Update()
    {
        if (canSpawn && Time.time - spawningCooldown >= lastTimeSpawned)
        {
            SpawnProcess();
        }
    }

    void SetCanSpawn(bool canEnemiesBeSpawned)
    {
        canSpawn = canEnemiesBeSpawned;
    }

    void UpdateSpawnCooldown(int dayCount)
    {
        // If spawn cooldown is already smaller than or equal to
        // the minimum value it can have, no need to do calculations
        if (spawningCooldown <= 0.5f)
        {
            return;
        }

        // As the days go by, zombies will spawn more frequently
        spawningCooldown = initialSpawnCooldown * Mathf.Pow((1 - rateOfChange), dayCount);

        // Clamp the spawn cooldown within the values between 0.5f and 3f
        spawningCooldown = Mathf.Clamp(spawningCooldown, 0.5f, 3f);
    }

    void InitializeZombiePool()
    {
        for (int i = 0; i < zombieContainer.transform.childCount; i++)
        {
            // ZombieQueue is a pool that includes all the inactive zombies
            zombieQueue.Enqueue(zombieContainer.transform.GetChild(i).gameObject);
        }
    }

    public void AddZombieToPool(GameObject zombie)
    {
        zombie.SetActive(false);

        Instantiate(zombieDeathEffect, zombie.transform.position, Quaternion.identity);

        zombieQueue.Enqueue(zombie);
        //foreach(GameObject deactiveZomboi in zombieQueue.ToArray())
        //{
        //    Debug.Log(deactiveZomboi.name);
        //}
    }

    void SpawnProcess()
    {
        // Take a zombie from the pool (queue)
        GameObject zombie = zombieQueue.Dequeue();

        // Activate the object
        zombie.SetActive(true);

        // Get a random index
        int randomIndex = Random.Range(0, spawnTransforms.Length);

        // Get a random position
        Vector3 spawnLocation = spawnTransforms[randomIndex].position;

        // Set the zombie location to spawn location
        zombie.transform.position = new Vector3(spawnLocation.x, zombie.transform.position.y, spawnLocation.z);

        lastTimeSpawned = Time.time;
    }
}
