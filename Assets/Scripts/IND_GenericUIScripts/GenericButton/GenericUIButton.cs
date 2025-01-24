using Tweens;
using UnityEngine;
using UnityEngine.EventSystems;
public class GenericUIButton : GenericButtonBase, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private float _originalYPosition;
    
    [Header("Settings for Popup Animation")]
    [SerializeField] private float _toYposition = 0f;
    [SerializeField] private float _offsetFromYposition = 10f;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    #region Override Start Animation

    // Override default start animation to fit for UI RectTransform properties
    protected override void PopUp()
    {
        var popUpTween = new AnchoredPositionYTween
        {
            from = _toYposition + _offsetFromYposition,
            to = _toYposition,
            duration = startAnimationDuration,
            easeType = EaseType.ElasticOut,
            useUnscaledTime = !affectedByTimeScale
        };
        gameObject.AddTween(popUpTween);
    }

#endregion
#region Click Interaction

    protected override void InitializeButton()
    {
        // Custom initialization for UIButton
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HandleMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HandleMouseExit();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HandleMouseClick();
    }
    
#endregion
}
