using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;         // 이동 속도
    public float rotationSpeed = 720f;   // 회전 속도 (도/초)

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // 좌우
        float moveZ = Input.GetAxis("Vertical");   // 상하

        Vector3 inputDirection = new Vector3(moveX, 0f, moveZ);

        if (inputDirection.sqrMagnitude > 0.001f)
        {
            // 1. 이동 방향 → 평면 회전 (y축만 회전)
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            Vector3 euler = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(0f, euler.y, 0f); // y축만 회전

            // 2. 부드럽게 회전
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // 3. forward 방향으로 이동
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
