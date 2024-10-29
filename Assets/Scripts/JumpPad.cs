using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float force;

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾��� Rigidbody ������Ʈ ��������
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // ���� �ӵ��� �ʱ�ȭ�ϰ� �������� ���� ����
                rb.velocity = Vector3.zero; // ���� �ӵ��� �ʱ�ȭ
                rb.AddForce(Vector3.up * force, ForceMode.Impulse); // Impulse�� �������� �� ���ϱ�
            }
        }
    }
}
