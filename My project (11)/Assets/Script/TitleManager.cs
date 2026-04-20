using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject Help;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        SceneManager.LoadScene("StartStory");
    }
    public void OpenHelpPanel()
    {
        Help.SetActive(true);
    }

    public void CloseHelpPanel()
    {
        Help.SetActive(false);
    }
    public void QuitGame()
    {
        // 에디터에서 실행 중일 때 종료 (테스트용)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 실제 빌드된 게임에서 종료
        Application.Quit();
#endif

        Debug.Log("게임이 종료되었습니다.");
    }
}
