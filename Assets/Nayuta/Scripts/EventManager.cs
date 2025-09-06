using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    [Header("制限時間（秒）")]
    public float gameDuration = 150f;  // 2分30秒 = 150秒

    [Header("タイムアップ後の遷移までの待機時間（秒）")]
    public float timeUpDelay = 5f;

    private bool timeUpTriggered = false;

    void Start()
    {
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameDuration);

        // タイムアップ表示
        Debug.Log("[EventManager] TIME UP!");

        timeUpTriggered = true;

        // 5秒待ってからResultシーンへ遷移
        yield return new WaitForSeconds(timeUpDelay);

        Debug.Log("[EventManager] Loading Result scene...");
        SceneManager.LoadScene("Result");
    }

    void Update()
    {
        // 追加で何か監視したいイベントがあればここに書ける
    }
}
