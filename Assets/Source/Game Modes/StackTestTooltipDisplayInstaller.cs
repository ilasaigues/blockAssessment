using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StackTestTooltipDisplayInstaller : MonoInstaller
{
    [SerializeField] StackTestTooltipDisplay _displayInstance;
    public override void InstallBindings()
    {
        Container.Bind<StackTestTooltipDisplay>().FromInstance(_displayInstance);
    }
}
