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

    // 조이스틱 반지름
    private float Radius = 0.0f;

    // 입력이 된 상태 확인
    private bool TouchCheck = false;

    // 타겟의 이동 속도
    private float Speed = 0.0f;

    // 타겟이 이동할 방향
    private Vector2 Direction;

    // 타겟이 이동할 방향 및 속도 등을 반영할 임시 포지션 (조이스틱의 2차원 값을 3차원으로 변형시키기 위함)
    private Vector3 Movement;

    // 타겟이 바라볼 방향
    private Vector3 Rotation;

    private void Awake()
    {
        // 조이스틱의 패드를 받아옴
        LeftFiledStick = GameObject.Find("LeftFiledCircle").GetComponent<RectTransform>();

        // 조이스틱의 부모객체 (빈 게임 오브젝트)
        LeftJoyPad = GameObject.Find("LeftOutLineCircle");
        LeftBackBoard = LeftJoyPad.GetComponent<RectTransform>();
    }

    void Start()
    {
        // 반지름을 구함
        Radius = ((int)LeftBackBoard.rect.width >> 1); // 연산하지 않고 비트를 좌 또는 우 로 밀어서 곱셈 또는 나눗셈 ( 2n ) 하는 방법
                                                       // (위의 것과 동일) Radius = (LeftBackBoard.rect.width * 0.5f);

        // 현재 터치 상태를 초기값으로 설정
        TouchCheck = false;
        // 이동 속도 설정
        Speed = 5.0f;
    }

    void Update()
    {
        // 입력 시
        if(Input.GetMouseButtonDown(0))
        {
            // 스크린(화면)의 중앙으로부터 좌우측을 구분하고 현재 조이스틱이 좌측을 담당하므로 좌측면 입력에 해당 시 실행
            if((Screen.width >> 1) > Input.mousePosition.x)
            {
                // 입력 활성화
                TouchCheck = true;
                // 터치된 위치로 조이스틱의 위치를 변경
                LeftBackBoard.position = Input.mousePosition;
            }
        }

        // 입력 중
        if(Input.GetMouseButton(0))
        {
            // 터치 상태 활성화 여부 확인
            if(TouchCheck)
            {
                // 터치가 입력된 위치로 이동
                LeftFiledStick.localPosition = new Vector2(Input.mousePosition.x - LeftBackBoard.position.x, Input.mousePosition.y - LeftBackBoard.position.y);

                // 상한선 제한 (Radius를 넘지 못하게 함)
                LeftFiledStick.localPosition = Vector2.ClampMagnitude(LeftFiledStick.localPosition, Radius);

                // 조이스틱의 스틱 방향을 받아옴
                Direction = LeftFiledStick.localPosition.normalized;

                // Ratio = 비율 & sqrMagnitude = 거리 비교용 float 반환값
                float Ratio = (LeftBackBoard.position - LeftFiledStick.position).sqrMagnitude / (Radius * Radius);

                Rotation = new Vector3(0.0f, Direction.y * 90, 0.0f);

                // 평면 상의 좌표값을 공간 벡터로 변형 및 속도값 추가
                Movement = new Vector3(Direction.x * Speed * Ratio * Time.deltaTime, 0.0f, Direction.y * Speed * Ratio * Time.deltaTime);
            }
        }

        // 입력 종료
        if(Input.GetMouseButtonUp(0))
        {
            // 터치 비활성화
            TouchCheck = false;
            // 조이스틱 및 조이패드 원위치
            LeftBackBoard.localPosition = Vector3.zero;
            LeftFiledStick.localPosition = Vector3.zero;
        }

        // 터치 중
        if( TouchCheck)
        {
            // 타겟 이동
            Character.position += Movement;
            Character.rotation = Quaternion.Euler(Rotation);
        }
    }
}