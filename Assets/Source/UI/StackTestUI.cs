using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
public class StackTestUI : MonoBehaviour
{

    [Inject] private StackTestGameMode _stackTestGameMode;

    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;


    void Awake()
    {
        _stackTestGameMode.OnSelectedStackChanged += SetUpButtons;
    }

    void Start()
    {
        _stackTestGameMode.ChangeSelectedStack(0);
    }

    public void NextStack()
    {
        _stackTestGameMode.ChangeSelectedStack(+1);
    }

    public void PrevStack()
    {
        _stackTestGameMode.ChangeSelectedStack(-1);
    }

    void SetUpButtons(Vector2Int results)
    {
        _leftButton.interactable = results.x > 0; // indices left on the left
        _rightButton.interactable = results.y > results.x; // indices left on the right
    }

    public void StartSimulation()
    {
        _stackTestGameMode.StartSimulation();
    }

    public void StopSimulation()
    {
        _stackTestGameMode.StopSimulation();
    }


}
