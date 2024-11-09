using DG.Tweening;
using EasyTransition;
using UnityEngine;
using UnityEngine.EventSystems;

public class ESC : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //임시
    public GameObject p;
    public AudioSource audioSource;
    [SerializeField] private GameObject escPanel;

    public Ease ease;
    public TransitionSettings t;


    private void Start()
    {

        audioSource = GetComponent<AudioSource>();

        escPanel.transform.localScale = new Vector3(1, 0, 1);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.isGameStart == true && escPanel.activeSelf == false)
        {
            audioSource.clip = SoundManager.instance.esc;
            audioSource.Play();

            escPanel.SetActive(true);
            escPanel.transform.DOScaleY(1, 0.15f).SetEase(ease);
        }
    }

    public void Resume()
    {
        audioSource.clip = SoundManager.instance.buttonClick;
        audioSource.Play();

        escPanel.transform.DOScaleY(0, 0.15f).SetEase(ease).OnComplete(() =>
        {
            escPanel.SetActive(false);
        });

    }

    public void Option()
    {
        audioSource.clip = SoundManager.instance.buttonClick;
        audioSource.Play();

        p.SetActive(true);
        Debug.Log("없는기능 . . . . . .");
    }

    public void Exit()
    {
        audioSource.clip = SoundManager.instance.buttonClick;
        audioSource.Play();

        escPanel.SetActive(false);

        TransitionManager.Instance().runningTransition = false;
        TransitionManager.Instance().Transition(t, 0.1f);

        Invoke("DestroyStage", 1f);
        StartCoroutine(StageManager.instance.OnUiCanvas(1f));

        GameManager.Instance.isGameStart = false;
    }

    void DestroyStage()
    {
        Destroy(StageManager.instance.currentStage);
        StageManager.instance.currentStage = null;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector2(1.1f, 1.1f), 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(new Vector2(1f, 1f), 0.2f);
    }
}
