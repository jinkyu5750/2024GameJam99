using DG.Tweening;
using EasyTransition;
using UnityEngine;
using UnityEngine.UI;
public class Clear : MonoBehaviour
{

    // 클리어 조건은 StageManager의 currentStage!=null && 모든닭이 감염닭 

    [SerializeField] private TransitionSettings t;
    [SerializeField] private Ease ease;


    [SerializeField] private Text revengeText;
    [SerializeField] private Text clearTimeText;
    [SerializeField] private Text chickenNumText;
    [SerializeField] private Button quitBtn;
    [SerializeField] private bool isQuitBtnActive = false;
    private void Start()
    {
        transform.localScale = Vector3.zero;
        quitBtn.transform.localScale = Vector3.zero;
        GameManager.Instance.isGameStart = false;
    }


    private void OnEnable()
    {

        clearTimeText.text = ""; chickenNumText.text = "";

        string revenge = "복수 완료 .. !!";
        string clearTime = ((int)(GameManager.Instance.clearTime / 60)).ToString("D2") + " : "
            + ((int)(GameManager.Instance.clearTime % 60)).ToString("D2");

        string chickenNum = "00" + "마리는 내 친구가 된 것 같은데?";
        //chickenNumText.text = (GameManager.Instance.)

      
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f).SetEase(ease));
        seq.Append(transform.DOScale(Vector3.one, 0.3f).SetEase(ease));
        seq.Append(revengeText.DOText(revenge, 1f));
        seq.Append(clearTimeText.DOText(clearTime, 1f));
        seq.Append(chickenNumText.DOText(chickenNum, 1f));
        seq.Play().OnComplete(() =>
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(quitBtn.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f));
            seq.Append(quitBtn.transform.DOScale(Vector3.one, 0.2f));
            seq.Play().OnComplete(() =>
            {
                isQuitBtnActive = true;

           /*     if (isQuitBtnActive)
                    StartBouncing();*///포기..
            });
        });
    }

    void StartBouncing()
    {
        quitBtn.transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.5f).OnComplete(() =>
        {
            quitBtn.transform.DOScale(new Vector3(0.95f, 0.95f, 0.95f), 0.5f);

        }).SetLoops(-1);
    }
    public void ClickQuitButton()
    {

        TransitionManager.Instance().runningTransition = false;
        TransitionManager.Instance().Transition(t, 0.2f);

        isQuitBtnActive = false;

        Invoke("DestroyStage", 1.2f);
        StartCoroutine(StageManager.instance.OnUiCanvas(1.2f));

    }


    void DestroyStage()
    {
        Destroy(StageManager.instance.currentStage);
        StageManager.instance.currentStage = null;
    }

}
