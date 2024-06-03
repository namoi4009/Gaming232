using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLevelHandler : MonoBehaviour
{
    [SerializeField]
    GameObject[] sectionsPrefabs;

    GameObject[] sectionsPool = new GameObject[30];

    // Choose from Section Pool to appear on game
    GameObject[] sections = new GameObject[10];

    Transform playerCarTransform;

    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);

    const float sectionLength = 24;

    // Start is called before the first frame update
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        // Create prefab pool
        for (int i = 0; i < sectionsPool.Length; i++)
        {
            sectionsPool[i] = Instantiate(sectionsPrefabs[prefabIndex]);
            sectionsPool[i].SetActive(false);

            prefabIndex++;

            // Reset prefabIndex if it exceeds the limit
            if (prefabIndex > sectionsPrefabs.Length - 1)
                prefabIndex = 0;
        }

        for (int i = 0; i < sections.Length; i++)
        {
            // Get a random section
            GameObject randomSection = GetRandomSectionFromPool();

            // Move the section into position and set ACTIVE
            randomSection.transform.position = new Vector3(sectionsPool[i].transform.position.x, -10, i * sectionLength);
            randomSection.SetActive(true);

            // Set the section chosen into the array
            sections[i] = randomSection;
        }

        StartCoroutine(UpdateLessOftenCO());

    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            UpdateSectionPosition();
            yield return waitFor100ms;
        }
    }

    void UpdateSectionPosition()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            // Check if the section is too far behind
            if (sections[i].transform.position.z - playerCarTransform.position.z < -sectionLength)
            {
                // Store the position and disable
                Vector3 lastSectionPosition = sections[i].transform.position;
                sections[i].SetActive(false);

                // Get new section
                sections[i] = GetRandomSectionFromPool();

                // Move the new section and activate it
                sections[i].transform.position = new Vector3(lastSectionPosition.x, -10, lastSectionPosition.z + sectionLength * sections.Length);
                sections[i].SetActive(true);
            }
        }
    }

    GameObject GetRandomSectionFromPool()
    {
        // Pick a random index
        int randomIndex = Random.Range(0, sectionsPool.Length);
        
        bool isNewSectionFound = false;

        while (!isNewSectionFound)
        {
            if (!sectionsPool[randomIndex].activeInHierarchy)
                isNewSectionFound = true;
            else
            {
                randomIndex++;
                if (randomIndex > sectionsPool.Length - 1)
                    randomIndex = 0;
            }
        }

        return sectionsPool[randomIndex];
    }
}
