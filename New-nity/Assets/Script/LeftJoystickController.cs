using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftJoystickController : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("움직일 대상")]
    public Transform Character;

    [Header("JoyStick")]
    [Tooltip("움직일 대상의 움직임을 제어하는 왼쪽 스틱")]
    [SerializeField] private RectTransform LeftFiledStick;

    [Tooltip("왼쪽 조이스틱의 뒷 배경")]
    [SerializeField] private GameObject LeftJoyPad;
    [SerializeField] private RectTransform LeftBackBoard;

    private float Radius = 0.0f;

    private bool TouchCheck = false;

    private float Speed = 0.0f;

    private Vector2 Direction;

    private Vector3 Movement;

    private Vector3 Rotation;

    private void Awake()
    {
        LeftFiledStick = GameObject.Find("LeftFiledCircle").GetComponent<RectTransform>();

        LeftJoyPad = GameObject.Find("LeftOutLineCircle");
        LeftBackBoard = LeftJoyPad.GetComponent<RectTransform>();
    }

    void Start()
    {
        Radius = (LeftBackBoard.rect.width * 0.5f);
    }

    void Update()
    {
        
    }
}