using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuObject : MonoBehaviour
{
    public string loadLevel;

    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene(loadLevel);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
