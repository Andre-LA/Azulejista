// There's some portuguese comments, I will translate the comments to portuguese later

using UnityEngine;
using Azulejista;

namespace Azulejista {
  public struct AdjacentTiles {
    public bool left_up;  public bool middle_up;  public bool right_up;
    public bool left_middle; /* middle_middle */   public bool right_middle;
    public bool left_bottom; public bool middle_bottom; public bool right_bottom; 
  }

  [ExecuteInEditMode]
  public class AzulejistaTileMaker : MonoBehaviour {

    #if UNITY_EDITOR
    
    public string editor_lang;

    #endif

    [System.Serializable]
    public struct MapLine {
      public bool[] line;
    }

    public MapLine[] map;
    
    public Transform mapParent;
    public AzulejistaTileSelector tileSelector;

    public int width;
    public int height;
    
    public Vector2 stepX;
    public Vector2 stepY;
    public float zStep;

    AdjacentTiles GetAdjacentTiles(int px, int py) {
      AdjacentTiles azul_vizinhos = new AdjacentTiles();

      var px_esq   = px - 1;
      var px_dir   = px + 1;
      var py_cima  = py - 1;
      var py_baixo = py + 1;

      var px_primeiro = px == 0;
      var px_ultimo   = px == (width - 1);

      var py_primeiro = py == 0;
      var py_ultimo   = py == (height - 1);

      azul_vizinhos.middle_up =
        py_primeiro
          ? false
          : map[py_cima].line[px];

      azul_vizinhos.right_up = 
        px_ultimo || py_primeiro
          ? false
          : map[py_cima].line[px_dir];

      azul_vizinhos.right_middle = 
        px_ultimo
          ? false
          : map[py].line[px_dir];

      azul_vizinhos.right_bottom = 
        px_ultimo || py_ultimo
          ? false
          : map[py_baixo].line[px_dir];

      azul_vizinhos.middle_bottom = 
        py_ultimo
          ? false
          : map[py_baixo].line[px];

      azul_vizinhos.left_middle = 
        px_primeiro
          ? false
          : map[py].line[px_esq];

      azul_vizinhos.left_up = 
        px_primeiro || py_primeiro
          ? false
          : map[py_cima].line[px_esq];

      return azul_vizinhos;
    }

    void AlignParent() {
      var pos = transform.parent.position;
      pos.x = Mathf.Ceil(pos.x) - 0.5f;
      pos.y = Mathf.Ceil(pos.y) - 0.5f;
      transform.parent.position = pos;
    }

    public void DestroyMap() {
      for (int i = mapParent.childCount - 1; i >= 0; i--) {
        #if UNITY_EDITOR
          DestroyImmediate(mapParent.GetChild(i).gameObject);
        #else
          Destroy(mapParent.GetChild(i).gameObject);
        #endif
      }
    }

    public void GenerateMap() {
      //  ATTENTION: if you change this algorithm, copy it and paste it into OnDrawGizmosSelected: D
      var instance_pos = Vector3.zero;

      for (int tile_line = 0; tile_line < map.Length; tile_line++) {
        for (int tile_column = 0; tile_column < map[tile_line].line.Length; tile_column++) {
          var pos_z = 0f;

          // se n há azulejo aqui, pule pro próximo
          if (map[tile_line].line[tile_column]) {
            // Obterm azulejo adequado
            GameObject tile_to_instance = tileSelector.GetSuitableGbj(GetAdjacentTiles(tile_column, tile_line));
            
            // Cria azulejo
            var instanced_tile_gbj = Instantiate<GameObject>(tile_to_instance, instance_pos + mapParent.position, Quaternion.identity);

            // define o mapa_pai como pai desse azulejo
            instanced_tile_gbj.transform.SetParent(mapParent);

            // mudar nome para algo identificável
            instanced_tile_gbj.name = instanced_tile_gbj.name.Replace("(Clone)", string.Concat(" [x: ", tile_column.ToString(), ", y: ", tile_line.ToString(),"]"));

            pos_z = zStep;
          } else
            pos_z = 0;
          
          // dá passo em x (e em z)
          instance_pos += new Vector3(stepX.x, stepX.y, pos_z);
        }
        
        // dá passo em y
        instance_pos = new Vector3(0, 0, instance_pos.z) + new Vector3(stepY.x, stepY.y, 0) * (tile_line+1);
      }
    }

    #if UNITY_EDITOR
    
    public Color previewColor;
    public float previewSize;

    void OnDrawGizmosSelected() {
      // ATENÇÂO cópia do algorítmo gerador de gbjs, atualize isso ao atualizar o de cima :3

      if (previewColor.a < 0.1f)
        return;

      Gizmos.color = previewColor;

      var pt_lu = Vector3.zero;
      var pt_ru = Vector3.zero;
      var pt_lb = Vector3.zero;
      var pt_rb = Vector3.zero;
      
      var instance_pos = Vector3.zero;

      for (int i_tile_line = 0; i_tile_line < height; i_tile_line++) {
        for (int i_tile_comumn = 0; i_tile_comumn < width; i_tile_comumn++) {
          var pos_z = 0f;
          var final_pos = instance_pos + mapParent.position;
          Gizmos.DrawWireCube(final_pos, Vector3.one * previewSize / 3f);

          if (i_tile_line < map.Length && i_tile_comumn < map[i_tile_line].line.Length && map[i_tile_line].line[i_tile_comumn]) {
            Gizmos.DrawWireSphere(final_pos, previewSize);          
            pos_z = zStep;
          } else
            pos_z = 0;
            
          if (i_tile_line == 0) {
            if (i_tile_comumn == 0)
              pt_lu = final_pos;
            else if (i_tile_comumn == width - 1)
              pt_ru = final_pos;
          } else if (i_tile_line == height - 1) {
            if (i_tile_comumn == 0)
              pt_lb = final_pos;
            else if (i_tile_comumn == width - 1)
              pt_rb = final_pos;
          }

          instance_pos += new Vector3(stepX.x, stepX.y, pos_z);
        }
        
        instance_pos = new Vector3(0, 0, instance_pos.z) + new Vector3(stepY.x, stepY.y, 0) * (i_tile_line+1);
      }

      Gizmos.DrawLine(pt_lu, pt_ru);
      Gizmos.DrawWireCube(pt_lu, Vector3.one * previewSize / 1.5f);
      Gizmos.DrawLine(pt_ru, pt_rb);
      Gizmos.DrawWireCube(pt_ru, Vector3.one * previewSize / 1.5f);
      Gizmos.DrawLine(pt_rb, pt_lb);
      Gizmos.DrawWireCube(pt_rb, Vector3.one * previewSize / 1.5f);
      Gizmos.DrawLine(pt_lb, pt_lu);
      Gizmos.DrawWireCube(pt_lb, Vector3.one * previewSize / 1.5f);
    }

    void OnValidate() {
      if (!tileSelector)
        tileSelector = GetComponent<AzulejistaTileSelector>();
        
      AlignParent();
      if (height < 0) {
        height = 0;
        Debug.LogWarning("Height must be positive");
      }
      if (width < 0) {
        width = 0;
        Debug.LogWarning("Width must be positive");
      }
    }

    #endif
  }
}
