using UnityEngine;
using DG.Tweening;

public abstract class MainMenuComponent : MonoBehaviour, IMainMenuComponent
{
    [SerializeField] protected float animationDuration = 0.4f;
    [SerializeField] protected Ease easeType = Ease.InOutQuad;
    protected MainMenuGUIController controller;

    public virtual void Open()
    {
        Debug.Log("Opening: " + gameObject.name);
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;  // Animasyonu 0'dan başlat
        transform.DOScale(1, animationDuration).SetEase(easeType).OnComplete(OnOpenComplete);
    }

    public virtual void Close()
    {
        Debug.Log("Closing: " + gameObject.name);
        transform.DOScale(0, animationDuration).SetEase(easeType).OnComplete(() =>
        {
            gameObject.SetActive(false);
            OnCloseComplete();
        });
    }
    public void SetupController(MainMenuGUIController mainMenuController) => controller = mainMenuController;
    protected virtual void OnOpenComplete() {}
    protected virtual void OnCloseComplete() {}

}
