using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{

    [SerializeField] private GameObject[] ingredients;
    [SerializeField] private Sprite[] customerSprites;

    float posTween = 0f;
    bool isActive = false;
    float cooldown = 1f;
    Vector3 originalPos;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Burger")) {
            Debug.Log("destroy");
            Transform killBurger = other.transform.parent;
            while (killBurger.childCount > 0) {
                killBurger.GetChild(0).gameObject.AddComponent<Rigidbody>();
                killBurger.GetChild(0).parent = killBurger.parent;
            }
           // GameObject.Destroy(killBurger.gameObject);
            cooldown = 5f;
            isActive = false;
            Debug.Log("ouch.");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Burger") && isActive) {
            Debug.Log("destroy " + other.gameObject.name);
            Transform killBurger = other.transform.parent;
            while (killBurger.childCount > 0) {
                killBurger.GetChild(0).gameObject.AddComponent<Rigidbody>();
                killBurger.GetChild(0).GetComponent<Rigidbody>().AddForce(
                    new Vector3(Random.Range(-8f, -3f), Random.Range(10f, 15f) * 1 - (Random.Range(0, 2) * 2), Random.Range(10f, 15f) * 1 - (Random.Range(0, 2) * 2)), ForceMode.Impulse);
                killBurger.GetChild(0).GetComponent<Rigidbody>().AddTorque(
                    new Vector3(Random.Range(5f, 15f) * 1 - (Random.Range(0, 2) * 2), Random.Range(5f, 15f) * 1 - (Random.Range(0, 2) * 2), Random.Range(5f, 15f) * 1 - (Random.Range(0, 2) * 2)), ForceMode.Impulse);
                killBurger.GetChild(0).parent = killBurger.parent;
            }
            // GameObject.Destroy(killBurger.gameObject);
            cooldown = 5f;
            isActive = false;
            Debug.Log("ouch.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0f) {
            cooldown = Mathf.Max(0f, cooldown - Time.deltaTime);
            if (cooldown <= 0f) {
                isActive = true;
                transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = customerSprites[Random.Range(0, customerSprites.Length)];
                Transform bubble = transform.GetChild(0);
                while (bubble.childCount > 0)
                    GameObject.DestroyImmediate(bubble.GetChild(0).gameObject);
                int ingredientCount = Random.Range(3, 6);
                bool hasMeat = false;
                for (int i = 0; i < ingredientCount; i++) {
                    int ingredientIndex = Random.Range(2, ingredients.Length);
                    if (i == 0)
                        ingredientIndex = 0;
                    else if (i == ingredientCount - 1)
                        ingredientIndex = 1;
                    if (i == 2 || i == 3)
                        hasMeat = true;
                    if (i == ingredientCount - 2 && !hasMeat)
                        ingredientIndex = Random.Range(2, 3);
                    Transform ingredientSprite = GameObject.Instantiate(ingredients[ingredientIndex], bubble).transform;
                    ingredientSprite.localPosition = new Vector3(0f, -0.65f + 0.4f * i, 0f);
                    ingredientSprite.GetComponent<SpriteRenderer>().sortingOrder = i + 2;
                }
            }
        }
        if (isActive && posTween < 1f) {
            posTween = Mathf.Min(1f, posTween + Time.deltaTime);
            transform.position = originalPos - new Vector3(MezzMath.halfSine(posTween) * 7f, 0f, 0f);
        } else if (!isActive && posTween > 0f) {
            posTween = Mathf.Max(0f, posTween - Time.deltaTime * 2f);
            transform.position = originalPos - new Vector3(posTween * 7f, 0f, 0f);
        }
    }
}
