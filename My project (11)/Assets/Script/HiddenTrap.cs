using System.Collections;
using UnityEngine;

public class HiddenTrap : MonoBehaviour
{
    [Header("함정 설정")]
    public Transform trapBody;      // 실제로 튀어오를 뾰족한 함정 오브젝트
    public float springHeight = 2f; // 위로 튀어오를 높이
    public float springSpeed = 15f; // 튀어오르는 속도 (빠를수록 위협적)
    public float returnSpeed = 2f;  // 제자리로 돌아가는 속도 (천천히 복귀)
    public float holdTime = 1f;     // 올라온 상태로 유지되는 시간

    private Vector3 originalPos;
    private Vector3 targetPos;
    private bool isTriggered = false;

    private void Start()
    {
        // 시작할 때 함정의 원래 위치와 목표 위치를 저장해둡니다.
        if (trapBody != null)
        {
            originalPos = trapBody.position;
            targetPos = originalPos + Vector3.up * springHeight;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 감지 영역(Trigger)에 들어왔고, 함정이 쉬는 상태라면 작동!
        if (collision.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(ActivateTrap());
        }
    }

    private IEnumerator ActivateTrap()
    {
        isTriggered = true;

        // 1. 위로 빠르게 튀어오름
        while (Vector3.Distance(trapBody.position, targetPos) > 0.01f)
        {
            trapBody.position = Vector3.MoveTowards(trapBody.position, targetPos, springSpeed * Time.deltaTime);
            yield return null; // 다음 프레임까지 대기
        }
        trapBody.position = targetPos; // 오차 보정

        // 2. 튀어오른 상태로 잠시 대기
        yield return new WaitForSeconds(holdTime);

        // 3. 서서히 원래 자리로 복귀
        while (Vector3.Distance(trapBody.position, originalPos) > 0.01f)
        {
            trapBody.position = Vector3.MoveTowards(trapBody.position, originalPos, returnSpeed * Time.deltaTime);
            yield return null;
        }
        trapBody.position = originalPos; // 오차 보정

        // 4. 다시 작동할 수 있도록 초기화
        isTriggered = false;
    }
}