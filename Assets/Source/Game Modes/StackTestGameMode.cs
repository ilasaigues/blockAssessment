using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
public class StackTestGameMode : MonoBehaviour
{

    #region Injected
    [Inject] private IBlockDataProvider _blockDataProvider;
    [Inject] private BlockDisplay.Factory _blockDisplayFactory;

    [Inject] private StackTestTooltipDisplay _tooltipDisplay;
    #endregion


    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraContainer;

    [SerializeField] private Vector3 _cameraOffset;

    [SerializeField][Range(0, 90)] private float _cameraMaxAngle;

    [SerializeField] private float _cameraRotationSensitivity;

    [SerializeField][Range(0, 1)] private float _cameraHorizontalSensitivity;

    [Header("Block Settings")]

    [SerializeField] private List<BlockType> _blockTypeData;

    [SerializeField] private Vector3 _blockScale;
    [SerializeField] private float _blockSeparation;

    [System.Serializable]
    public class StackDisplayData
    {
        public string ID;
        public Transform Spawn;
    }

    [SerializeField] private List<StackDisplayData> _stackData = new List<StackDisplayData>();

    private int _currentStackId;

    private float _cameraRotationY;

    public bool CanMoveLeft => _currentStackId > 0;
    public bool CanMoveRight => _currentStackId < _stackData.Count - 1;


    private bool _simulating;
    public System.Action<StackDisplayData> OnSimulationStart = _ => { };
    public System.Action<StackDisplayData> OnSimulationStop = _ => { };
    public System.Action<Vector2Int> OnSelectedStackChanged = _ => { };


    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(() => _blockDataProvider.HasLoaded);
        SetUpStacks();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCamera();

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                OnSelectedBlockChanged(hit.collider.gameObject.GetComponent<BlockDisplay>());
            }
            else
            {
                OnSelectedBlockChanged(null);
            }
        }
    }

    void HandleCamera()
    {
        _cameraContainer.transform.position = Vector3.Lerp(_cameraContainer.transform.position, _stackData[_currentStackId].Spawn.position + _cameraOffset, _cameraHorizontalSensitivity);
        if (Input.GetMouseButton(0)) // if we're Right clicking
        {
            _cameraRotationY += Input.GetAxis("Mouse X") * _cameraRotationSensitivity * Time.deltaTime;
            _cameraRotationY = Mathf.Clamp(_cameraRotationY, -_cameraMaxAngle, _cameraMaxAngle);
            _cameraContainer.transform.rotation = Quaternion.Euler(0, _cameraRotationY, 0);
        }
    }

    private void SetUpStacks()
    {
        var allBlocks = _blockDataProvider.GetAllBlocks();
        foreach (var stackData in _stackData)
        {
            var stackBlocks = allBlocks[stackData.ID];
            for (int i = 0; i < stackBlocks.Count; i++)
            {
                int floorNumber = Mathf.FloorToInt(i / 3);
                int idInFloor = i % 3; // range 0-2, id of the block in the current floor

                var blockData = stackBlocks[i];

                var position = stackData.Spawn.position + new Vector3(0, (floorNumber + 0.5f) * _blockScale.y, 0); // center of the current floor of the stack

                Quaternion rotation = Quaternion.identity;

                if (floorNumber % 2 == 0) // if the floor number is even, tiles stack on the X axis
                {
                    position += new Vector3((idInFloor - 1) * (_blockScale.x + _blockSeparation), 0, 0);
                }
                else  // if the floor number is even, tiles stack on the Z axis, and we rotate them 90 degrees in the Y axis
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                    position += new Vector3(0, 0, (idInFloor - 1) * (_blockScale.x + _blockSeparation));
                }

                //var prefab = _blockPrefabs[blockData.mastery];

                var blockDisplay = _blockDisplayFactory.Create(blockData, _blockTypeData[blockData.mastery]);
                blockDisplay.transform.SetParent(stackData.Spawn);
                blockDisplay.SetTransformData(position, rotation, _blockScale);
                blockDisplay.Simulate = false;
            }
        }
    }

    public void ChangeSelectedStack(int offset)
    {
        StopSimulation();
        _currentStackId += offset;
        OnSelectedStackChanged(new Vector2Int(_currentStackId, _stackData.Count - 1));
    }

    public void OnSelectedBlockChanged(BlockDisplay blockDisplay)
    {
        _tooltipDisplay.ShowTooltip(blockDisplay);
        if (blockDisplay != null)
        {
            var newStackID = _stackData.IndexOf(_stackData.Where(sd => sd.ID == blockDisplay.BlockData.grade).First());
            if (newStackID != _currentStackId)
            {
                ChangeSelectedStack(newStackID - _currentStackId);
            }
        }
        else
        {
            StopSimulation();
        }
    }

    public void StartSimulation()
    {
        _simulating = true;
        OnSimulationStart(_stackData[_currentStackId]);
    }

    public void StopSimulation()
    {
        _simulating = false;
        OnSimulationStop(_stackData[_currentStackId]);
    }

}
