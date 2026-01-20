using UnityEngine;

[CreateAssetMenu(fileName = "Cell Texture Data", menuName = "Texture Management/Cell Texture Data")]
public class CellTextureData : ScriptableObject
{
    public Texture2D[] cellTextures;
}
