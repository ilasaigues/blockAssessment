using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Block Type")]
public class BlockType : ScriptableObject
{
    public Material BaseMaterial;
    public Material SelectedMaterial;
}
