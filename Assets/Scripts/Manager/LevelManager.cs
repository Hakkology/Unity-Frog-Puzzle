using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour, ISingleton
{
    public LevelConfig currentLevelConfig;
    private TileManager tileManager;
    private TextureManager textureManager;
    public Transform levelParent;

    public GameObject cellPrefab;
    private Dictionary<string, GameObject> prefabDictionary;

    private bool isReady = false;
    public bool IsReady => isReady;

    public void Init()
    {
        tileManager = SingletonManager.GetSingleton<TileManager>();
        textureManager = SingletonManager.GetSingleton<TextureManager>();

        prefabDictionary = new Dictionary<string, GameObject>();
        if (tileManager != null && tileManager.IsReady)
        {
            LoadPrefabs();
            isReady = true;
        }
    }
    private void LoadPrefabs()
    {
        GameObject cellPrefab = Resources.Load<GameObject>("Prefabs/Objects/Cell");
        if (cellPrefab == null)
        {
            Debug.LogError("Cell prefab could not be loaded from Resources/Prefabs/Objects/Cell");
        }
        else
        {
            prefabDictionary.Add("Cell", cellPrefab);
        }

        string[] objectNames = { "Frog", "Grape", "Arrow" };
        foreach (var objectName in objectNames)
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Objects/{objectName}");
            if (prefab == null)
            {
                Debug.LogError($"{objectName} prefab could not be loaded from Resources/Prefabs/Objects/{objectName}");
            }
            else
            {
                prefabDictionary.Add(objectName, prefab);
            }
        }
    }

    /// <summary>
    /// Generates a level based on the specified level configuration.
    /// </summary>
    /// <param name="levelConfig">The configuration of the level to generate.</param>
    public void GenerateLevel(LevelConfig levelConfig)
    {
        if (!isReady || cellPrefab == null)
        {
            Debug.LogError("LevelManager is not ready or cellPrefab is not loaded.");
            return;
        }

        ClearExistingLevel();
        SingletonManager.GetSingleton<GameManager>()?.ClearFrogs();

        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 6; col++)
            {
                Vector3 position = new Vector3(col, 0, row);
                GameObject cellInstance = Instantiate(prefabDictionary["Cell"], position, Quaternion.identity, transform);
                Tile tile = cellInstance.GetComponent<Tile>();

                if (tile != null)
                {
                    tile.Initialize(col, row);
                    AddConfiguredTileObjects(tile, levelConfig.GetTileConfig(col, row));
                    tileManager.RegisterTile(tile);
                }
                else
                {
                    Debug.LogError("Tile component could not be found on the instantiated cellPrefab.");
                }
            }
        }
    }

    /// <summary>
    /// Clears the existing level by destroying all child objects.
    /// </summary>
    private void ClearExistingLevel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Adds configured tile objects to the specified tile based on the tile configuration.
    /// </summary>
    /// <param name="tile">The tile to add the objects to.</param>
    /// <param name="tileConfig">The configuration of the tile.</param>
    private void AddConfiguredTileObjects(Tile tile, LevelConfig.TileConfig tileConfig)
    {
        if (tileConfig == null)
        {
            return;
        }

        foreach (var objConfig in tileConfig.objects)
        {
            if (prefabDictionary.TryGetValue(objConfig.objectType, out GameObject prefab))
            {
                Vector3 position = tile.transform.position + Vector3.up * objConfig.verticalPosition;
                GameObject objInstance = Instantiate(prefab, position, Quaternion.identity, tile.transform);

                // Setup the object's properties based on objConfig
                BaseObject baseObject = objInstance.GetComponent<BaseObject>();
                    if (baseObject != null)
                    {
                        tile.AddTileObject(baseObject);

                        if (baseObject is DirectionalEntity directionalEntity)
                        {
                            directionalEntity.facingDirection = objConfig.direction;
                            directionalEntity.RotateObjectToFaceDirection();
                        }

                        // Handle Texture Change based on object type and color
                        if (baseObject is Frog)
                        {
                            var textures = textureManager.GetFrogTexture(objConfig.color);
                            ((GridEntity)baseObject).HandleTextureChange(textures.frogTexture, textures.cellTexture, objConfig.color);
                        }
                        else if (baseObject is Grape)
                        {
                            var textures = textureManager.GetGrapeTexture(objConfig.color);
                            ((GridEntity)baseObject).HandleTextureChange(textures.grapeTexture, textures.cellTexture, objConfig.color);
                        }
                        else if (baseObject is Arrow)
                        {
                            var textures = textureManager.GetArrowTexture(objConfig.color);
                            ((GridEntity)baseObject).HandleTextureChange(textures.arrowTexture, textures.cellTexture, objConfig.color);
                        }
                    }
            }
            else
            {
                Debug.LogError($"Prefab for object type {objConfig.objectType} not found in dictionary.");
            }
        }

        tile.GetTopmostObject()?.gameObject.SetActive(true);
    }


}


