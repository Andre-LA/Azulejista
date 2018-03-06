using UnityEngine;
using Azulejista;

namespace Azulejista {
  public class AzulejistaTileSelector : MonoBehaviour {
    public string[] ids;
    public GameObject[] tileGameObjects;

    string GetTileID (AdjacentTiles adjacent_tiles) {
      string[] digits = new string[8];

      digits[0] = adjacent_tiles.middle_up ? "1" : "0";
      digits[1] = adjacent_tiles.right_up ? "1" : "0";
      digits[2] = adjacent_tiles.right_middle ? "1" : "0";
      digits[3] = adjacent_tiles.right_bottom  ? "1" : "0";
      digits[4] = adjacent_tiles.middle_bottom ? "1" : "0";
      digits[5] = adjacent_tiles.left_bottom ? "1" : "0";
      digits[6] = adjacent_tiles.left_middle ? "1" : "0";
      digits[7] = adjacent_tiles.left_up ? "1" : "0";

      return string.Concat(
        digits[0],
        digits[1],
        digits[2],
        digits[3],
        digits[4],
        digits[5],
        digits[6],
        digits[7]
      );
    }

    string GetSimplifiedTileId (AdjacentTiles adjacent_tiles) {
      string[] digits = new string[4];

      digits[0] = adjacent_tiles.middle_up ? "1" : "0";    
      digits[1] = adjacent_tiles.right_middle ? "1" : "0";    
      digits[2] = adjacent_tiles.middle_bottom ? "1" : "0";    
      digits[3] = adjacent_tiles.left_middle ? "1" : "0";
      

      return string.Concat(
        digits[0],
        digits[1],
        digits[2],
        digits[3]
      );
    }
    
    int GetTileIdIndex(string id) {
      for (int i = 0; i < ids.Length; i++)
        if (ids[i] == id)
          return i;

      return -1;
    }

    public GameObject GetSuitableGbj(AdjacentTiles adjacent_tiles) {
      var complete_id = GetTileID(adjacent_tiles);
      var id_index = GetTileIdIndex(complete_id);

      if (id_index == -1) {
        complete_id = GetSimplifiedTileId(adjacent_tiles);
        id_index = GetTileIdIndex(complete_id);

        if (id_index == -1)
          id_index = 0;
      }
      
      return tileGameObjects[id_index];
    }

    #if UNITY_EDITOR
    void OnValidate() {
      if (ids.Length != tileGameObjects.Length)
        Debug.LogWarning("Ids and tile GameObjects arrays have different lengths!");
    }

    #endif
  }
}