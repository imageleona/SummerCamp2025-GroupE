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
using UnityEditor;

// editor script to create the Number input
public class CreateXYController
{


    [MenuItem("GameObject/Number Input/Number Input 1", false, -1)]
    private static void createNumberInput1()
    {
        createPrefabResource("Number Input 1");
    }


    [MenuItem("GameObject/Number Input/Number Input 2", false, -1)]
    private static void createNumberInput2()
    {
        createPrefabResource("Number Input 2");
    }


    [MenuItem("GameObject/Number Input/Number Input 3", false, -1)]
    private static void createNumberInput3()
    {
        createPrefabResource("Number Input 3");
    }


    [MenuItem("GameObject/Number Input/Number Input 4", false, -1)]
    private static void createNumberInput4()
    {
        createPrefabResource("Number Input 4");
    }


    [MenuItem("GameObject/Number Input/Number Input 5", false, -1)]
    private static void createNumberInput5()
    {
        createPrefabResource("Number Input 5");
    }


    [MenuItem("GameObject/Number Input/Number Input 6", false, -1)]
    private static void createNumberInput6()
    {
        createPrefabResource("Number Input 6");
    }



    private static void createPrefabResource(string object_name)
    {

        Object ui_object = Resources.Load("Prefabs/" + object_name); // find prefab in resources

        GameObject ui_game_object = (GameObject)GameObject.Instantiate(ui_object, Vector3.zero, Quaternion.identity); // instantiate ui number input

        ui_game_object.name = object_name; // name object



        GameObject selected_object = Selection.activeGameObject; // current selected game object


        if (selected_object) // if selected object is not null
        {

            if (selected_object.GetComponent<RectTransform>()) // if there is a current seleted game object and that game object has a RectTransform component
            {
                ui_game_object.transform.SetParent(selected_object.transform, false);
            }

            else// if there is a current seleted game object and that game object does not have a RectTransform component
            {
                GameObject canvas = createCustomCanvasObject(); // create a canvas object

                canvas.transform.SetParent(selected_object.transform); // set canvas object to child of selected game object

                ui_game_object.transform.SetParent(canvas.transform, false); // set the number input ui to the child of the canvas
            }

        }
        else // if selected object is null
        {
            // create a canvas object if none exists
            GameObject canvas = GameObject.FindObjectOfType<Canvas>() ? GameObject.FindObjectOfType<Canvas>().gameObject : createCustomCanvasObject();

            // set the number input object to the child of the canvas
            ui_game_object.transform.SetParent(canvas.transform, false);
        }




        Selection.activeGameObject = ui_game_object;
    }


    private static GameObject createCustomCanvasObject()
    {
        // create a gameobject with all the canvas components attached 
        GameObject canvas_object = new GameObject("Canvas", new System.Type[] { typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster) });

        canvas_object.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay; // set canvas to screen space mode

        canvas_object.layer = 5; // Ui layer

        return canvas_object;
    }
}