using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
public class StackTestGameModeInstaller : MonoInstaller
{
    [SerializeField] private StackTestGameMode _instance;

    public override void InstallBindings()
    {
        Container.Bind<StackTestGameMode>().FromInstance(_instance);
    }
}
