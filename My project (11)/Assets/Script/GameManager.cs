using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_InputField inputField; // InputField 연결
    public Button gameStartButton;    // Button 연결

    private void Start()
    {
        // 버튼 클릭 시 OnGameStartButtonClicked 함수가 실행되도록 연결
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
    }

    private void OnGameStartButtonClicked()
    {
        string playerName = inputField.text;

        // 이름이 비어있는지 확인
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("플레이어 이름을 입력하세요.");
            return;
        }

        // 이름 저장
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        Debug.Log("플레이어 이름 저장 됨: " + playerName);

        // 다음 씬으로 이동
        SceneManager.LoadScene("Level_1");
    }
}