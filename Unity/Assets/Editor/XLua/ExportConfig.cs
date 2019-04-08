/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using XLua;
using System;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Game;

public static class ExportConfig
{
    [CSharpCallLua]
    public static List<Type> LuaDelegates = new List<Type>()
    {
        typeof(Action),
        typeof(Action<float>),
        typeof(Action<float, float>),
        typeof(Func<int>),
    };

    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(Action),
        typeof(Action<int>),
        typeof(Action<bool>),
        typeof(Action<string>),
        typeof(Action<int, byte[]>),
        typeof(Action<NetworkChannel>),
        typeof(Func<int, int>),
        typeof(Predicate<int>),
        typeof(Comparison<int>),


        typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(UObjectExtention),

        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Quaternion),
        typeof(Color),
        typeof(Ray),
        typeof(Bounds),
        typeof(Ray2D),
        typeof(Mathf),
        typeof(Time),
        typeof(Rect),

        typeof(GL),
        typeof(Input),
        typeof(Screen),
        typeof(Physics),
        typeof(Graphics),
        typeof(Resources),
        typeof(Application),
        typeof(SleepTimeout),

        typeof(Collider),
        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CharacterController),
        typeof(CapsuleCollider),

        typeof(WWW),
        typeof(Behaviour),
        typeof(MonoBehaviour),
        typeof(GameObject),
        typeof(Texture),
        typeof(Texture2D),
        typeof(AudioClip),
        typeof(AssetBundle),
        typeof(AsyncOperation),

        typeof(Animator),
        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),

        typeof(Shader),
        typeof(Renderer),
        typeof(MeshRenderer),
        typeof(TrackedReference),
        typeof(SkinnedMeshRenderer),
        typeof(ParticleSystem),
        typeof(RenderSettings),
        typeof(RenderTexture),

        typeof(LightType),
        typeof(CameraClearFlags),
        typeof(KeyCode),
        typeof(Space),
        typeof(AnimationBlendMode),
        typeof(QueueMode),
        typeof(PlayMode),
        typeof(WrapMode),
        typeof(BlendWeights),
        typeof(RuntimePlatform),

        typeof(RectTransform),
        typeof(Text),
        typeof(Image),
        typeof(Button),
        typeof(RawImage),

        typeof(UnityEngine.Debug),

        typeof(EventTrigger),
        typeof(BaseEventData),
        typeof(AxisEventData),
        typeof(PointerEventData),
        typeof(EventTriggerType),
        typeof(UnityEngine.Events.UnityAction),


        typeof(Util),
        typeof(ConstSetting),
        typeof(LuaHelper),
        typeof(GameManager),
        typeof(LuaManager),
        typeof(ResourceManager),
        typeof(ResourceLoadType),
        typeof(SoundManager),
        typeof(NetworkManager),
        typeof(NetworkChannel),
        typeof(SceneManager),
        typeof(Game.Platform.CustomInterface),
        typeof(Game.Platform.Interface),

    };

    //自动把LuaCallCSharp涉及到的delegate加到CSharpCallLua列表，后续可以直接用lua函数做callback
    [CSharpCallLua]
    public static List<Type> CSharpCallLua
    {
        get
        {
            var lua_call_csharp = LuaCallCSharp;
            var delegate_types = new List<Type>();
            var flag = BindingFlags.Public | BindingFlags.Instance
                | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly;
            foreach (var field in (from type in lua_call_csharp select type).SelectMany(type => type.GetFields(flag)))
            {
                if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                {
                    delegate_types.Add(field.FieldType);
                }
            }

            foreach (var method in (from type in lua_call_csharp select type).SelectMany(type => type.GetMethods(flag)))
            {
                if (typeof(Delegate).IsAssignableFrom(method.ReturnType))
                {
                    delegate_types.Add(method.ReturnType);
                }
                foreach (var param in method.GetParameters())
                {
                    var paramType = param.ParameterType.IsByRef ? param.ParameterType.GetElementType() : param.ParameterType;
                    if (typeof(Delegate).IsAssignableFrom(paramType))
                    {
                        delegate_types.Add(paramType);
                    }
                }
            }
            return delegate_types.Distinct().ToList();
        }
    }

    //黑名单
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()  {
                new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
                new List<string>(){"UnityEngine.WWW", "movie"},
                new List<string>(){"UnityEngine.Texture", "imageContentsHash"},
                new List<string>(){ "UnityEngine.UI.Text", "OnRebuildRequested"},
                new List<string>(){"UnityEngine.Input", "IsJoystickPreconfigured", "System.String"},
    #if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
                new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
                new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
                new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
                new List<string>(){"UnityEngine.Light", "areaSize"},
                new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
                new List<string>(){"UnityEngine.WWW", "MovieTexture"},
                new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
                new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
    #if !UNITY_WEBPLAYER
                new List<string>(){"UnityEngine.Application", "ExternalEval"},
    #endif
                new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
                new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
                new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
            };
}
