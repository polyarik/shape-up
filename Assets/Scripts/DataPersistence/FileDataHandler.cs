using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath;
    private string dataFileName;

    public FileDataHandler(string dataDirPath="", string dataFileName="")
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public PlayerData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        PlayerData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data from the file
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }

                    loadedData = JsonUtility.FromJson<PlayerData>(dataToLoad);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Error occured when trying to load data from file: {0}\n{1}", fullPath, e));
            }
        }

        return loadedData;
    }

    public void Save(PlayerData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            Debug.Log("Data has been saved to file: " + fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError(string.Format("Error occured when trying to save data to file: {0}\n{1}", fullPath, e));
        }
    }
}
