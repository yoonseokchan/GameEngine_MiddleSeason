using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 추가

public class StageManager : MonoBehaviour
{

    public int currentStage = 2;
    public int totalScore = 0; 


    public void AddScore(int itemPoint)
    {
        totalScore += itemPoint;
        Debug.Log($"점수 획득! 현재 점수: {totalScore}");
    }

    public void StageClear()
    {
        Debug.Log("스테이지 클리어! 데이터를 저장합니다.");

        StageResultSaver.SaveStage(currentStage, totalScore);

        SceneManager.LoadScene("Title"); 
    }
}