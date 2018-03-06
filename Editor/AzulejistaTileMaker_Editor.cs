using UnityEngine;
using UnityEditor;
using Azulejista;

namespace Azulejista {

  [CustomEditor(typeof (AzulejistaTileMaker))]
  public class AzulejistaTileMaker_Editor : Editor {
    TileMakerEditorLabels labels;
    int lang_selected_index;
    string[] languages_available;

    SerializedProperty
      map_parent_sprop,
      tile_selector_sprop,
      width_sprop,
      height_sprop,
      step_x_sprop,
      step_y_sprop,
      z_step_sprop,
      preview_color_sprop,
      preview_size_sprop;


    void OnEnable() {
      map_parent_sprop = serializedObject.FindProperty("mapParent");
      tile_selector_sprop = serializedObject.FindProperty("tileSelector");
      width_sprop = serializedObject.FindProperty("width");
      height_sprop = serializedObject.FindProperty("height");
      step_x_sprop = serializedObject.FindProperty("stepX");
      step_y_sprop = serializedObject.FindProperty("stepY");
      z_step_sprop = serializedObject.FindProperty("zStep");
      preview_color_sprop = serializedObject.FindProperty("previewColor");
      preview_size_sprop = serializedObject.FindProperty("previewSize");
      languages_available = AzulejistaLabels.GetAvailableLanguages();
    }

    void UpdateMapData() {
      var component =  target as AzulejistaTileMaker;
      var map_copy = component.map;

      component.map = new AzulejistaTileMaker.MapLine[component.height];
      
      for (int i = 0; i < component.map.Length; i++) {
        component.map[i].line = new bool[component.width];
        
        if (i < map_copy.Length) {
          var menor_tamanho =
            map_copy[i].line.Length > component.map[i].line.Length
              ? component.map[i].line.Length
              : map_copy[i].line.Length;

          for (int j = 0; j < menor_tamanho; j++)
            component.map[i].line[j] = map_copy[i].line[j];
        }
      }

      map_copy = component.map;
    }

    public override void OnInspectorGUI() {
      serializedObject.Update();
      
      var component = target as AzulejistaTileMaker;

      GUILayout.Label(labels.editor_only, EditorStyles.boldLabel);
      
      GUILayout.BeginHorizontal();
      GUILayout.Label(labels.language);
      lang_selected_index = EditorGUILayout.Popup(lang_selected_index, languages_available);
      GUILayout.EndHorizontal();

      if (component.editor_lang != languages_available[lang_selected_index])
        component.editor_lang = languages_available[lang_selected_index];

      if (labels.lang != component.editor_lang)
        labels = AzulejistaLabels.GetTileMakerEditorLabels(component.editor_lang);            

      EditorGUILayout.PropertyField(preview_color_sprop, new GUIContent(labels.preview_color));
      EditorGUILayout.PropertyField(preview_size_sprop, new GUIContent(labels.preview_size));

      GUILayout.Space(10);
      GUILayout.Label(labels.references, EditorStyles.boldLabel);
      EditorGUILayout.PropertyField(map_parent_sprop, new GUIContent(labels.map_parent));
      EditorGUILayout.PropertyField(tile_selector_sprop, new GUIContent(labels.tile_selector));

      GUILayout.Space(10);
      GUILayout.Label(labels.map_dimensions, EditorStyles.boldLabel);
      EditorGUILayout.PropertyField(width_sprop, new GUIContent(labels.width));
      EditorGUILayout.PropertyField(height_sprop, new GUIContent(labels.height));

      GUILayout.Space(10);
      GUILayout.Label(labels.step_by_tile, EditorStyles.boldLabel);
      EditorGUILayout.PropertyField(step_x_sprop, new GUIContent(labels.step_x));
      EditorGUILayout.PropertyField(step_y_sprop, new GUIContent(labels.step_y));
      EditorGUILayout.PropertyField(z_step_sprop, new GUIContent(labels.z_step));

      GUILayout.Space(15);
      

      GUILayout.BeginHorizontal();

      if (GUILayout.Button(labels.regenerate_map_data))
        UpdateMapData();


      // botão de pré visualização
      if (GUILayout.Button(labels.generate_map)) {
        component.DestroyMap();
        component.GenerateMap();
      }

      GUILayout.EndHorizontal();

      GUIStyle style = new GUIStyle();
      style.fixedWidth  = 15;
      style.fixedHeight = 15;      

      var i_map_line = 0;

      while (i_map_line < component.map.Length) {
        var i_map_column = 0;

        GUILayout.BeginHorizontal(style);

        if (i_map_line == 0)  {
          GUILayout.Label("#");
          for (int i_column = 0; i_column < component.map[i_map_line].line.Length; i_column++)
            GUILayout.Label(i_column.ToString(), GUILayout.Width(18));

          GUILayout.EndHorizontal();
          GUILayout.BeginHorizontal(style);
        }

        GUILayout.Label(i_map_line.ToString());
        while (i_map_column < component.map[i_map_line].line.Length) {
          component.map[i_map_line].line[i_map_column] = GUILayout.Toggle(component.map[i_map_line].line[i_map_column], "");
          i_map_column += 1;
        }
          
        GUILayout.EndHorizontal();


        i_map_line += 1;
      }

      serializedObject.ApplyModifiedProperties();
    }
  }
}