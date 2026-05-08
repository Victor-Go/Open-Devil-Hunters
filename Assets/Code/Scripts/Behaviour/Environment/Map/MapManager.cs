using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Environment.Map
{
  public class TileContainer
  {
    public Vector2 Location { get; set; }
    public Vector2 TopRightPosition { get; set; }
    public Vector2 BottomLeftPosition { get; set; }
    public GameObject Tile { get; set; }
  }

  public class MapManager : PauseableGameObject, IStoreChangedHandler
  {
    private ObjectPool objectPool;

    private Vector2 tileSize;
    private MapConfiguration mapConfiguration;
    private GameObject tilePrefab;

    private Vector2[] playerPositions;
    private bool[] playerAlive;

    private TileContainer currentTileContainer;
    private List<TileContainer> tiles = new();

    // Graveyard map
    private float spawnBurningSkullTimeout = 15;
    private float spawnGhostTimeout;

    private int numberOfPlayers;

    protected override void Awake()
    {
      base.Awake();

      storeManager = StoreManager.Instance;
      objectPool = ObjectPool.Instance;
    }

    protected override void Start()
    {
      base.Start();

      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this);

      var levelConfig = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);

      playerAlive = levelConfig.PlayerAlive;
      numberOfPlayers = levelConfig.NumberOfPlayers;

      mapConfiguration = levelConfig.MapConfiguration;
      var mapConfig = levelConfig.MapConfiguration;

      tilePrefab = ResourceManager.Instance.GetResource(mapConfig.MapPrefabName);
      tileSize = tilePrefab.GetComponent<SpriteRenderer>().size;

      playerPositions = new Vector2[numberOfPlayers];

      SpawnMapTiles();
    }

    private TileContainer CreateTile(Vector2 location)
    {
      var tile = Instantiate(tilePrefab, transform, true);
      var map = tile.GetComponent<Assets.Code.Scripts.Src.Types.Map>();

      map.SetBackgroundDecorationConfigs(mapConfiguration.backgroundDecorationConfigs)
        .SetMapBackgroundNames(mapConfiguration.MapBackgroundNames)
        .SetMaximumDecorations(mapConfiguration.MaximumDecorations)
        .SetMinimumDecorations(mapConfiguration.MinimumDecorations)
        .Init();

      var position = new Vector2(
        location.x * tileSize.x,
        location.y * tileSize.y
      );
      tile.transform.position = position;

      var tileContainer = new TileContainer()
      {
        Tile = tile,
        Location = location,
        TopRightPosition = position + tileSize / 2,
        BottomLeftPosition = position - tileSize / 2,
      };
      tiles.Add(tileContainer);

      return tileContainer;
    }

    private List<TileContainer> GetTileAtLocation(Vector2 location)
    {
      return tiles.FindAll(container => container.Location.Equals(location));
    }

    private void SpawnMapTiles()
    {
      currentTileContainer ??= CreateTile(Vector2.zero);

      for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
      {
        if (!playerAlive[playerNumber])
        {
          continue;
        }

        if (playerPositions[playerNumber].x < currentTileContainer.BottomLeftPosition.x)
        {
          currentTileContainer = GetTileAtLocation(currentTileContainer.Location + new Vector2(-1, 0))[0];
          break;
        }

        if (playerPositions[playerNumber].x > currentTileContainer.TopRightPosition.x)
        {
          currentTileContainer = GetTileAtLocation(currentTileContainer.Location + new Vector2(1, 0))[0];
          break;
        }

        if (playerPositions[playerNumber].y < currentTileContainer.BottomLeftPosition.y)
        {
          currentTileContainer = GetTileAtLocation(currentTileContainer.Location + new Vector2(0, -1))[0];
          break;
        }

        if (playerPositions[playerNumber].y > currentTileContainer.TopRightPosition.y)
        {
          currentTileContainer = GetTileAtLocation(currentTileContainer.Location + new Vector2(0, 1))[0];
          break;
        }
      }

      for (var i = -1; i <= 1; i++)
      {
        for (var j = -1; j <= 1; j++)
        {
          if (GetTileAtLocation(currentTileContainer.Location + new Vector2(i, j)).Count == 0)
          {
            CreateTile(currentTileContainer.Location + new Vector2(i, j));
          }
        }
      }
    }

    private void RecycleMapTiles()
    {
      tiles.RemoveAll(tile =>
        {
          for (var i = -1; i <= 1; i++)
          {
            for (var j = -1; j <= 1; j++)
            {
              if (currentTileContainer.Location + new Vector2(i, j) == tile.Location) return false;
            }
          }

          Destroy(tile.Tile);

          return true;
        }
      );
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          var levelConfigs = (LevelConfigurationState)state;
          numberOfPlayers = levelConfigs.NumberOfPlayers;
          playerAlive = levelConfigs.PlayerAlive;
          break;
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    #region Handle different maps.

    // Graveyard
    private void SpawnBurningSkull()
    {
      var skull = objectPool.GetObject("Map/Graveyard/Decorations/GraveyardBurningSkull");
      skull.transform.SetParent(null);
    }

    private void SpawnGhost()
    {
      var ghost = objectPool.GetObject("Map/Graveyard/Decorations/GraveyardGhost");
      ghost.transform.SetParent(null);
    }

    private void HandleGraveyardOnFixedUpdate()
    {
      spawnBurningSkullTimeout -= Time.fixedDeltaTime;
      spawnGhostTimeout -= Time.fixedDeltaTime;

      if (spawnBurningSkullTimeout <= 0)
      {
        spawnBurningSkullTimeout = Random.Range(10, 30);
        SpawnBurningSkull();
      }

      if (spawnGhostTimeout <= 0)
      {
        spawnGhostTimeout = Random.Range(10, 20);
        int count = Random.Range(1, 3);
        for (int i = 0; i < count; i++)
        {
          SpawnGhost();
        }
      }
    }

    #endregion

    private float mapDetectCountdown = 1;

    private void FixedUpdate()
    {
      if (paused) return;

      switch (mapConfiguration.MapName)
      {
        case MapNames.GRAVEYARD: HandleGraveyardOnFixedUpdate(); break;
      }

      mapDetectCountdown -= Time.fixedDeltaTime;
      if (mapDetectCountdown <= 0)
      {
        mapDetectCountdown = 1;
        var state = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore);
        playerPositions = state.PlayerPositions;
        SpawnMapTiles();
        RecycleMapTiles();
      }
    }
  }
}