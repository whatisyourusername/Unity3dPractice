using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;         // �̵� �ӵ�
    public float rotationSpeed = 720f;   // ȸ�� �ӵ� (��/��)

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // �¿�
        float moveZ = Input.GetAxis("Vertical");   // ����

        Vector3 inputDirection = new Vector3(moveX, 0f, moveZ);

        if (inputDirection.sqrMagnitude > 0.001f)
        {
            // 1. �̵� ���� �� ��� ȸ�� (y�ุ ȸ��)
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            Vector3 euler = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(0f, euler.y, 0f); // y�ุ ȸ��

            // 2. �ε巴�� ȸ��
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // 3. forward �������� �̵�
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
