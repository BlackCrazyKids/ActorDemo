using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace FluxEditor
{
    public class FSettings : ScriptableObject
    {

        [MenuItem(FSequenceEditorWindow.MENU_PATH + FSequenceEditorWindow.PRODUCT_NAME + "/Create Flux Settings", false, 101)]
        public static void CreateColorSettings()
        {
            string settingsPath = FluxEditor.FUtility.GetFluxEditorPath() + "FluxSettings.asset";

            if (AssetDatabase.LoadMainAssetAtPath(settingsPath) != null)
            {
                if (!EditorUtility.DisplayDialog("Warning", "Flux Settings already exist, are you sure you want to replace them?", "Replace", "Cancel"))
                    return;
            }

            FSettings settings = CreateInstance<FSettings>();
            AssetDatabase.CreateAsset(settings, settingsPath);
        }

        [SerializeField]
        private List<FColorSetting> _eventColors = new List<FColorSetting>();
        public List<FColorSetting> EventColors { get { return _eventColors; } }

        private Dictionary<string, FColorSetting> _eventColorsHash = null;

        [SerializeField]
        private List<FColorSetting> _defaultContainers = new List<FColorSetting>();
        public List<FColorSetting> DefaultContainers { get { return _defaultContainers; } }

        [SerializeField]
        private List<FContainerSetting> _containerType = new List<FContainerSetting>();
        public List<FContainerSetting> ContainerType { get { return _containerType; } }

        public void Init()
        {
            if (_eventColorsHash == null)
                _eventColorsHash = new Dictionary<string, FColorSetting>();
            else
                _eventColorsHash.Clear();

            foreach (FColorSetting colorSetting in _eventColors)
            {
                if (string.IsNullOrEmpty(colorSetting._str))
                    return;

                if (_eventColorsHash.ContainsKey(colorSetting._str))
                    return; // can't add duplicates!

                _eventColorsHash.Add(colorSetting._str, colorSetting);
            }


            Type containerType = typeof(Flux.FContainerEnum);
            var fields = Enum.GetValues(containerType);
            List<FContainerSetting> cset = new List<FContainerSetting>(ContainerType);
            ContainerType.Clear();
            for (int i = 0; i < fields.Length; i++)
            {
                Flux.FContainerEnum e = (Flux.FContainerEnum)Enum.Parse(containerType, fields.GetValue(i).ToString());
                if (e == Flux.FContainerEnum.FContainer) continue;

                string title = "-";
                foreach (var pair in Flux.FContainer.ContainerMap)
                {
                    if (pair.Value == e)
                    {
                        title = pair.Key;
                        break;
                    }
                }
                if (title.Equals("-")) continue;
                 
                int index = cset.FindIndex(m => m._type == e);
                if (index < 0)
                {
                    FContainerSetting container = new FContainerSetting(e, title, new List<string>());
                    ContainerType.Add(container);
                }
                else
                {
                    cset[index]._name = title;
                    ContainerType.Add(cset[index]);
                }
            }
        }

        public Color GetEventColor(string str)
        {
            if (_eventColorsHash == null)
                Init();
            //			Debug.Log ( eventTypeStr );
            FColorSetting c;
            if (!_eventColorsHash.TryGetValue(str, out c))
                return FGUI.GetEventColor();
            return c._color;
        }
    }

    [System.Serializable]
    public class FColorSetting
    {
        public string _str;
        public Color _color;

        public FColorSetting(string str, Color color)
        {
            _str = str;
            _color = color;
        }
    }
    [System.Serializable]
    public class FContainerSetting
    {
        public Flux.FContainerEnum _type;
        public string _name;
        public List<string> _list;

        public FContainerSetting(Flux.FContainerEnum type, string name, List<string> list)
        {
            _type = type;
            _name = name;
            _list = list;
        }
    }
}
