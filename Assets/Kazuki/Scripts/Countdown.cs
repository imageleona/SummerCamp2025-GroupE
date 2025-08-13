using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text timerText;       // UI��Text��Inspector�Őݒ�
    public int currentTime = 10; // �������ԁi�b�j
    private float timer = 1f;    // 1�b���Ƃ̃J�E���g�p

    void Update()
    {
        timer -= Time.deltaTime; // �o�ߎ��Ԃ����炷

        if (timer <= 0f && currentTime > 0)
        {
            currentTime--; // 1�b�o�߂��Ƃ�-1
            timer = 1f;    // �J�E���g���Z�b�g
            timerText.text = "�c��" + currentTime.ToString() + "�b"; // UI�X�V

            if (currentTime <= 0)
            {
                timerText.text = "Time Up!";
                Debug.Log("�Q�[���I��");
            }
        }
    }
}
