/*
 * 
 * Developed by Olusola Olaoye, 2024
 * 
 * To only be used by those who purchased from the Unity asset store
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NumberInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Drag axis determines how the mouse movement will affect the number's increment or decrement when moving
    public enum DragAxis
    {
        X, // number will increment or decrement based on mouse's left right movement
        Y, // number will increment or decrement based on mouse's forward backward movement
        XY, // number will increment when mouse moves forward or rightward and will decrement when mouse moves leftwards or backwards
        NONE // number will not be affected by mouse movement
    }

    [SerializeField]
    [Tooltip("Drag axis determines how the mouse movement will affect the number's increment or decrement when moving")]
    private DragAxis drag_axis;


    // textures for the cursor depending on the type of drag axis we have
    [SerializeField]
    private Texture2D scale_horizontal_texture;

    [SerializeField]
    private Texture2D scale_vertical_texture;

    [SerializeField]
    private Texture2D scale_XY_texture;



    [SerializeField]
    private MouseOverButton decrement_button; // button to decrement value

    [SerializeField]
    private MouseOverButton increment_button; // button to increment value


    [SerializeField]
    [Tooltip("the initial value of the number field on start")]
    private float initial_value = 0; // the initial value of the number field on start

    [SerializeField]
    [Tooltip("// the minimum value the number should be")]
    private float minimum_value = 0; // the minimum value the number should be

    [SerializeField]
    [Tooltip("// the maximum value the number should be")]
    private float maximum_value = 100; // the maximum value the number should be


    [SerializeField]
    private TMPro.TMP_InputField input_field; // the input field


    [SerializeField]
    [Range(0,4)]
    [Tooltip("// the number of decimal places that the number should have")]
    private int decimal_places; // the number of decimal places that the number should have

    


    [SerializeField]
    [Tooltip("the value which we increment and decrement our number with, everytime we click on the increment or decrement button")]
    private float change_factor = 1; // the value which we increment and decrement our number with, everytime we click on the 
                                     // increment or decrement button


    // when the user long clicks on the increment or decrement button, the number increases  or decreases by change factor
    // this value determines how frequent the long click action will be invoked
    // so a long_click_invoke_rate of 1 will be 10x slower than a long_click_inverse_speed of 0.1.
    [SerializeField]
    [Tooltip("when the user long clicks on the increment or decrement button, the number increases  or decreases by change factor. This value determines how frequent the long click action will be invoked")]
    [Range(0.05f,1)] 
    private float long_click_invoke_rate = 0.1f
;


    // the number that will be displayed
    private float number;

    // if mouse is over this UI object
    private bool is_mouse_on; 


    private bool invoking_drag_action;

    // Start is called before the first frame update
    void Start()
    {

        number = initial_value; // assign initial value to number

        number = Mathf.Clamp(number, minimum_value, maximum_value); // clamp number

        input_field.text = number.ToString();


        input_field.onValueChanged.AddListener(delegate 
        {
            number = float.Parse(input_field.text); // update the number value everytime the value changes via text update
        });


        // assign the rate at which the long click action will be called on both the increment and the decrement buttons
        increment_button.invoke_rate = long_click_invoke_rate;
        decrement_button.invoke_rate = long_click_invoke_rate;


        decrement_button.button.onClick.AddListener(() => 
        {
            number -= change_factor;
            clampNumberAndSetInputField();
        });

        increment_button.button.onClick.AddListener(() =>
        {
            number += change_factor ;
            clampNumberAndSetInputField();
        });


        decrement_button.long_press_action = ()=>
        {
            number -= change_factor;
            clampNumberAndSetInputField();
        };

        increment_button.long_press_action = () =>
        {
            number += change_factor;
            clampNumberAndSetInputField();
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("adjustValueWithPointerDrag");

        }


        if (is_mouse_on && Input.GetMouseButtonDown(0))
        {
            invoking_drag_action = true;

        }


        if (invoking_drag_action)
        {
            InvokeRepeating("adjustValueWithPointerDrag", 0, long_click_invoke_rate/3);

            invoking_drag_action = false;
        }

    }


    // this will make sure that the value changes .........
    private void adjustValueWithPointerDrag()
    {
        
        switch (drag_axis)
        {
            case DragAxis.X:

                // increment or decrement the number by change factor depending on the direction of the mouse movement in the x axis
                number += Input.GetAxis("Mouse X") > 0 ? change_factor : Input.GetAxis("Mouse X") < 0 ? -change_factor: 0;

                break;


            case DragAxis.Y:

                // increment or decrement the number by change factor depending on the direction of the mouse movement in the y axis
                number += Input.GetAxis("Mouse Y") > 0 ? change_factor : Input.GetAxis("Mouse Y") < 0 ? -change_factor : 0;

                break;


            case DragAxis.XY:

                // increment or decrement the number by change factor depending on the direction of the mouse movement in the x and y axis
                number += Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0 ? change_factor 
                        : Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse Y") < 0 ? -change_factor 
                        : 0;

                break;
        }

        // clamp the number
        number = Mathf.Clamp(number, minimum_value, maximum_value);

        // format number based on the number of decimal places
        input_field.text = number.ToString($"n{decimal_places}");
    }


    // update cursor texture depending on the drag axis type we have
    public void updateCursur()
    {
        switch (drag_axis)
        {
            case DragAxis.X:

                Cursor.SetCursor(scale_horizontal_texture, Vector2.zero, CursorMode.Auto);
                break;

            case DragAxis.Y:
                Cursor.SetCursor(scale_vertical_texture, Vector2.zero, CursorMode.Auto);
                break;


            case DragAxis.XY:
                Cursor.SetCursor(scale_XY_texture, Vector2.zero, CursorMode.Auto);
                break;


            case DragAxis.NONE:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
        }
    }


    // if mouse pointer is over this UI object
    public void OnPointerEnter(PointerEventData eventData)
    {
        is_mouse_on = true;

        updateCursur();
    }

  
    // if mouse pointer exits this UI object
    public void OnPointerExit(PointerEventData eventData)
    {
        is_mouse_on = false;

        // set cursor texture back to default
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    // clamp the number value and assign the value to the input field's text
    private void clampNumberAndSetInputField()
    {
        // clamp number between min and max value
        number = Mathf.Clamp(number, minimum_value, maximum_value);

        // set input field text and format the text basd on the number of decimal places
        input_field.text = number.ToString($"n{decimal_places}");
    }

}