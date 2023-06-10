using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
public class BlockDataProvider : MonoBehaviour, IBlockDataProvider
{
    public List<BlockData> DataMap = new List<BlockData>();

    [SerializeField]
    private string _apiURL;

    public bool HasLoaded => _hasLoaded;

    private bool _hasLoaded = false;

    void Start()
    {
        StartCoroutine(GetAPIData());
    }

    private IEnumerator GetAPIData()
    {
        UnityWebRequest wr = UnityWebRequest.Get(_apiURL);
        yield return wr.SendWebRequest();
        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wr.error);
        }
        else
        {
            ParseAPIData(wr.downloadHandler.text);
        }
    }

    private void ParseAPIData(string jsonData)
    {
        DataMap = JsonConvert.DeserializeObject<List<BlockData>>(jsonData);
        _hasLoaded = true;
    }

    BlockData IBlockDataProvider.GetBlockByID(int id)
    {
        return DataMap[id];
    }

    public Dictionary<string, List<BlockData>> GetAllBlocks()
    {
        var result = new Dictionary<string, List<BlockData>>();
        foreach (BlockData nextData in DataMap)
        {
            if (!result.ContainsKey(nextData.grade))
            {
                result[nextData.grade] = new List<BlockData>(); // if the result dictionary doesn't yet have this grade's list, initialize it
            }
            result[nextData.grade].Add(nextData);
        }


        foreach (KeyValuePair<string, List<BlockData>> kvp in result)
        {
            var blockList = kvp.Value;

            // Sorting each group, according to spec:
            // - By domain name ascending,
            // - Then by cluster name ascending,
            // - Then by standardID ascending
            var orderedList = blockList.OrderBy(block => block.domain)
                                       .ThenBy(block => block.cluster)
                                       .ThenBy(block => block.standardid)
                                       .ToList();
            result[kvp.Key].Clear();
            result[kvp.Key].AddRange(orderedList);
        }
        return result;
    }
}
