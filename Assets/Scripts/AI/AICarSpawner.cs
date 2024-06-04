using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarsSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] CarsAIPrefab;

    GameObject[] CarAIPool = new GameObject[20];

    Transform playerCarTransform;

    // Timing
    float timeLastCarSpawn = 0;
    WaitForSeconds wait = new WaitForSeconds(0.5f);

    // Overlapped Check
    [SerializeField]
    LayerMask otherCarsLayerMask;

    Collider[] overlappedCheckCollider = new Collider[1];

    // Start is called before the first frame update
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        for (int i = 0; i < CarAIPool.Length; i++)  
        {
            CarAIPool[i] = Instantiate(CarsAIPrefab[prefabIndex]);
            CarAIPool[i].SetActive(false);

            prefabIndex++;

            if (prefabIndex > CarsAIPrefab.Length - 1)
                prefabIndex = 0;
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    // Update is called once per frame
    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            CleanupCarsBeoyndView();
            SpawnNewCar();

            yield return wait;
        }
    }

    void SpawnNewCar()
    {
        if (Time.time - timeLastCarSpawn < 2)
            return;

        GameObject carToSpawn = null;

        // Choose a car to spawn
        foreach (GameObject aicar in CarAIPool)
        {
            if (aicar.activeInHierarchy)
                continue;

            carToSpawn = aicar;
            break;
        }

        // No car available to spawn
        if (carToSpawn == null)
            return;

        Vector3 spawnPosition = new Vector3(0, 0, playerCarTransform.transform.position.z + 100);

        // If there has been a aicar in that position, stop spawn
        if (Physics.OverlapBoxNonAlloc(spawnPosition, Vector3.one * 2, overlappedCheckCollider, Quaternion.identity, otherCarsLayerMask) > 0)
            return;
            

        carToSpawn.transform.position = spawnPosition;
        carToSpawn.SetActive(true);

        timeLastCarSpawn = Time.time;
    }

    void CleanupCarsBeoyndView()
    {
        foreach (GameObject aicar in CarAIPool)
        {
            // Skip inactive car
            if (!aicar.activeInHierarchy) continue;

            // Clean car which is too far away
            if (aicar.transform.position.z - playerCarTransform.position.z > 200)
                aicar.SetActive(false);

            // Clean car which is too far behind
            if (aicar.transform.position.z - playerCarTransform.position.z < -50)
                aicar.SetActive(false);
        }
    }
}
