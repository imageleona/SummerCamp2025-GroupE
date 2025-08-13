using UnityEngine;

public class WASDMovements : MonoBehaviour
{
    public float moveSpeed = 3.0f; // �ړ����x

    void Update()
    {
        // ���͂��擾
        float horizontal = Input.GetAxis("Horizontal"); // A,D�L�[ �� -1 �` 1
        float vertical = Input.GetAxis("Vertical");     // W,S�L�[ �� -1 �` 1

        // �ړ��x�N�g��
        Vector3 direction = new Vector3(horizontal, 0, vertical);

        // �J�����̌����Ɉˑ������A���[���h���W�ňړ�
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }
}

