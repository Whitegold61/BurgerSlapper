using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientShaderControl : MonoBehaviour
{
    float decayRate = 2f;

    Material ingredientMat;
    float flopDecay = 0f;
    public void selectIngredient(bool wasSelected) {
        ingredientMat.SetInt("Selected", wasSelected ? 1 : 0);

        flopDecay = 0.5f;
    }

    public void setFlop() {
        flopDecay = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        ingredientMat = gameObject.GetComponent<Material>();
    }

    // Update is called once per frame
    void Update() {
        if (flopDecay > 0f) {
            flopDecay = Mathf.Max(0f, flopDecay - Time.deltaTime * decayRate);
            ingredientMat.SetFloat("FlopAmount", flopDecay);
        }
    }
}
