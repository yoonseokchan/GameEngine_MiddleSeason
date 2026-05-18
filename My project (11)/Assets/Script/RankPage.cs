using System.Linq;
using UnityEngine;
using TMPro;

public class RankPage : MonoBehaviour
{
    [SerializeField] Transform contentRoot;   
    [SerializeField] GameObject rowPrefab;    

    StageResultList allData;

    void Awake()
    {
        allData = StageResultSaver.LoadRank();
        RefreshRankList(1);
    }

    public void OnClickStageButton(int stageNumber)
    {
        RefreshRankList(stageNumber);
    }

    public void RefreshRankList(int stageNumber)
    {
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        var sortedData = allData.results
            .Where(r => r.stage == stageNumber) 
            .OrderByDescending(x => x.score)
            .ToList();

        for (int i = 0; i < sortedData.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();


            rankText.text = $"{i + 1}. {sortedData[i].playerName} - {sortedData[i].score}";
        }
    }
}