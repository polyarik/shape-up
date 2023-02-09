using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private PlayerData playerData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler; //todo: use cloud data handler

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Data Persistence Manager in the scene.");
        }

        Instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();

        InvokeRepeating(nameof(SaveGame), GameConstants.saveRepeatRate, GameConstants.saveRepeatRate);
    }

    public void StartNewGame()
    {
        this.playerData = new PlayerData();
    }

    public void LoadGame()
    {
        // load any saved data from a file using data handler
        this.playerData = dataHandler.Load();

        if (this.playerData == null)
        {
            Debug.Log("No data was found. Initialasing data to defaults.");
            StartNewGame();
        }

        // push the loaded data to the scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(playerData);
        }
    }

    public void SaveGame()
    {
        // pass the data to the other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref playerData);
        }

        // save data to a file using data handler
        dataHandler.Save(playerData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    // ----debugging-----
    public void ResetGame()
    {
        StartNewGame();
        dataHandler.Save(playerData);
        LoadGame();
    }
}
