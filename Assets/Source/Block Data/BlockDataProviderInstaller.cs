using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class BlockDataProviderInstaller : MonoInstaller
{

    [SerializeField]
    private BlockDataProvider _dataProvider;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<IBlockDataProvider>().FromInstance(_dataProvider);
    }
}
