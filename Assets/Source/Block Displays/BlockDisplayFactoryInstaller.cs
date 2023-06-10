using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class BlockDisplayFactoryInstaller : MonoInstaller
{
    [SerializeField] private BlockDisplay _blockPrefab;


    public override void InstallBindings()
    {
        Container.BindFactory<BlockData, BlockType, BlockDisplay, BlockDisplay.Factory>().FromComponentInNewPrefab(_blockPrefab);
    }
}
