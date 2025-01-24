using UnityEngine;
using Tweens;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public abstract class GenericButtonBase : MonoBehaviour
{
    protected bool audioManagerExists = (null != Type.GetType("AudioManager"));
    protected TweenInstance scaleUpTweenInstance;
    protected TweenInstance scaleDownTweenInstance;
    protected TweenInstance clickTweenInstance;
    
    protected Vector3 defaultScale, hoverScale, clickScale;
    protected RectTransform rectTransform;

    [Header("Settings")]
    [SerializeField] protected bool isAnimated = true;
    [SerializeField] protected float hoverScaleMultiplier = 1.1f;
    [SerializeField] protected float clickScaleMultiplier = 0.9f;
    [SerializeField] protected EaseType hoverEaseType = EaseType.SineInOut;
    [SerializeField] protected EaseType clickEaseType = EaseType.QuintInOut;
    [SerializeField] protected float hoverDuration = 0.1f;
    [SerializeField] protected float clickDuration = 0.1f;
    [SerializeField] protected bool affectedByTimeScale = false;
    [SerializeField] protected bool isOpenAudioPanelButton = false;
    [SerializeField] protected UnityEvent onClick;
    [Header("Extra")]
    [SerializeField] protected bool hasStartAnimation = false;
    [SerializeField] protected AnimType startAnimation = AnimType.PopUp;
    [SerializeField] protected float startAnimationDuration = 1f;
    [Header("Audio")]
    [SerializeField] protected string clickSFXName;

    protected abstract void InitializeButton();

    protected virtual void Start()
    {
        defaultScale = transform.localScale;
        hoverScale = defaultScale * hoverScaleMultiplier;
        clickScale = defaultScale * clickScaleMultiplier;
        InitializeButton();

        if (hasStartAnimation)
            OnButtonActive();
    }

    protected virtual void OnEnable()
    {
        if (hasStartAnimation)
            OnButtonActive();
    }

#region Mouse Handler
/// <summary>
/// Overrideable methods for handling mouse events, can call base method to keep default behavior as well
/// </summary>
    protected virtual void HandleMouseEnter()
    {
        if (isAnimated)
            StartScaleTween(hoverScale, ref scaleUpTweenInstance, ref scaleDownTweenInstance, hoverDuration);
        else
            transform.localScale = hoverScale;
    }
    protected virtual void HandleMouseExit()
    {
        if (isAnimated)
            StartScaleTween(defaultScale, ref scaleDownTweenInstance, ref scaleUpTweenInstance, hoverDuration);
        else
            transform.localScale = defaultScale;
    }
    protected virtual void HandleMouseClick()
    {
        if (clickTweenInstance != null)
            return;

        if (audioManagerExists && !string.IsNullOrEmpty(clickSFXName))
            AudioManager.PlaySFX(clickSFXName);
        
        // Cancel both existing tweens before allowing the click tween to start
        CancelTweens();
        var tween = new LocalScaleTween
        {
            to = clickScale,
            duration = clickDuration,
            usePingPong = true,
            easeType = clickEaseType,
            useUnscaledTime = !affectedByTimeScale,
            onEnd = (instance) => { 
                clickTweenInstance = null;
                onClick?.Invoke();// Call the UnityEvent after animation

                if (isOpenAudioPanelButton)
                {
                    AudioManager.ToggleAudioControlPanel(true);
                }
            }
        };
        clickTweenInstance = gameObject.AddTween(tween);
    }
#endregion
#region Tween Functions

    protected void StartScaleTween(Vector3 targetScale, ref TweenInstance tweenToStart, ref TweenInstance tweenToCancel, float duration)
    {
        tweenToCancel?.Cancel();

        var tween = new LocalScaleTween
        {
            to = targetScale,
            duration = duration,
            easeType = hoverEaseType,
            useUnscaledTime = !affectedByTimeScale
        };
        tweenToStart = gameObject.AddTween(tween);
    }

    protected void CancelTweens()
    {
        scaleUpTweenInstance?.Cancel();
        scaleDownTweenInstance?.Cancel();
    }

    protected void OnButtonActive()
    {
        switch (startAnimation)
        {
            case AnimType.PopUp:
                PopUp();
                break;
            case AnimType.ScalePop:
                ScalePop();
                break;
            default:
                break;
        }
    }
#endregion
#region Tween Animations

    protected virtual void PopUp()
    {
        var popUpTween = new LocalPositionYTween
        {
            from = transform.localPosition.y - 1,
            to = transform.localPosition.y,
            duration = startAnimationDuration,
            useUnscaledTime = !affectedByTimeScale,
            easeType = EaseType.ElasticOut
        };
        gameObject.AddTween(popUpTween);
    }
    protected virtual void ScalePop()
    {
        var scalePopTween = new LocalScaleTween
        {
            from = Vector3.zero,
            to = defaultScale,
            duration = startAnimationDuration,
            easeType = EaseType.ElasticOut,
            useUnscaledTime = !affectedByTimeScale
        };
        gameObject.AddTween(scalePopTween);
    }
#endregion
}