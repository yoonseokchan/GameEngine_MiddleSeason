using System.Linq;
using UnityEngine;
using TMPro;

public class RankPage : MonoBehaviour
{
    [SerializeField] Transform contentRoot;    // Content 오브젝트 (UI List가 담길 곳)
    [SerializeField] GameObject rowPrefab;     // RankRow 프리팹 (한 줄 디자인)

    StageResultList allData;

    void Awake()
    {
        // 저장된 데이터를 불러오고 리스트를 새로고침함
        allData = StageResultSaver.LoadRank();
        RefreshRankList();
    }

    void RefreshRankList()
    {
        // 1. 기존의 모든 자식 오브젝트 삭제 (리스트 초기화)
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        // 2. 랭크 데이터 정렬 (1스테이지 데이터만 필터링 후 점수 높은 순 정렬)
        var sortedData = allData.results
            .Where(r => r.stage == 1)
            .OrderByDescending(x => x.score)
            .ToList();

        // 3. 랭크 데이터 생성 및 텍스트 설정
        for (int i = 0; i < sortedData.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();

            // "순위. 이름 - 점수" 형식으로 표시
            rankText.text = $"{i + 1}. {sortedData[i].playerName} - {sortedData[i].score}";
        }
    }
}