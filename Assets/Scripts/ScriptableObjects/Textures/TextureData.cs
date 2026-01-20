using UnityEngine;

[CreateAssetMenu(fileName = "Texture Data", menuName = "Texture Management/All Texture Data")]
public class TextureData : ScriptableObject
{
    public CellTextureData cellTextureData;
    public FrogTextureData frogTextureData;
    public GrapeTextureData grapeTextureData;
}
