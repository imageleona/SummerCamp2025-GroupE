using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Tooltip("遷移先のシーン名")]
    public string nextSceneName = "Game";  // デフォルト値を入れてもOK

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Invoke("ChangeScene", 0.5f);
        }

    }

    void ChangeScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("次のシーン名が設定されていません。");
        }
    }
}
