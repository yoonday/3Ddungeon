using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnPlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // ������ �ڽ����� �����Ͽ� �Բ� �̵�
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // ���ǿ��� ����� �θ�-�ڽ� ���� ����
            transform.SetParent(null);
        }
    }
}
