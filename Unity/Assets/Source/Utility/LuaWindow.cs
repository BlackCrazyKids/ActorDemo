using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Game
{
    public class LuaWindow : Window
    {
        static int _maxNum = 7;
        static Stack<LuaWindow> _pool = new Stack<LuaWindow>();

        public static LuaWindow Get()
        {
            if (_pool.Count > 0)
                return _pool.Pop();
            else
                return new LuaWindow();
        }
        public static void Release(LuaWindow window)
        {
            if (window != null && _pool.Count <= _maxNum)
                _pool.Push(window);
            else
                window.Dispose();
        }
        public static void Destroy()
        {
            int length = _pool.Count;
            for (int i = 0; i < length; i++)
                _pool.Pop().Dispose();
        }


        LuaTable _table;
        LuaFunction _OnInit;
        LuaFunction _DoHideAnimation;
        LuaFunction _DoShowAnimation;
        LuaFunction _OnShown;
        LuaFunction _OnHide;

        private LuaWindow() { }

        public void ConnectLua(LuaTable table)
        {
            _table = table;
            _OnInit = table.Get<LuaFunction>("OnInit");
            _DoHideAnimation = table.Get<LuaFunction>("DoShow");
            _DoShowAnimation = table.Get<LuaFunction>("DoHide");
            _OnShown = table.Get<LuaFunction>("OnShow");
            _OnHide = table.Get<LuaFunction>("OnHide");
        }
        /// <summary>
        /// 仅在显示动画完成时调用
        /// </summary>
        public void ShowImmediately()
        {
            OnShown();
        }
        public override void Dispose()
        {
            if (_table != null)
                _table.Dispose();
            if (_OnInit != null)
                _OnInit.Dispose();
            if (_DoHideAnimation != null)
                _DoHideAnimation.Dispose();
            if (_DoShowAnimation != null)
                _DoShowAnimation.Dispose();
            if (_OnShown != null)
                _OnShown.Dispose();
            if (_OnHide != null)
                _OnHide.Dispose();
            base.Dispose();
            Release(this);
        }

        protected override void OnInit()
        {
            if (_OnInit != null)
                _OnInit.Action(_table);
        }

        protected override void DoHideAnimation()
        {
            if (_DoHideAnimation != null)
                _DoHideAnimation.Action(_table);
            else
                base.DoHideAnimation();
        }

        protected override void DoShowAnimation()
        {
            if (_DoShowAnimation != null)
                _DoShowAnimation.Action(_table);
            else
                base.DoShowAnimation();
        }

        protected override void OnShown()
        {
            base.OnShown();

            if (_OnShown != null)
                _OnShown.Action(_table);
        }

        protected override void OnHide()
        {
            base.OnHide();

            if (_OnHide != null)
                _OnHide.Action(_table);
        }
    }
}