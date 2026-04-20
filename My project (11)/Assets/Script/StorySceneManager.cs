using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

[System.Serializable]
public class StoryData
{
    [TextArea(3, 10)]
    public string sentence;
    public Sprite illustration;
}

public class StorySceneManager : MonoBehaviour
{
    [Header("UI 요소")]
    public TextMeshProUGUI dialogueText;
    public Image displayImage;

    [Header("설정")]
    public float typingSpeed = 0.05f;
    public List<StoryData> storyFlow;

    [Header("씬 전환 설정")]
    public string nextSceneName; 

    private int _index;
    private bool _isTyping;
    private bool _isStoryEnded; 

    void Start()
    {
        if (storyFlow.Count > 0)
        {
            UpdateScene();
        }
    }

    public void DisplayNextSentence()
    {
       
        if (_isStoryEnded)
        {
            LoadNextScene();
            return;
        }

       
        if (_isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = storyFlow[_index].sentence;
            _isTyping = false;
            return;
        }

        
        _index++;

        if (_index < storyFlow.Count)
        {
            UpdateScene();
        }
        else
        {
           
            dialogueText.text = "모험을 출발한다";
            _isStoryEnded = true;
        }
    }

    void UpdateScene()
    {
        StartCoroutine(TypeSentence(storyFlow[_index].sentence));

        if (storyFlow[_index].illustration != null)
        {
            displayImage.sprite = storyFlow[_index].illustration;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        _isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        _isTyping = false;
    }

    
    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("다음 씬 이름이 설정되지 않았습니다!");
        }
    }
}