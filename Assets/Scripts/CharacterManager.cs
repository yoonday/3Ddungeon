using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance; 
    public static CharacterManager Instance 
    {
        get // get을 통해 _instance에 접근
        {
            if (_instance == null) // 방어코드 : CharacterManager의 인스턴스가 게임 씬에 없을 경우를 대비
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
            _instance = this; // 나 자신 집어넣기
            DontDestroyOnLoad(gameObject); //씬 이동해도 가진 정보가 유지되게 함
        }
        else // _instance가 null이 아닐 때 
        {
            if (_instance != this) // 나와 다르다면 파괴하기)
            {
                Destroy(gameObject);
            }
        }
    }
}
