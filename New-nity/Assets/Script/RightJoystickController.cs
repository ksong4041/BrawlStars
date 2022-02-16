using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightJoystickController : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("움직일 대상")]
    public Transform Character;

    [Header("JoyStick")]
    [Tooltip("움직일 대상의 움직임을 제어하는 오른쪽 스틱")]
    [SerializeField] private RectTransform RightFiledStick;

    [Tooltip("오른쪽 조이스틱의 뒷 배경")]
    [SerializeField] private GameObject RightJoyPad;
    [SerializeField] private RectTransform RightBackBoard;

    // 조이스틱 반지름
    private float Radius = 0.0f;

    // 입력이 된 상태 확인
    private bool TouchCheck = false;

    // 타겟이 이동할 방향
    private Vector2 Direction;

    // 타겟이 바라볼 방향
    private Vector3 Rotation;

    private void Awake()
    {
        // 조이스틱의 패드를 받아옴
        RightFiledStick = GameObject.Find("RightFiledCircle").GetComponent<RectTransform>();

        // 조이스틱의 부모객체 (빈 게임 오브젝트)
        RightJoyPad = GameObject.Find("RightOutLineCircle");
        RightBackBoard = RightJoyPad.GetComponent<RectTransform>();
    }

    void Start()
    {
        // 반지름을 구함
        Radius = ((int)RightBackBoard.rect.width >> 1);

        // 현재 터치 상태를 초기값으로 설정
        TouchCheck = false;
    }

    void Update()
    {
        // 입력 시
        if (Input.GetMouseButtonDown(0))
        {
            // 스크린(화면)의 중앙으로부터 좌우측을 구분하고 현재 조이스틱이 우측을 담당하므로 우측면 입력에 해당 시 실행
            if ((Screen.width >> 1) < Input.mousePosition.x)
            {
                // 입력 활성화
                TouchCheck = true;
                // 터치된 위치로 조이스틱의 위치를 변경
                RightBackBoard.position = Input.mousePosition;
            }
        }

        // 입력 중
        if (Input.GetMouseButton(0))
        {
            // 터치 상태 활성화 여부 확인
            if (TouchCheck)
            {
                // 터치가 입력된 위치로 이동
                RightFiledStick.localPosition = new Vector2(Input.mousePosition.x - RightBackBoard.position.x, Input.mousePosition.y - RightBackBoard.position.y);

                // 상한선 제한 (Radius를 넘지 못하게 함)
                RightFiledStick.localPosition = Vector2.ClampMagnitude(RightFiledStick.localPosition, Radius);

                // 조이스틱의 스틱 방향을 받아옴
                Direction = RightFiledStick.localPosition.normalized;

                Rotation = Direction.normalized;
            }
        }

        // 입력 종료
        if (Input.GetMouseButtonUp(0))
        {
            // 터치 비활성화
            TouchCheck = false;
            // 조이스틱 및 조이패드 원위치
            RightBackBoard.localPosition = Vector3.zero;
            RightFiledStick.localPosition = Vector3.zero;

            // 투사체 발사 예정
        }

        // 터치 중
        if (TouchCheck)
        {
            // 타겟 회전
            Character.localRotation = Quaternion.Euler(new Vector3(0.0f, Mathf.Atan2(Rotation.x, Rotation.y) * Mathf.Rad2Deg, 0.0f));
        }
    }
}