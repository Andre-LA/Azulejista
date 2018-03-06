using UnityEngine;
using System.Collections.Generic;

namespace Azulejista {

  [System.Serializable]
  public struct TileMakerEditorLabels {
    public string lang;
    public string editor_only;
    public string language;
    public string references;
    public string map_parent;
    public string tile_selector;
    public string map_dimensions;
    public string width;
    public string height;
    public string step_by_tile;
    public string step_x;
    public string step_y;
    public string z_step;
    public string gizmos_preview;
    public string preview_color;
    public string preview_size;
    public string edit;
    public string close_edition;
    public string regenerate_map_data;
    public string generate_map;
  }
  
  public class AzulejistaLabels {
    static string[] possible_languages = {
      // insert any language here, even if is not available
      "en-US",
      "pt-BR",
    };

    public static string[] GetAvailableLanguages() {
      List<string> available_languages = new List<string>();

      for (int i = 0; i < possible_languages.Length; i++) {
        var filename = UnityEngine.Application.dataPath + "/Editor/Azulejista/localized_texts/" + possible_languages[i] + ".json";
        if (System.IO.File.Exists(filename))
          available_languages.Add(possible_languages[i]);
      }

      return available_languages.ToArray();
    }

    public static TileMakerEditorLabels GetTileMakerEditorLabels(string lang) {
      var filename = Application.dataPath + "/Editor/Azulejista/localized_texts/" + lang + ".json";
      if (System.IO.File.Exists(filename))
        return JsonUtility.FromJson<TileMakerEditorLabels>(System.IO.File.ReadAllText(filename));
      else {
        Debug.LogWarning("lang " + lang +" not found: " + filename);
        filename = Application.dataPath + "/Editor/Azulejista/localized_texts/en-US.json";
        return JsonUtility.FromJson<TileMakerEditorLabels>(System.IO.File.ReadAllText(filename));
      }
    }
  }
}