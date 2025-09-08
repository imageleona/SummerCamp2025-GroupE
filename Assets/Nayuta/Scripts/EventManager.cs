using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    [Header("制限時間（秒）")]
    public float gameDuration = 150f;  // 2分30秒

    [Header("タイムアップ後の遷移までの待機時間（秒）")]
    public float timeUpDelay = 5f;

    [Header("UI参照")]
    public Text timerText;      // 残り時間表示
    public Text timeUpText;     // タイムアップ表示

    private float remainingTime;
    private bool timeUpTriggered = false;

    void Start()
    {
        remainingTime = gameDuration;

        if (timerText != null)
            timerText.gameObject.SetActive(true);

        if (timeUpText != null)
            timeUpText.gameObject.SetActive(false);

        StartCoroutine(GameTimer());
    }

    void Update()
    {
        if (!timeUpTriggered)
        {
            remainingTime -= Time.deltaTime;
            remainingTime = Mathf.Max(remainingTime, 0);

            if (timerText != null)
                timerText.text = FormatTime(remainingTime);
        }
    }

    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameDuration);

        Debug.Log("[EventManager] TIME UP!");
        timeUpTriggered = true;

        if (timerText != null)
            timerText.gameObject.SetActive(false);

        if (timeUpText != null)
            timeUpText.gameObject.SetActive(true);

        yield return new WaitForSeconds(timeUpDelay);

        Debug.Log("[EventManager] Loading Result scene...");
        SceneManager.LoadScene("Result");
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
