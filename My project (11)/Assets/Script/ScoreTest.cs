using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ScoreTest : MonoBehaviour
{
    [Header("Stage Score Texts")]
    public TextMeshProUGUI stage1;
    public TextMeshProUGUI stage2;
    public TextMeshProUGUI stage3;
    public TextMeshProUGUI stage4;
    public TextMeshProUGUI stage5;

    void Start()
    {
        UpdateLeaderBoard();
    }

    public void UpdateLeaderBoard()
    {

        if (stage1 != null)
            stage1.text = "STAGE 1 : " + LeaderBoard.Load(1).ToString();

        if (stage2 != null)
            stage2.text = "STAGE 2 : " + LeaderBoard.Load(2).ToString();

        if (stage3 != null)
            stage3.text = "STAGE 3 : " + LeaderBoard.Load(3).ToString();

        if (stage4 != null)
            stage4.text = "STAGE 4 : " + LeaderBoard.Load(4).ToString();

        if (stage5 != null)
            stage5.text = "STAGE 5 : " + LeaderBoard.Load(5).ToString();
    }
}
