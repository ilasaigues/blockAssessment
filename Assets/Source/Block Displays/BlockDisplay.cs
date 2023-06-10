using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class BlockDisplay : MonoBehaviour
{


    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Vector3 _startScale;

    public bool Simulate
    {
        get => _simulate;
        set
        {
            _simulate = value;
            _rb.isKinematic = !value;
        }
    }
    private bool _simulate;

    private Rigidbody _rb;

    private BlockType _blockTypeInfo;
    private StackTestGameMode _stackTestGameMode;
    private BlockData _blockData;
    public BlockData BlockData => _blockData;

    [Inject] private StackTestTooltipDisplay _tooltip;

    [Inject]
    private void Initialize(BlockType blockTypeInfo, StackTestGameMode stackTestGameMode, BlockData blockData)
    {
        _blockTypeInfo = blockTypeInfo;
        _blockData = blockData;
        _stackTestGameMode = stackTestGameMode;
        GetComponent<Renderer>().material = blockTypeInfo.BaseMaterial;
    }

    public void SetTransformData(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        _startPosition = position;
        _startRotation = rotation;
        _startScale = scale;
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _stackTestGameMode.OnSimulationStart += OnSimulationStart;
        _stackTestGameMode.OnSimulationStop += OnSimulationStop;
    }

    void OnDestroy()
    {
        _stackTestGameMode.OnSimulationStart -= OnSimulationStart;
        _stackTestGameMode.OnSimulationStop -= OnSimulationStop;
    }


    void Update()
    {

    }

    public void OnSimulationStart(StackTestGameMode.StackDisplayData currentStackData)
    {
        if (_blockData.grade == currentStackData.ID) //This block is currently part of the simulated stack
        {
            ResetState();
            if (_blockData.mastery == 0) // if made of glass, disappear
            {
                Simulate = false;
                LeanTween.scale(gameObject, Vector3.zero, 0.1f).setEaseOutCubic();
            }
            else // simulate normally
            {
                Simulate = true;
            }
        }
        else
        {
            Simulate = false;
        }
    }

    public void DisplayAsSelected()
    {
        GetComponent<Renderer>().material = _blockTypeInfo.SelectedMaterial;

    }

    public void DisplayAsNormal()
    {
        GetComponent<Renderer>().material = _blockTypeInfo.BaseMaterial;

    }

    private void ResetState()
    {
        LeanTween.cancel(gameObject);
        Simulate = false;
        gameObject.SetActive(true);
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        transform.localScale = _startScale;
    }

    private void TweenReset()
    {
        Simulate = false;
        gameObject.SetActive(true);
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, _startPosition, 1).setEaseOutCubic();
        LeanTween.rotate(gameObject, _startRotation.eulerAngles, 1).setEaseOutCubic();
        LeanTween.scale(gameObject, _startScale, 1).setEaseOutCubic();
    }

    public void OnSimulationStop(StackTestGameMode.StackDisplayData currentStackData)
    {
        TweenReset();
    }

    public class Factory : PlaceholderFactory<BlockData, BlockType, BlockDisplay> { }
}