using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectablePhys : MonoBehaviour
{

    [SerializeField] bool freezeOnSelection = false;
    [SerializeField] int selectionLayer;
    [SerializeField] private UnityEvent onSelected = new UnityEvent();
    [SerializeField] private UnityEvent onDeselected = new UnityEvent();
    private bool isSelected;

    void selectionFreeze() {

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
