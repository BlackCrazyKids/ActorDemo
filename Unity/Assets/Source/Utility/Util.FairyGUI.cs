using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using XLua;

namespace Game
{
    public partial class Util
    {
        /// <summary>
        /// FGUI url转换
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void InputIcon(FairyGUI.GTextInput input, string icon)
        {
            string content = string.Format("[img]{0}[/img]", icon);
            input.ReplaceSelection(content);
        }
        public static void ListItemRenderer(GList list, LuaFunction function)
        {
            list.itemRenderer = (index, obj) => function.Action(index, obj);
        }
        public static void ListItemProvider(GList list, LuaFunction function)
        {
            list.itemProvider = (index) => function.Func<int, string>(index);
        }
    }
}

