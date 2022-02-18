using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickController : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("움직일 대상")]
    public Transform Character;

    [Header("RightJoyStick")]
    [Tooltip("움직일 대상의 움직임을 제어하는 오른쪽 스틱")]
    [SerializeField] protected RectTransform RightFiledStick;
    [SerializeField] protected int RightStickID; // 우측 조이스틱 구별하는 고유 ID 지정

    [Tooltip("오른쪽 조이스틱의 뒷 배경")]
    [SerializeField] protected GameObject RightJoyPad;
    [SerializeField] protected RectTransform RightBackBoard;

    [Header("LeftJoyStick")]
    [Tooltip("움직일 대상의 움직임을 제어하는 왼쪽 스틱")]
    [SerializeField] protected RectTransform LeftFiledStick;
    [SerializeField] protected int LeftStickID; // 좌측 조이스틱 구별하는 고유 ID 지정

    [Tooltip("왼쪽 조이스틱의 뒷 배경")]
    [SerializeField] protected GameObject LeftJoyPad;
    [SerializeField] protected RectTransform LeftBackBoard;

    [SerializeField] protected bool TouchCheck;

    // 조이스틱 반지름
    private float Radius = 0.0f;

    // 타겟의 이동 속도
    private float Speed = 0.0f;

    // 타겟이 이동할 방향 및 속도 등을 반영할 임시 포지션 (조이스틱의 2차원 값을 3차원으로 변형시키기 위함)
    private Vector3 Movement;

    // 타겟이 바라볼 방향
    private Vector3 Rotation;

    private void Awake()
    {
        // 조이스틱의 패드를 받아옴
        RightFiledStick = GameObject.Find("RightFiledCircle").GetComponent<RectTransform>();

        // 조이스틱의 부모객체 (빈 게임 오브젝트)
        RightJoyPad = GameObject.Find("RightOutLineCircle");
        RightBackBoard = RightJoyPad.GetComponent<RectTransform>();

        // 조이스틱의 패드를 받아옴
        LeftFiledStick = GameObject.Find("LeftFiledCircle").GetComponent<RectTransform>();

        // 조이스틱의 부모객체 (빈 게임 오브젝트)
        LeftJoyPad = GameObject.Find("LeftOutLineCircle");
        LeftBackBoard = LeftJoyPad.GetComponent<RectTransform>();
    }

    private void Start()
    {
        TouchCheck = false;

        // 반지름을 구함
        Radius = ((int)RightBackBoard.rect.width >> 1);

        // 이동 속도 설정
        Speed = 5.0f;
    }

    private void Update()
    {
        // 중복 터치 처리
        for (int i = 0; i < Input.touchCount; ++i)
        {
            // 터치 입력
            Touch GetTouch = Input.GetTouch(i);
            
            // 입력 받은 위치값 참조 및 저장
            Vector2 Point = GetTouch.position;

            // 터치 입력 순간 ( = GetButtonDown )
            if(GetTouch.phase == TouchPhase.Began)
            {
                // 좌우측 터치 구별
                if((Screen.width >> 1) > GetTouch.position.x)
                {
                    // 터치 입력의 ID 부여
                    LeftStickID = GetTouch.fingerId;

                    LeftBackBoard.position = Point;
                }
                else
                {
                    // 터치 입력의 ID 부여
                    RightStickID = GetTouch.fingerId;

                    RightBackBoard.position = Point;

                    TouchCheck = true;
                }
            }
            // 터치 입력 중
            else if(GetTouch.phase == TouchPhase.Moved)
            {
                if(GetTouch.fingerId == RightStickID)
                {
                    // 터치가 입력된 위치로 이동
                    RightFiledStick.localPosition = new Vector2(Point.x - RightBackBoard.position.x, Point.y - RightBackBoard.position.y);

                    // 상한선 제한 (Radius를 넘지 못하게 함)
                    RightFiledStick.localPosition = Vector2.ClampMagnitude(RightFiledStick.localPosition, Radius);

                    // 조이스틱의 스틱 방향을 받아옴
                    Vector2 Direction = RightFiledStick.localPosition.normalized;

                    Rotation = Direction.normalized;
                }
                if (GetTouch.fingerId == LeftStickID)
                {
                    // 터치가 입력된 위치로 이동
                    LeftFiledStick.localPosition = new Vector2(Point.x - LeftBackBoard.position.x,Point.y - LeftBackBoard.position.y);

                    // 상한선 제한 (Radius를 넘지 못하게 함)
                    LeftFiledStick.localPosition = Vector2.ClampMagnitude(LeftFiledStick.localPosition, Radius);

                    // 조이스틱의 스틱 방향을 받아옴
                    Vector2 Direction = LeftFiledStick.localPosition.normalized;

                    // Ratio = 비율 & sqrMagnitude = 거리 비교용 float 반환값 ( 조이스틱 상의 드래그 비율 => 속도 변환의 이유 )
                    float Ratio = (LeftBackBoard.position - LeftFiledStick.position).sqrMagnitude / (Radius * Radius);

                    // 평면 상의 좌표값을 공간 벡터로 변형 및 속도값 추가
                    Movement = new Vector3(Direction.x * Speed * Ratio * Time.deltaTime, 0.0f, Direction.y * Speed * Ratio * Time.deltaTime);

                    if(!TouchCheck)
                    {
                        Rotation = Direction.normalized;
                    }
                }       
            }
            else if(GetTouch.phase == TouchPhase.Ended || GetTouch.phase == TouchPhase.Canceled)
            {
                if(GetTouch.fingerId == RightStickID)
                {
                    // 조이스틱 및 조이패드 원위치
                    RightBackBoard.localPosition = Vector3.zero;
                    RightFiledStick.localPosition = Vector3.zero;
                    RightStickID = -1;
                    TouchCheck = false;
                }

                if (GetTouch.fingerId == LeftStickID)
                {
                    // 조이스틱 및 조이패드 원위치
                    LeftBackBoard.localPosition = Vector3.zero;
                    LeftFiledStick.localPosition = Vector3.zero;
                    LeftStickID = -1;
                    Movement = Vector3.zero;
                }
            }
        }

        if(Input.touchCount == 0)
        {
            RightStickID = -1;
            LeftStickID = -1;
            Movement = Vector3.zero;
            TouchCheck = false;
        }

        // 타겟 이동
        Character.position += Movement;

        // 타겟 회전
        Character.localRotation = Quaternion.Euler(new Vector3(0.0f, Mathf.Atan2(Rotation.x, Rotation.y) * Mathf.Rad2Deg, 0.0f));
    }
}