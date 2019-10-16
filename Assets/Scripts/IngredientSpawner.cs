using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] ingredients;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnVariance = 0.2f;
    int spawnCount;
    private int lastSpawn = 0;
    private List<Transform> spawned;

    float spawnDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        spawned = new List<Transform>();
        spawnCount = transform.childCount;
    }

    // Update is called once per frame
    void Update() {
        spawnDelay = spawnDelay - Time.deltaTime;
        if (spawnDelay <= 0f) {
            spawnDelay = spawnInterval + Random.Range(-spawnVariance, spawnVariance);
            Transform spawnPos = transform.GetChild(Random.Range(0, spawnCount));
            int ingredientType = Random.Range(0, ingredients.Length);
            spawned.Add(
                GameObject.Instantiate(ingredients[ingredientType], spawnPos.position, Quaternion.Euler(new Vector3(-120f, 0f, 0f)), transform)
                .transform);
            spawned[spawned.Count - 1].name = ingredients[ingredientType].name;
            int xMult = (spawnPos.localPosition.x < 0f) ? 1 : -1;
            spawned[spawned.Count - 1].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(15f, 20f) * xMult, Random.Range(0f, 4f), 0f), ForceMode.Impulse);
            spawned[spawned.Count - 1].GetComponent<Rigidbody>().AddTorque(new Vector3(
                                Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), ForceMode.Impulse);
            // object, position, rotation, parent
        }
        for (int i = 0; i < spawned.Count; i++) {
            if (spawned[i]) {
                if (spawned[i].position.y < -5f) {
                    Transform marked = spawned[i];
                    spawned.RemoveAt(i);
                    GameObject.Destroy(marked.gameObject);
                }
            } else {
                spawned.RemoveAt(i);
            }
        }
    }
}
