using UnityEngine;
using System.Collections;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities.Editor;
using System.Text;

namespace AssetMgr
{
    [GlobalConfig("Editor/AssetMgr/Config", UseAsset = true)]
    public class TextureImpoterCfg : GlobalConfig<TextureImpoterCfg>
    {
        [FolderPath(AbsolutePath = false, RequireValidPath = true)]
        public string UITexture;
        [FolderPath(AbsolutePath = false, RequireValidPath = true)]
        public string SceneTexture;
        [FolderPath(AbsolutePath = false, RequireValidPath = true)]
        public string SfxTexture;
        [FolderPath(AbsolutePath = false, RequireValidPath = true)]
        public string __TestTexture;

        [ListDrawerSettings()]
        public List<TextureSetting> TextureSettings = new List<TextureSetting>();

        List<string> ValidRootPaths { get { return new List<string>() { UITexture, SceneTexture, SfxTexture, __TestTexture }; } }

        [Button("加载配置", ButtonSizes.Large)]
        /// <summary>
        /// 加载路径
        /// </summary>
        private void GUILoad()
        {
            Importer.Instance.RefreshTree();
        }
        [Button("导入所有", ButtonSizes.Large)]
        /// <summary>
        /// 重写导入所有
        /// </summary>
        private void GUIReimportAll()
        {
            ReimportAll();
        }
        [Button("删除所有配置", ButtonSizes.Large)]
        private void GUIClear()
        {
            TextureSettings.Clear();
            EditorUtility.UnloadUnusedAssetsImmediate();
            AssetDatabase.Refresh();
        }
        [Button("刷新配置", ButtonSizes.Large)]
        public void ClearValid()
        {
            for (int i = 0; i < TextureSettings.Count; i++)
                CheckValidPath(TextureSettings[i]);
            CheckValidRootPath();
            Importer.Instance.RefreshTree();
        }
        private void CheckValidPath(TextureSetting parent)
        {
            List<TextureSetting> children = parent.Children;
            for (int i = 0; i < children.Count; i++)
            {
                if (!Directory.Exists(children[i].Path))
                    parent.Children.Remove(children[i]);
                else
                    CheckValidPath(children[i]);
            }
        }
        public void Load()
        {
            CheckValidRootPath();
            GetDirSetting(UITexture);
            GetDirSetting(SceneTexture);
            GetDirSetting(SfxTexture);
            GetDirSetting(__TestTexture);
        }

        private void CheckValidRootPath()
        {
            var valids = ValidRootPaths;
            TextureSettings.RemoveAll(s =>
            {
                string relPath = AMTool.GetUnityPath(s.Path);
                return !valids.Contains(relPath);
            });
        }
        private void GetDirSetting(string relPath)
        {
            if (!string.IsNullOrEmpty(relPath))
            {
                string path = AMTool.GetAbsPath(relPath);
                if (!Directory.Exists(path)) return;
                Dictionary<string, TextureSetting> dict = new Dictionary<string, TextureSetting>();
                TextureSetting root = TextureSettings.Find(t => t.RelPath.Equals(relPath));
                if (root == null)
                {
                    var setting = new TextureSetting();
                    setting.RelPath = relPath;
                    dict.Add(path, setting);
                    TextureSettings.Add(dict[path]);
                }
                else
                    AMTool.GetCurrentSetting(root, dict);
                AMTool.LoadDirSetting(path, dict);
            }
        }
        public void ReimportAll()
        {
            for (int i = 0; i < TextureSettings.Count; i++)
                TextureSettings[i].DoHandle();

            EditorUtility.UnloadUnusedAssetsImmediate();
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }


        //-------特殊导入方案
        /// <summary>
        /// 检查当前设置是否适合包含的资源
        /// </summary>
        public void CheckSettingIllegel()
        {
            Dictionary<string, TextureSetting> dict = AMTool.GetDictionary(TextureSettings);
            float count = 0;
            StringBuilder nullBuilder = new StringBuilder();
            StringBuilder illegelBuilder = new StringBuilder();
            foreach (var item in dict)
            {
                EditorUtility.DisplayProgressBar("检查", item.Value.RelPath, count++ / dict.Count);
                if (item.Value.Children.Count > 0 || !item.Value.IsActive) continue;

                bool hasAlpha = item.Value.AndroidImporterFormat == TextureImporterFormat.ETC2_RGBA8
                    || item.Value.IOSImporterFormat == TextureImporterFormat.PVRTC_RGBA4;
                string[] fs = Directory.GetFiles(AMTool.GetAbsPath(item.Value.RelPath), "*.*", SearchOption.TopDirectoryOnly);
                foreach (var f in fs)
                {
                    if (Path.GetExtension(f).Equals(".meta")) continue;

                    string relPath = AMTool.GetUnityPath(f);
                    TextureImporter importer = AssetImporter.GetAtPath(relPath) as TextureImporter;
                    if (importer == null)
                    {
                        nullBuilder.AppendLine(relPath);
                        continue;
                    }
                    if (importer.DoesSourceTextureHaveAlpha() == hasAlpha) continue;

                    illegelBuilder.AppendLine(relPath);
                }
            }
            Debug.LogError(illegelBuilder.ToString());
            Debug.LogError("---------------TextureImporter-NULL----------------");
            Debug.LogError(nullBuilder.ToString());
            EditorUtility.ClearProgressBar();
        }
    }

    public class TextureImpoterView
    {
        [ShowInInspector, LabelText("UI图片目录")]
        [FolderPath(AbsolutePath = false, RequireValidPath = true)]
        public string UITexture
        {
            get { return TextureImpoterCfg.Instance.UITexture; }
            set { TextureImpoterCfg.Instance.UITexture = value; }
        }
        [ShowInInspector, LabelText("场景图片目录")]
        [FolderPath(AbsolutePath = false, RequireValidPath = true)]
        public string SceneTexture
        {
            get { return TextureImpoterCfg.Instance.SceneTexture; }
            set { TextureImpoterCfg.Instance.SceneTexture = value; }
        }
        [ShowInInspector, LabelText("特效图片目录")]
        [FolderPath(AbsolutePath = false, RequireValidPath = true)]
        public string SfxTexture
        {
            get { return TextureImpoterCfg.Instance.SfxTexture; }
            set { TextureImpoterCfg.Instance.SfxTexture = value; }
        }
        [ShowInInspector, LabelText("测试图片目录")]
        [FolderPath(AbsolutePath = false, RequireValidPath = true)]
        public string __TestTexture
        {
            get { return TextureImpoterCfg.Instance.__TestTexture; }
            set { TextureImpoterCfg.Instance.__TestTexture = value; }
        }

        [Button("刷新配置", ButtonSizes.Large)]
        public void ClearValid()
        {
            TextureImpoterCfg.Instance.ClearValid();
        }

        [Title("扩展功能")]
        [Button("检查配置合法性", ButtonSizes.Large)]
        public void CheckSettingIllegel()
        {
            TextureImpoterCfg.Instance.CheckSettingIllegel();
        }
    }
}