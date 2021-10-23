using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DataInfo;

public class BuildingCommonData
{
    public string name;
    public int supply;
    public int[] resourceCost;
    public Color fontColor;
}

public class ResourceBuildingData : BuildingCommonData
{
    public GameResourceType type;
    public int maxStorage;
    public float curStorage;
    public int storageUp;
    public float output;
}

public class BarrackData : BuildingCommonData
{
    public string type;
    public float hp;
    public float attack;
    public float speed;
}

public class DefenceTowerData : BuildingCommonData
{
    public int damage;
    public int attackCount;
    public float attackSpeed;
    public float attackRadius;
}

public class ETCBuildingData : BuildingCommonData
{
    public int firstValue;
    public int firstValueIncreace;
    public int secondValue;
    public int secondValueIncreace;
}

public struct UnitData
{
    public string name;
    public string type;
    public float hp;
    public float speed;
    public float attack;
    public float attackRadius; 
}

public struct WaveData
{
    public int wave;
    public int color;
    public string mainAttacker;
    public int mainLv;
    public int mainMin;
    public int mainMax;
    public string subAttacker1;
    public int sub1Lv;
    public int sub1Min;
    public int sub1Max;
    public string subAttacker2;
    public int sub2Lv;
    public int sub2Min;
    public int sub2Max;
    public int callCount1;
    public int callCount2;
    public int callCount3;
    public float SpawnTime;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private Dictionary<string, ResourceBuildingData> resourceData = new Dictionary<string, ResourceBuildingData>();
    private Dictionary<string, BarrackData> barrackData = new Dictionary<string, BarrackData>();
    private Dictionary<string, DefenceTowerData> defenceTowerData = new Dictionary<string, DefenceTowerData>();
    private Dictionary<string, ETCBuildingData> etcData = new Dictionary<string, ETCBuildingData>();
    private Dictionary<string, UnitData> unitData = new Dictionary<string, UnitData>();
    private Dictionary<string, WaveData> waveData = new Dictionary<string, WaveData>();

    private string dataPath;

    public void Initialize()
    {
        dataPath = Application.persistentDataPath + "/gameData.dat";
    }

    public void Save(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(dataPath);

        GameData data = new GameData();
        data.buildingDatas = gameData.buildingDatas;
        data.curResource = gameData.curResource;
        data.waveLevel = gameData.waveLevel;

        bf.Serialize(file, data);
        file.Close();
    }

    public GameData Load()
    {
        if(File.Exists(dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            return data;
        }
        else
        {
            GameData data = new GameData();

            return data;
        }
    }

    private void Awake() 
    {
        instance = this;

        LoadResourceData();
        LoadBarrackData();
        LoadDefenceTowerData();
        LoadETCData();
        LoadUnitData();
        LoadUnitRedData();
        LoadUnitYellowData();
        LoadUnitVioletData();
        LoadWaveData();
    }

    public ResourceBuildingData GetResourceData(string key)
    {
        if(!resourceData.ContainsKey(key))
            return new ResourceBuildingData();
        
        return resourceData[key];
    }

    public BarrackData GetBarrackData(string key)
    {
        if(!barrackData.ContainsKey(key))
            return new BarrackData();

        return barrackData[key];
    }

    public DefenceTowerData GetDefenceTowerData(string key)
    {
        if(!defenceTowerData.ContainsKey(key))
            return new DefenceTowerData();

        return defenceTowerData[key];
    }

    public ETCBuildingData GetETCData(string key)
    {
        if(!etcData.ContainsKey(key))
            return new ETCBuildingData();

        return etcData[key];
    }
    
    public UnitData GetUnitData(string key)
    {
        if (!unitData.ContainsKey(key))
            return new UnitData();
        return unitData[key];       
    }

    public WaveData GetWaveData(string key)
    {
        if (!waveData.ContainsKey(key))
            return new WaveData();
       
        return waveData[key];
    }

    private void LoadResourceData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/ResourceBuilding");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string [] rowData = temp.Split('\n');

        for(int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split('\t');

            string key = colData[0];

            ResourceBuildingData data = new ResourceBuildingData();
            data.name = colData[1];
            data.type = (GameResourceType)int.Parse(colData[2]);
            data.maxStorage = int.Parse(colData[3]);
            data.storageUp = int.Parse(colData[4]);
            data.output = float.Parse(colData[5]);
            data.supply = int.Parse(colData[6]);
            data.resourceCost = new int[(int)GameResourceType.SUPPLY + 1];
            data.resourceCost[0] = int.Parse(colData[7]);
            data.resourceCost[1] = int.Parse(colData[8]);
            data.resourceCost[2] = int.Parse(colData[9]);
            data.resourceCost[3] = int.Parse(colData[10]);
            data.curStorage = 0.0f;
            data.fontColor = new Color(6, 255, 0);

            resourceData.Add(key, data);

        }
    }

    private void LoadBarrackData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/BarrackBuilding");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string [] rowData = temp.Split('\n');

        for(int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split('\t');

            string key = colData[0];

            BarrackData data = new BarrackData();
            data.name = colData[1];
            data.type = colData[2];
            data.hp = float.Parse(colData[3]);
            data.attack = float.Parse(colData[4]);
            data.speed = float.Parse(colData[5]);
            data.supply = int.Parse(colData[6]);
            data.resourceCost = new int[(int)GameResourceType.SUPPLY + 1];
            data.resourceCost[0] = int.Parse(colData[7]);
            data.resourceCost[1] = int.Parse(colData[8]);
            data.resourceCost[2] = int.Parse(colData[9]);
            data.resourceCost[3] = int.Parse(colData[10]);
            data.fontColor = new Color(255, 255, 255);

            barrackData.Add(key, data);
        }
    }

    private void LoadDefenceTowerData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/DefenceBuilding");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string [] rowData = temp.Split('\n');

        for(int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split('\t');

            string key = colData[0];

            DefenceTowerData data = new DefenceTowerData();

            data.name = colData[1];
            data.damage = int.Parse(colData[2]);
            data.attackCount = int.Parse(colData[3]);
            data.attackSpeed = float.Parse(colData[4]);
            data.attackRadius = float.Parse(colData[5]);
            data.supply = int.Parse(colData[6]);
            data.resourceCost = new int[(int)GameResourceType.SUPPLY + 1];
            data.resourceCost[0] = int.Parse(colData[7]);
            data.resourceCost[1] = int.Parse(colData[8]);
            data.resourceCost[2] = int.Parse(colData[9]);
            data.resourceCost[3] = int.Parse(colData[10]);
            data.fontColor = new Color(0, 158, 255);

            defenceTowerData.Add(key, data);
        }
    }

    private void LoadETCData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/ETCBuilding");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string [] rowData = temp.Split('\n');

        for(int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split('\t');

            string key = colData[0];

            ETCBuildingData data = new ETCBuildingData();

            data.name = colData[1];
            data.firstValue = int.Parse(colData[2]);
            data.firstValueIncreace = int.Parse(colData[3]);
            data.secondValue = int.Parse(colData[4]);
            data.secondValueIncreace = int.Parse(colData[5]);
            data.resourceCost = new int[(int)GameResourceType.SUPPLY + 1];
            data.resourceCost[0] = int.Parse(colData[6]);
            data.resourceCost[1] = int.Parse(colData[7]);
            data.resourceCost[2] = int.Parse(colData[8]);
            data.resourceCost[3] = int.Parse(colData[9]);
            data.fontColor = new Color(6, 255, 0);

            etcData.Add(key, data);
        }
    }

    private void LoadUnitData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/UnitData");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string [] rowData = temp.Split('\n');

        for(int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split('\t');

            string key = colData[0];

            UnitData data;
            
            data.name = colData[1];
            data.type = colData[2];
            data.hp = float.Parse(colData[3]);
            data.speed = float.Parse(colData[4]);
            data.attack = float.Parse(colData[5]);
            data.attackRadius = float.Parse(colData[6]);

            unitData.Add(key, data);
        }
    }

    private void LoadUnitRedData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/Unit_Red");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] rowData = temp.Split('\n');

        for (int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split('\t');

            string key = colData[0];

            UnitData data;

            data.name = colData[1];
            data.type = colData[2];
            data.hp = float.Parse(colData[3]);
            data.speed = float.Parse(colData[4]);
            data.attack = float.Parse(colData[5]);
            data.attackRadius = float.Parse(colData[6]);

            unitData.Add(key, data);
        }
    }

    private void LoadUnitYellowData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/Unit_Yellow");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] rowData = temp.Split('\n');

        for (int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split('\t');

            string key = colData[0];

            UnitData data;

            data.name = colData[1];
            data.type = colData[2];
            data.hp = float.Parse(colData[3]);
            data.speed = float.Parse(colData[4]);
            data.attack = float.Parse(colData[5]);
            data.attackRadius = float.Parse(colData[6]);

            unitData.Add(key, data);
        }
    }

    private void LoadUnitVioletData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/Unit_Violet");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] rowData = temp.Split('\n');

        for (int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split('\t');

            string key = colData[0];

            UnitData data;

            data.name = colData[1];
            data.type = colData[2];
            data.hp = float.Parse(colData[3]);
            data.speed = float.Parse(colData[4]);
            data.attack = float.Parse(colData[5]);
            data.attackRadius = float.Parse(colData[6]);

            unitData.Add(key, data);
        }
    }

    private void LoadWaveData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/WaveData");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] rowData = temp.Split('\n');

        for (int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split('\t');

            string key = colData[0];

            WaveData data;

            data.wave = int.Parse(colData[1]);
            data.color = int.Parse(colData[2]);
            data.mainAttacker = colData[3];
            data.mainLv = int.Parse(colData[4]);
            data.mainMin = int.Parse(colData[5]);
            data.mainMax = int.Parse(colData[6]);
            data.subAttacker1 = colData[7];
            data.sub1Lv = int.Parse(colData[8]);
            data.sub1Min = int.Parse(colData[9]);
            data.sub1Max = int.Parse(colData[10]);
            data.subAttacker2 = colData[11];
            data.sub2Lv = int.Parse(colData[12]);
            data.sub2Min = int.Parse(colData[13]);
            data.sub2Max = int.Parse(colData[14]);
            data.callCount1 = int.Parse(colData[15]);
            data.callCount2 = int.Parse(colData[16]);
            data.callCount3 = int.Parse(colData[17]);
            data.SpawnTime = float.Parse(colData[18]);

            waveData.Add(key, data);
        }
    }
}
