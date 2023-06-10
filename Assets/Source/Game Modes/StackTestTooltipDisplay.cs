using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackTestTooltipDisplay : MonoBehaviour
{

    [Header("Animation Data")]
    [SerializeField] private float _fadeTime;
    [SerializeField] private LeanTweenType _tweenInType;
    [SerializeField] private LeanTweenType _tweenOutType;

    [Header("Container and Text")]

    [SerializeField] private RectTransform _container;
    [SerializeField] private TMPro.TextMeshProUGUI _gradeField;
    [SerializeField] private TMPro.TextMeshProUGUI _clusterField;
    [SerializeField] private TMPro.TextMeshProUGUI _descriptionField;



    private BlockDisplay _target;

    void Awake()
    {
        _container.localScale = Vector3.zero;
    }

    void Update()
    {
        if (_target != null)
        {
            var intendedPos = Camera.main.WorldToScreenPoint(_target.transform.position);
            if (intendedPos.x < 0 || intendedPos.x + _container.rect.width > Camera.main.pixelWidth)
            {
                intendedPos.x = Mathf.Clamp(intendedPos.x, 0, Camera.main.pixelWidth - _container.rect.width);
            }

            if (intendedPos.y < _container.rect.height || intendedPos.y > Camera.main.pixelHeight)
            {
                intendedPos.y = Mathf.Clamp(intendedPos.y, _container.rect.height, Camera.main.pixelHeight);
            }
            _container.position = intendedPos;
        }

    }

    public void ShowTooltip(BlockDisplay selectedDisplay)
    {
        if (_target != null) _target.DisplayAsNormal();
        _target = selectedDisplay;
        if (_target == null)
        {
            HideTooltip();
            return;
        }
        _target.DisplayAsSelected();
        var data = selectedDisplay.BlockData;
        _container.localScale = Vector3.zero;
        LeanTween.scale(_container.gameObject, Vector3.one, _fadeTime).setEase(_tweenInType);
        _gradeField.text = $"{data.grade}: {data.domain}";
        _clusterField.text = $"{data.cluster}";
        _descriptionField.text = $"{data.standardid}: {data.standarddescription}";
    }


    public void HideTooltip()
    {
        if (_target != null) _target.DisplayAsNormal();
        LeanTween.scale(_container.gameObject, Vector3.zero, _fadeTime).setEase(_tweenOutType);
    }
}
