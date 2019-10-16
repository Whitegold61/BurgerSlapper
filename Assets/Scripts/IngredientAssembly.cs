using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientAssembly : MonoBehaviour
{

    private Transform burg;
    public List<Transform> trackIngredients;
    private List<float> ingredientTimes;

    void adjustTrigger() {
        int ingredientCount = trackIngredients.Count + transform.GetChild(0).childCount;
        gameObject.GetComponents<BoxCollider>()[1].center = new Vector3(0f, 0f, 0f + 0.25f * ingredientCount);
        gameObject.GetComponents<BoxCollider>()[1].size = new Vector3(2.8f, 2.8f, 0.8f + 0.5f * ingredientCount);

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Ingredient")) {
            trackIngredients.Add(other.transform);
            ingredientTimes.Add(0f);
            adjustTrigger();

        }
    }

    private void OnTriggerExit(Collider other) {
        if (trackIngredients.Contains(other.transform)) {
            ingredientTimes.Remove(trackIngredients.IndexOf(other.transform));
            trackIngredients.Remove(other.transform);
            adjustTrigger();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        burg = transform.GetChild(0);
        trackIngredients = new List<Transform>();
        ingredientTimes = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < trackIngredients.Count; i++) {
            if (trackIngredients[i].GetComponent<Rigidbody>().velocity.magnitude <= 0f &&
                !trackIngredients[i].GetComponent<Rigidbody>().isKinematic) {
                ingredientTimes[i] += Time.deltaTime;
                if (ingredientTimes[i] > 1f) {
                    trackIngredients[i].GetComponent<Rigidbody>().isKinematic = true;
                    trackIngredients[i].SetParent(transform.GetChild(0));
                    trackIngredients[i].gameObject.tag = "Untagged";
                    trackIngredients[i].gameObject.layer = 0;
                    trackIngredients.RemoveAt(i);
                }
            } else if (trackIngredients[i].GetComponent<Rigidbody>().velocity.magnitude > 0f) {
                ingredientTimes[i] = 0f;
            }
        }

        // for (int i = 0; i < burg.childCount; i++) {

        // }
    }
}
