using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadController : MonoBehaviour
{
    static string NextScene;

    [SerializeField] private Image Progressbar;
    private float CrossLine;

    void Start()
    {
        StartCoroutine(LoadSceneData());
        CrossLine = 0.85f;
    }

    public static void SetScene(string _Scenename)
    {
        NextScene = _Scenename;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadSceneData()
    {
        // 장면 전환 시켜주는 함수
        AsyncOperation AsyncLoad = SceneManager.LoadSceneAsync(NextScene);

        // Scene을 바로 넘길지 설정하는 함수
        AsyncLoad.allowSceneActivation = false;

        float FakeTime = 0.0f;

        // 로딩이 됬는지 체크하는 반복문
        while (!AsyncLoad.isDone)
        {
            yield return null;

            // 진행 상태가 CrossLine(85%) 미만
            if (AsyncLoad.progress < CrossLine)
            {
                // 진행 상태 표시
                Progressbar.fillAmount = AsyncLoad.progress;
            }
            // 진행 상태가 CrossLine(85%) 이상
            else
            {
                // 가상 시간 제공 및 그 시간 비율만큼 로딩창 게이지 적용
                FakeTime += Time.deltaTime;
                Progressbar.fillAmount = Mathf.Lerp(CrossLine, 1.0f, FakeTime);

                // 로딩 완료 시 반복문 종료
                if(Progressbar.fillAmount >= 1.0f)
                {
                    AsyncLoad.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}