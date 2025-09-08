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
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Button))]
public class MouseOverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler // this class tells us if cursur is above the object it is attached to
{
    // reference to the button component
    public Button button;


    // if mouse is over this UI object
    private bool is_mouse_over;


    // action when mouse is pressed on this object for long enough
    public System.Action long_press_action
    {
        get;
        set;
    }

    // delay before long press action starts executing.
    // Meaning that for each time the user long clicks on this object, there is a delay before action is invoked
    // it is advised you leave this number as 1.1 because it matches the delay speed in regular programs
    private float delay=1.1f;


    //counter to keep track of long press time
    private float counter = 0;


    // the rate at which the long press action will keep invoking
    public float invoke_rate
    {
        get;
        set;
    } = 0;



    private bool did_click_on_this_button;

    private void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("invokeLongPressAction");

            did_click_on_this_button = false;
        }


        if (is_mouse_over && Input.GetMouseButtonDown(0))
        {
            did_click_on_this_button = true;
        }


        if (did_click_on_this_button)
        {
            if (counter > delay)
            {
                InvokeRepeating("invokeLongPressAction", 0, invoke_rate);

                counter = 0;
            }
            else
            {
                counter += Time.deltaTime;
            }
        }

    }


    private void invokeLongPressAction()
    {
        long_press_action?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        is_mouse_over = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        is_mouse_over = false;
    }


}
