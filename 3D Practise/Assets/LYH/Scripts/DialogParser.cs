using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

[System.Serializable]
public class DialogLine
{
    public int index;
    public string charName;
    public string imgName;
    public string dialog;
    public string animation;
    public string bgmusic;
    public string bgImg;
    public string effect;
    public string soundEffect;
    public string transition;
}

public class DialogParser : MonoBehaviour
{
    [SerializeField] private string dialogName;

    private TextAsset[] dialogDatas;
    private TextAsset dialogData;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogText;

    [SerializeField] private GameObject dialogUI;
    [SerializeField] private GameObject playerDialogObject;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject changeNameUI;

    [SerializeField] private Image blackTransition;
    [SerializeField] private Image effectPanel;

    [SerializeField] private TextMeshProUGUI playerDialogText;

    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private AudioSource bgSoundPlayer;

    [SerializeField] private Image charImg;
    [SerializeField] private Image monsterImg;
    [SerializeField] private Image bgImg;

    [SerializeField] private Transform logContent; // Content 오브젝트
    [SerializeField] private GameObject logLinePrefab; // 로그 프리팹 (안에 TextMeshProUGUI 2개 있음)
    private List<GameObject> logObjects = new List<GameObject>();

    [SerializeField] private float transitionTime = 1.0f;

    private Sprite[] charSprites;
    private Sprite[] bgSprites;
    private Sprite[] effects;

    private AudioClip[] sounds;
    private AudioClip[] bgSounds;

    private string[] charNames = {
        "한서리",
        "백웅",
        "고서은",
        "소인랑",
        "백천아",
        "엄모안",
        "산드레",
        "달리아",
        "장희빈",
        "육호연",
        "상점 주인"
    };

    private List<DialogLine> dialogLines = new List<DialogLine>();
    private int currentIndex = 0;
    private int charIndex = 0;
    private bool doTransition = false;

    public string playerName = "셰프";

    void Start()
    {
        // Resources 폴더에서 자동 로드
        dialogDatas = Resources.LoadAll<TextAsset>("Story/Dialogs");
        charSprites = Resources.LoadAll<Sprite>("Story/Sprites/Characters");
        bgSprites = Resources.LoadAll<Sprite>("Story/Sprites/Backgrounds");
        effects = Resources.LoadAll<Sprite>("Story/Sprites/Effects");

        sounds = Resources.LoadAll<AudioClip>("Story/Sounds/Effects");
        bgSounds = Resources.LoadAll<AudioClip>("Story/Sounds/BGM");

        // dialogName 과 같은 이름의 데이터 선택
        foreach (var data in dialogDatas)
        {
            if (data.name == dialogName)
            {
                dialogData = data;
                break;
            }
        }
        LoadDialog();

        for (int i = 0; i < dialogLines.Count; i++)
        {   // 로그 프리팹 생성
            GameObject logLineObj = Instantiate(logLinePrefab, logContent);
            logLineObj.SetActive(true);
            logObjects.Add(logLineObj);
        }

        ShowLine(0);
    }

    void LoadDialog()
    {
        string[] lines = dialogData.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            while (values.Length < 10)
            {
                Array.Resize(ref values, 10);
            }

            DialogLine line = new DialogLine
            {
                index = int.Parse(values[0]),
                charName = values[1],
                imgName = values[2],
                dialog = values[3].Replace("`", ","),
                animation = values[4],
                bgmusic = values[5],
                bgImg = values[6],
                effect = values[7],
                soundEffect = values[8],
                transition = values[9].Trim(),
            };

            dialogLines.Add(line);
        }
        Debug.Log($"총 {dialogLines.Count}개의 대사가 로드되었습니다.");
    }

    void ShowLine(int index) // 다음 대사를 표시하는 버튼을 눌렀을때 행동하는 함수
    {
        if (index < 0 || index >= dialogLines.Count)
        {
            Debug.LogWarning("대사 인덱스 범위를 벗어남");
            return;
        }
        // ----------------------------------------------------------------------------------------------------------------
        DialogLine line = dialogLines[index];
        // ----------------------------------------------------------------------------------------------------------------

        // 몬스터 대사 = 4
        charIndex = 4;
        // 플레이어 대사 = 0
        if (line.charName == "playerName") charIndex = 0;
        // 캐릭터 대사 = 1
        for (int i = 0; i < charNames.Length; i++)
        {
            if (charNames[i] == line.charName)
            {
                charIndex = 1;
            }
        }
        // 내레이션 대사 = 2
        if (line.charName == "내레이션") charIndex = 2;
        // 대사가 없는 경우 = 3
        if (line.charName == "") charIndex = 3;
        // 이름 입력 = 5
        if (line.charName == "이름입력") charIndex = 5;

        Debug.Log($"{line.charName}, {charIndex}");
        // ----------------------------------------------------------------------------------------------------------------
        // 배경음악 재생 기능
        if (line.bgmusic != "")
        {
            if (line.bgmusic == "stop")
            {
                Debug.Log("브금 정지");
                bgSoundPlayer.Stop();
            }
            else
            {
                for (int i = 0; i < bgSounds.Length; i++)
                {
                    // 중복 재생 안 되도록
                    if (bgSounds[i].name == line.bgmusic)
                    {
                        Debug.Log("브금 재생");
                        bgSoundPlayer.clip = bgSounds[i];
                        if (line.bgmusic != bgSoundPlayer.clip.name || !bgSoundPlayer.isPlaying) bgSoundPlayer.Play();
                    }
                }
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // 배경지정
        for (int i = 0; i < bgSprites.Length; i++)
        {
            if (bgSprites[i].name == line.bgImg)
            {
                bgImg.sprite = bgSprites[i];
                Debug.Log(line.bgImg);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // 대사 관리
        UIOff();
        switch (charIndex)
        {
            case 0: // 플레이어 대사
                Debug.Log("플레이어 대사 출력");
                menu.SetActive(false);
                playerDialogObject.gameObject.SetActive(true);
                break;

            case 1: // 캐릭터 대사
                Debug.Log("캐릭터 대사 출력");
                UIOn(true);
                for (int i = 0; i < charSprites.Length; i++)
                {
                    if (line.imgName == charSprites[i].name) charImg.sprite = charSprites[i];
                }
                nameText.text = line.charName;
                dialogText.text = line.dialog;
                break;

            case 2: // 내레이션
                Debug.Log("내레이션 출력");
                UIOn(true);
                nameText.text = "";
                dialogText.text = line.dialog;
                break;

            case 3: // 대사가 없는 경우
                Debug.Log("대사 없이 효과만 있는 경우");
                // 2초후 다음 대사로 진행
                DOVirtual.DelayedCall(2f, () => NextLine());
                break;

            case 4: // 몬스터 대사
                Debug.Log("몬스터 대사 출력");
                UIOn(false);
                nameText.text = line.charName;
                dialogText.text = line.dialog;
                break;

            case 5: // 이름 입력
                Debug.Log("이름 입력 UI 호출");
                changeNameUI.gameObject.SetActive(true);

                // TODO 1. InputField 에서 유저의 닉네임을 입력받아서 playerName 변수에 반영
                // TODO 2. 비속어 및 올바르지 않은 닉네임 입력시 거절 ("에이, 그런 이름을 가진 사람이 어디있어요?")
                // TODO 3. playerName 변수 -> 로컬 userName & 서버 userName에 반영

                break;

            default:
                Debug.LogWarning("처리되지 않은 charIndex 값");
                break;
        }

        // 트랜지션 효과 실행
        if (line.transition == "fade")
        {
            doTransition = true;
        }

        // 특수효과 사운드 실행
        if (line.soundEffect != "")
        {
            Debug.Log(line.soundEffect + " 사운드 실행");
            for (int i = 0; i < sounds.Length; i++)
            {
                if (line.soundEffect == sounds[i].name)
                {
                    soundPlayer.clip = sounds[i];
                    soundPlayer.Play();
                }
            }
        }

        // 특수효과 이미지 실행
        if (line.effect != "")
        {
            Debug.Log(line.effect + " 이펙트 실행");
            for (int i = 0; i < effects.Length; i++)
            {
                if (line.effect == effects[i].name)
                {
                    effectPanel.gameObject.SetActive(true);
                    effectPanel.sprite = effects[i];
                }
            }
        }
    }

    private void UIOff()
    {
        charImg.gameObject.SetActive(false);
        monsterImg.gameObject.SetActive(false);
        dialogUI.gameObject.SetActive(false);
    }

    private void UIOn(bool isChar)
    {
        if (!doTransition) menu.SetActive(true);
        playerDialogObject.SetActive(false);
        dialogUI.gameObject.SetActive(true);

        if (isChar) charImg.gameObject.SetActive(true);
        else monsterImg.gameObject.SetActive(true);
    }

    public void showLog() // 자동으로 로그 텍스트 입력 및 텍스트 박스 사이즈 조정
    {
        for (int i = 0; i < currentIndex; i++)
        {
            TextMeshProUGUI[] texts = logObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = dialogLines[i].charName == "playerName" ? playerName : dialogLines[i].charName; // 이름
            texts[1].text = dialogLines[i].dialog; // 대사
        }
        for (int i = currentIndex; i < dialogLines.Count; i++) // 이전 대사로 돌아갔을때 대사를 지울 필요가 있음
        {
            TextMeshProUGUI[] texts = logObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = ""; // 이름 비우기
            texts[1].text = ""; // 대사 비우기
        }
    }

    // 다음 대사로 넘어가는 예시 (나중에 버튼에 연결)
    public void NextLine()
    {
        if (currentIndex < dialogLines.Count - 1)
        {
            if (doTransition)
            {
                blackTransition.gameObject.SetActive(true);
                currentIndex++;
                DOVirtual.DelayedCall(transitionTime, () => ShowLine(currentIndex));
                doTransition = false;
            }
            else
            {
                currentIndex++;
                ShowLine(currentIndex);
            }
        }
        else
        {
            Debug.Log("모든 대사 종료");
            Debug.Log("다음 씬으로 전환");
        }
    }
    public void PreviousLine()
    {
        doTransition = false;
        if (currentIndex > 0)
        {
            currentIndex--;
            if (dialogLines[currentIndex].charName == "playerName") currentIndex--;
            if (dialogLines[currentIndex].charName == "이름입력") currentIndex--;
            if (dialogLines[currentIndex].charName == "") currentIndex--;

            if (currentIndex < 0) PreviousLine();
            ShowLine(currentIndex);
        }
        else
        {
            Debug.Log("첫 대사 출력 완료");
        }
    }

    public void changePlayerName(string name)
    {
        playerName = name;
        Debug.Log(name);
        changeNameUI.gameObject.SetActive(false);
        NextLine();
    }
}