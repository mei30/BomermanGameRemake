using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class StageIntro : MonoBehaviour
{
    public TextMeshProUGUI stageText;

    void Start()
    {
        stageText.text = "STAGE " + GameManager.Instance.currentStage;
        stageText.ForceMeshUpdate();
        StartCoroutine(GoToGameScene());
    }

    IEnumerator GoToGameScene()
    {
        yield return new WaitForSeconds(4f);
        GameManager.Instance.LoadStage();
    }
}