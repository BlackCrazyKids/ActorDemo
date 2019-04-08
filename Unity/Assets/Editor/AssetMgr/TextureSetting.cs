using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;

namespace AssetMgr
{

    [Serializable]
    public class TextureSetting : DirectorySetting<TextureSetting>
    {
        readonly int[] MAX_SIZE = { 32, 64, 128, 256, 512, 1024, 2048, 4096 };
        const string DEFAULT = "Default";
        const string STANDALONE = "Standalone";
        const string ANDROID = "Android";
        const string IPHONE = "iPhone";


        [HideInInspector, SerializeField]
        public bool NeedSave2Children = false;
        [HideInInspector, SerializeField]
        public bool IsDirty = false;
        [HideInInspector, SerializeField]
        public int CompressionQuality = 100;
        /// <summary>
        /// [反射]配置有修改时调用
        /// </summary>
        private void OnValueChanged()
        {
            IsDirty = true;
            NeedSave2Children = true;
        }
        public void SaveCfg2Children()
        {
            //CheckOverrideFieldState();
            if (!NeedSave2Children) return;

            NeedSave2Children = false;
            SaveCfgRecursive();
        }

        private void SaveCfgRecursive()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                if (this.GetHashCode() != child.Parent.GetHashCode())
                    Debug.LogFormat("父对象不一致!\n{0}\n{1}", Name, child.Parent.Name);
                if (child.IsActive && !child.IsOverride)
                {
                    CopyA2B(this, Children[i]);
                    Children[i].SaveCfgRecursive();
                }
            }
        }


        [ShowInInspector, ReadOnly, LabelText("路径"), PropertyOrder(-1)]
        public string Path { get { return AMTool.GetAbsPath(RelPath); } }
        [BoxGroup("导入设置"), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public TextureImporterType TextureType = TextureImporterType.Default;
        [BoxGroup("导入设置"), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public TextureImporterNPOTScale TextureNPOT = TextureImporterNPOTScale.ToNearest;
        [BoxGroup("导入设置"), LabelText("Read/Write Enabled"), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public bool IsReadable = false;
        [BoxGroup("导入设置"), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public bool IsTransparency = false;
        [BoxGroup("导入设置"), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public bool GenerateMipmap = false;
        [BoxGroup("导入设置"), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public TextureWrapMode WrapMode = TextureWrapMode.Clamp;
        [BoxGroup("导入设置"), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public FilterMode Filter = FilterMode.Bilinear;
        [BoxGroup("导入设置"), Range(0, 16), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public int AnisoLevel = 1;


        //---Window
        [TabGroup("导入设置/Platform", STANDALONE), ValueDropdown("MAX_SIZE"), LabelText("MaxSize")]
        [EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public int WindowMaxSize = 1024;
        [TabGroup("导入设置/Platform", STANDALONE), LabelText("Format"), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public TextureImporterFormat WindowImporterFormat = TextureImporterFormat.RGBA32;

        //---Android
        [TabGroup("导入设置/Platform", ANDROID), ValueDropdown("MAX_SIZE"), LabelText("MaxSize")]
        [EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public int AndroidMaxSize = 1024;
        [TabGroup("导入设置/Platform", ANDROID), LabelText("Format"), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public TextureImporterFormat AndroidImporterFormat = TextureImporterFormat.RGBA32;

        //---IOS
        [TabGroup("导入设置/Platform", IPHONE), ValueDropdown("MAX_SIZE"), LabelText("MaxSize")]
        [EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public int IOSMaxSize = 1024;
        [TabGroup("导入设置/Platform", IPHONE), LabelText("Format"), EnableIf("IsOverride"), OnValueChanged("OnValueChanged")]
        public TextureImporterFormat IOSImporterFormat = TextureImporterFormat.RGBA32;

        [HorizontalGroup("Option")]
        [ButtonGroup("Option/Left")]
        [Button("复制", ButtonSizes.Large), EnableIf("IsOverride")]
        public void Copy()
        {
            Clipboard.Copy(this);
        }
        [ButtonGroup("Option/Right")]
        [Button("粘贴", ButtonSizes.Large), EnableIf("IsOverride")]
        public void Paste()
        {
            TextureSetting setting;
            if (Clipboard.TryPaste(out setting) && setting == null)
                return;

            CopyA2B(setting, this);
        }
        private void CopyA2B(TextureSetting from, TextureSetting to)
        {
            to.TextureType = from.TextureType;
            to.TextureNPOT = from.TextureNPOT;
            to.IsReadable = from.IsReadable;
            to.IsTransparency = from.IsTransparency;
            to.GenerateMipmap = from.GenerateMipmap;
            to.WrapMode = from.WrapMode;
            to.Filter = from.Filter;
            to.AnisoLevel = from.AnisoLevel;

            to.WindowMaxSize = from.WindowMaxSize;
            to.WindowImporterFormat = from.WindowImporterFormat;

            to.AndroidMaxSize = from.AndroidMaxSize;
            to.AndroidImporterFormat = from.AndroidImporterFormat;

            to.IOSMaxSize = from.IOSMaxSize;
            to.IOSImporterFormat = from.IOSImporterFormat;
        }

        /// <summary>
        /// 按照配置重新导入资源
        /// </summary>
        public override void DoHandle()
        {
            base.DoHandle();

            if (!IsActive) return;

            List<string> files = AMTool.GetTextures(Path);
            TextureSetting parent = Parent as TextureSetting;
            if (Parent == null || Parent.IsNull)
                IsOverride &= true;
            for (int i = 0; i < files.Count; i++)
            {
                string refPath = AMTool.GetUnityPath(files[i]);
                TextureImporter importer = AssetImporter.GetAtPath(refPath) as TextureImporter;
                //importer.ClearPlatformTextureSettings(STANDALONE);
                //importer.ClearPlatformTextureSettings(ANDROID);
                //importer.ClearPlatformTextureSettings(IPHONE);
                TextureImporterSettings importerSetting = new TextureImporterSettings();
                importer.ReadTextureSettings(importerSetting);

                importer.textureType = TextureType;
                importer.npotScale = TextureNPOT;
                importer.isReadable = IsReadable;
                importer.alphaIsTransparency = IsTransparency;
                importer.mipmapEnabled = GenerateMipmap;
                importer.wrapMode = WrapMode;
                importer.filterMode = Filter;
                importer.anisoLevel = AnisoLevel;

                importer.SetPlatformTextureSettings(STANDALONE, WindowMaxSize, WindowImporterFormat, CompressionQuality, false);
                importer.SetPlatformTextureSettings(ANDROID, AndroidMaxSize, AndroidImporterFormat, CompressionQuality, false);
                importer.SetPlatformTextureSettings(IPHONE, IOSMaxSize, IOSImporterFormat, CompressionQuality, false);

                importer.SaveAndReimport();
                EditorUtility.DisplayProgressBar("按配置导入纹理", refPath, (i + 1f) / files.Count);
            }
            foreach (var item in Children)
                item.DoHandle();
        }
    }
}