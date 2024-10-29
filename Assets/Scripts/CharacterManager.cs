using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance; 
    public static CharacterManager Instance 
    {
        get // get�� ���� _instance�� ����
        {
            if (_instance == null) // ����ڵ� : CharacterManager�� �ν��Ͻ��� ���� ���� ���� ��츦 ���
            {
                _instance = new GameObject(nameof(CharacterManager)).AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    public Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private void Awake() 
    {
        if (_instance == null)
        {
            _instance = this; // �� �ڽ� ����ֱ�
            DontDestroyOnLoad(gameObject); //�� �̵��ص� ���� ������ �����ǰ� ��
        }
        else // _instance�� null�� �ƴ� �� 
        {
            if (_instance != this) // ���� �ٸ��ٸ� �ı��ϱ�)
            {
                Destroy(gameObject);
            }
        }
    }
}
