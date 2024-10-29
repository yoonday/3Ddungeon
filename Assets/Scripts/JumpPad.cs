using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float force;

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어의 Rigidbody 컴포넌트 가져오기
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // 현재 속도를 초기화하고 위쪽으로 힘을 가함
                rb.velocity = Vector3.zero; // 기존 속도를 초기화
                rb.AddForce(Vector3.up * force, ForceMode.Impulse); // Impulse로 순간적인 힘 가하기
            }
        }
    }
}
