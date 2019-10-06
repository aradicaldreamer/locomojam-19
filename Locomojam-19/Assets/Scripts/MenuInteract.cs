using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuInteract : MonoBehaviour{

    public EventSystem eventSystem;
    public GameObject selectedObject;

    public bool buttonSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        SelectMenuItem();
        //Debug.Log(Input.GetJoystickNames());
        //Debug.Log(Input.IsJoystickPreconfigured());
    }
    public void OnDisable() {
        buttonSelected = false;
    }

    public void SelectMenuItem() {
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false) {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
        if (buttonSelected == false) {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }
}
