using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // 점수 기능
    [SerializeField]
    Text score = null;

    [SerializeField]
    int increaseScore = 10;

    int currentScore = 0;

    [SerializeField]
    float[] weight = null;

    Animator anim;
    string scoreUp = "ScoreUp";

    // 콤보 기능
    [SerializeField]
    GameObject ComboImage = null;
    [SerializeField]
    Text ComboTxt = null;

    int currentCumbo = 0;

    // 콤보 추가 점수
    [SerializeField]
    Animator comboAnimator; // 콤보 애니메이션용 애니메이터
    [SerializeField]
    int comboBonusScore = 10;
    string comboUp = "ComboUp";

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currentScore = 0;
        score.text = "0";

        ComboTxt.gameObject.SetActive(false);
        ComboImage.SetActive(false);
    }

    public void IncreaseScore(int judgeState)
    {
        // 콤보 증가
        IncreaseCombo();

        // 콤보 보너스 점수
        int t_currentCombo = currentCumbo;
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;

        // 가중치 계산
        int t_increaseScore = increaseScore + t_bonusComboScore;
        t_increaseScore = (int)(t_increaseScore * weight[judgeState]);

        // 점수 반영
        currentScore += t_increaseScore;
        score.text = string.Format("{0:#,##0}", currentScore);

        anim.SetTrigger(scoreUp);
    }

    public void IncreaseCombo(int num = 1)
    {
        currentCumbo += num;
        ComboTxt.text = string.Format("{0:#,##0}", currentCumbo);

        if (currentCumbo > 2)
        {
            ComboTxt.gameObject.SetActive(true);
            ComboImage.SetActive(true);
            comboAnimator.SetTrigger(comboUp);

            // 콤보 텍스트 길이에 따라 X 포지션 조정
            AdjustComboTextPosition();
        }
    }

    public void ResetCombo()
    {
        currentCumbo = 0;
        ComboTxt.text = "0";
        ComboTxt.gameObject.SetActive(false);
        ComboImage.SetActive(false);
    }

    private void AdjustComboTextPosition()
    {
        // ComboTxt의 RectTransform 가져오기
        RectTransform comboRect = ComboTxt.GetComponent<RectTransform>();

        // 콤보 텍스트 길이에 따라 X 포지션 설정
        int textLength = ComboTxt.text.Length;

        if (textLength == 1)
        {
            comboRect.anchoredPosition = new Vector2(-70, comboRect.anchoredPosition.y);
        }
        else if (textLength == 2)
        {
            comboRect.anchoredPosition = new Vector2(-90, comboRect.anchoredPosition.y);
        }
        else if (textLength >= 3)
        {
            comboRect.anchoredPosition = new Vector2(-100, comboRect.anchoredPosition.y);
        }
    }
}
