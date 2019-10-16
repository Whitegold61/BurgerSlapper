using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowScript : MonoBehaviour {

    [SerializeField] private Transform dragGraphic;

    Transform gameCam;
    bool isDragging = false;

    float initialX;
    float initialY;
    // store coords of transform start/end for a drag graphi

    Vector2 startPos;

    Vector2 vector3to2(Vector3 input) {
        return new Vector2(input.x, input.y);
    }

    float getAng(Vector2 start, Vector2 end) {
        return Mathf.Atan2(end.y - start.y, end.x - start.x) * 180f / Mathf.PI;
    }

    Vector2 getMousePos() {
        return vector3to2(gameCam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition));
    }

    // Start is called before the first frame update
    void Start()
    {
        gameCam = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update() {
        if (isDragging) {
            // Debug.Log("dragginnn");
            Debug.DrawRay(new Vector3(startPos.x, startPos.y, 0f), (new Vector3(getMousePos().x, getMousePos().y, 0f) - new Vector3(startPos.x, startPos.y, 0f)));
            dragGraphic.localPosition = Vector3.Lerp(new Vector3(startPos.x, startPos.y, 0f), new Vector3(getMousePos().x, getMousePos().y, 0f), 0.5f);
            dragGraphic.localRotation = Quaternion.Euler(0f, 0f, getAng(new Vector3(startPos.x, startPos.y, 0f), new Vector3(getMousePos().x, getMousePos().y, 0f)) + 90f);
            dragGraphic.localScale = new Vector3(0.01f, (new Vector3(getMousePos().x, getMousePos().y, 0f) - new Vector3(startPos.x, startPos.y, 0f)).magnitude * 0.5f, 0.01f);
        }
        if (Input.GetKey(KeyCode.Mouse0) && !isDragging) {
            isDragging = true;
            startPos = getMousePos();
        }
        else if (Input.GetKey(KeyCode.Mouse0) && isDragging) {
            Vector3 newPos = new Vector3(getMousePos().x, getMousePos().y, 0f);
            Vector3 oldPos = new Vector3(startPos.x, startPos.y, 0f);
            // Vector2 finalPos = newPos;
            // Vector2 startPosMod = startPos;
            // Vector2 centerPos = Vector2.Lerp(startPos, newPos, 0.5f);

            float throwAngle = getAng(oldPos, newPos) - 0f;

            bool foundIngredient = true;
            int iterations = 0;
            RaycastHit nextTarg;
            while (foundIngredient) {
                iterations++;
                if (iterations > 100) {
                    Debug.Log("fuck!");
                    break;
                }
                foundIngredient = false;
                Physics.Raycast(oldPos, (newPos - oldPos), out nextTarg, (oldPos - newPos).magnitude);
                if (nextTarg.collider) {
                    if (nextTarg.collider.gameObject.CompareTag("Ingredient")) {
                        Rigidbody hitIngredient = nextTarg.collider.GetComponent<Rigidbody>();
                        // hitIngredient.velocity = Vector3.zero;


                        hitIngredient.velocity = Vector3.zero;
                        Debug.Log(Mathf.Cos(throwAngle * Mathf.Deg2Rad) + ", " + Mathf.Sin(throwAngle * Mathf.Deg2Rad));
                        hitIngredient.AddForce(new Vector3(Mathf.Cos(throwAngle * Mathf.Deg2Rad), Mathf.Sin(throwAngle * Mathf.Deg2Rad), 0f) * 10f, ForceMode.Impulse);
                        isDragging = false;
                        dragGraphic.localScale = new Vector3(0.1f, 0f, 0.1f);
                        break;
                        // foundIngredient = true;

                    }
                    else if (nextTarg.collider.gameObject.CompareTag("Plate") && throwAngle > 0f) {
                        Transform plate = nextTarg.collider.transform;
                        if (plate.GetChild(0).childCount > 0f) {
                            Debug.Log("throw BURG");
                            for (int i = 0; i < plate.GetChild(0).childCount; i++) {
                                Destroy(plate.GetChild(0).GetChild(i).GetComponent<Rigidbody>());
                                plate.GetChild(0).GetChild(i).gameObject.layer = 12;
                                plate.GetChild(0).GetChild(i).gameObject.tag = "Burger";
                            }
                            plate.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                            plate.GetChild(0).GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Cos(throwAngle * Mathf.Deg2Rad), Mathf.Sin(throwAngle * Mathf.Deg2Rad), 0f) * 25f, ForceMode.Impulse);
                            plate.GetChild(0).GetComponent<Rigidbody>().AddTorque(new Vector3(
                                Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)), ForceMode.Impulse);
                            plate.GetChild(0).SetParent(transform.parent);
                            Transform newBurg = new GameObject("Burger").transform;
                            newBurg.SetParent(plate);
                            newBurg.localPosition = Vector3.zero;
                            // newBurg.tag = "Burger";
                            newBurg.gameObject.layer = 11;
                            newBurg.gameObject.AddComponent<Rigidbody>();
                            newBurg.GetComponent<Rigidbody>().isKinematic = true;
                            isDragging = false;
                            dragGraphic.localScale = new Vector3(0.1f, 0f, 0.1f);
                            break;
                        }
                    }
                }
            }
        }
        else if (!Input.GetKey(KeyCode.Mouse0) && isDragging) {
            isDragging = false;
            dragGraphic.localScale = new Vector3(0f, 0f, 0f);
        }
    }
}
