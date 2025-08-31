using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public double countdownMinutes = 0.1;
    public double countdownSeconds;
    public Text timeText;
    public Text finishText;

    private void Start()
    {
        finishText.enabled = false; //テキスト非表示
        countdownSeconds = countdownMinutes * 60;
    }

    void Update()
    {
        if (countdownSeconds > 0)
        {
            countdownSeconds -= Time.deltaTime;
            var span = new TimeSpan(0, 0, (int)countdownSeconds);
            timeText.text = span.ToString(@"mm\:ss");
        }

        if (countdownSeconds <= 0)
        {
            // 0秒になったときの処理
            finishText.enabled = true; //テキスト表示
            finishText.text = "TIME UP!!";
        }
    }
}