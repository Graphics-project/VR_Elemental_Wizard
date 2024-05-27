using TMPro;
using UnityEngine;

public class EndCreditText : MonoBehaviour
{
    public TextMeshProUGUI endCreditText;
    public int kills;
    public float elapsedTime;

    void Start()
    {
        kills = GameManager.instance.kills;
        elapsedTime = GameManager.instance.elapsedTime;

        endCreditText.text = "The Elemental Wizard\n\n" +
        "처치한 적 수 : " + kills + "\n\n" +
        "총 플레이 시간 : " + (int)elapsedTime / 60 + "분 " + (int)elapsedTime % 60 + "초\n\n" +
        "플레이 해주셔서 감사합니다!\n\n\n\n" +
        "---------------------------------\n\n\n\n" +
        "제작자 : 고병하, 이영민, 이승원\n\n" +
        "제작기간 : 2024년 5월 1일 ~ 5월 28일\n\n" +
        "적 디자인 : 고병하\n\n" +
        "VR 세팅 : 이영민\n\n" +
        "맵 디자인 : 이승원\n\n" +
        "VFX : 고병하, 이영민\n\n" +
        "UI/UX : 이승원\n\n" +
        "게임 시스템 디자인 : 고병하, 이영민, 이승원\n\n" +
        "시나리오 : 고병하, 이영민, 이승원\n\n" +
        "게임 테스트 : 고병하, 이영민, 이승원\n\n\n\n\n" +
        "Very Special Thanks to Prof. 박 준\n";
    }
}
