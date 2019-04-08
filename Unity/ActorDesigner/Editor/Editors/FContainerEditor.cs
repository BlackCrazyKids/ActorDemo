using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

using Flux;

namespace FluxEditor
{
    [Serializable]
    public class FContainerEditor : FEditorList<FTrackEditor>
    {

        public const int CONTAINER_HEIGHT = 25;

        public FContainer Container { get { return (FContainer)Obj; } }

        private bool _isDragSelecting = false;
        private Vector2 _dragSelectingStartPos = Vector2.zero;

        private float _timelineHeaderWidth = 0;
        private float _pixelsPerFrame = 0;

        public void OnStop()
        {
            for (int i = 0; i != Editors.Count; ++i)
                Editors[i].OnStop();
        }

        public void UpdateTracks(int frame, float time)
        {
            for (int i = 0; i != Editors.Count; ++i)
            {
                if (!Editors[i].Track.enabled)
                    continue;
                Editors[i].UpdateEventsEditor(frame, time);
            }
        }

        public override void Init(FObject obj, FEditor owner)
        {
            base.Init(obj, owner);

            Editors.Clear();

            List<FTrack> tracks = Container.Tracks;

            for (int i = 0; i < tracks.Count; ++i)
            {
                FTrack track = tracks[i];
                FTrackEditor trackEditor = SequenceEditor.GetEditor<FTrackEditor>(track);
                trackEditor.Init(track, this);
                Editors.Add(trackEditor);
            }

            _icon = new GUIContent(FUtility.GetFluxTexture("Plus.png"));
        }

        public override FSequenceEditor SequenceEditor { get { return (FSequenceEditor)Owner; } }

        public override float HeaderHeight { get { return CONTAINER_HEIGHT; } }

        protected override string HeaderText { get { return Obj.name; } }
        protected override bool IconOnLeft { get { return false; } }

        protected override Color BackgroundColor { get { return Container.Color; } }

        protected override bool CanPaste(FObject obj)
        {
            // since Unity Objs can be "fake null"
            return obj != null && obj is FTrack;
        }

        protected override void Paste(object obj)
        {
            if (!CanPaste(obj as FObject))
                return;

            Undo.RecordObject(Container, string.Empty);

            FTrack track = Instantiate<FTrack>((FTrack)obj);
            track.hideFlags = Container.hideFlags;
            Container.Add(track);
            Undo.RegisterCreatedObjectUndo(track.gameObject, "Paste Timeline " + ((FTrack)obj).name);
        }

        protected override void Delete()
        {
            SequenceEditor.DestroyEditor(this);
        }

        protected override void OnHeaderInput(Rect labelRect, Rect iconRect)
        {
            if (Event.current.type == EventType.MouseDown && Event.current.clickCount > 1 && labelRect.Contains(Event.current.mousePosition))
            {
                Selection.activeTransform = Container.Owner;
                Event.current.Use();
            }
            base.OnHeaderInput(labelRect, iconRect);

            if (Event.current.type == EventType.MouseDown && iconRect.Contains(Event.current.mousePosition))
            {
                FSettings fSettings = FUtility.GetSettings();
                FContainerSetting setting = fSettings.ContainerType.Find(c => c._type == Container.ConatinerType);
                if (setting == null)
                    ShowAddTrackMenu();
                else
                    ShowAddTrackMenuBaseOnType(setting);
            }
        }

        private void ShowAddTrackMenuBaseOnType(FContainerSetting setting)
        {
            Event.current.Use();
            GenericMenu menu = new GenericMenu();
            
            System.Reflection.Assembly fluxAssembly = typeof(FEvent).Assembly;
            List<KeyValuePair<Type, FEventAttribute>> validTypeList = new List<KeyValuePair<Type, FEventAttribute>>();
            for (int i = 0; i < setting._list.Count; i++)
            {
                string t = setting._list[i];
                if (string.IsNullOrEmpty(t)) continue;
                Type type = fluxAssembly.GetType(t);
                if (type == null || !typeof(FEvent).IsAssignableFrom(type))
                {
                    Debug.LogErrorFormat("定义{0} 不是事件类型.", t);
                    continue;
                }

                object[] attributes = type.GetCustomAttributes(typeof(FEventAttribute), false);
                if (attributes.Length == 0 || ((FEventAttribute)attributes[0]).menu == null)
                    continue;

                validTypeList.Add(new KeyValuePair<Type, FEventAttribute>(type, (FEventAttribute)attributes[0]));
            }

            validTypeList.Sort(delegate (KeyValuePair<Type, FEventAttribute> x, KeyValuePair<Type, FEventAttribute> y)
            {
                return x.Value.menu.CompareTo(y.Value.menu);
            });

            foreach (KeyValuePair<Type, FEventAttribute> kvp in validTypeList)
            {
                menu.AddItem(new GUIContent(kvp.Value.menu), false, AddTrackMenu, kvp);
            }

            menu.ShowAsContext();

        }
        //--添加所有FTrack
        private void ShowAddTrackMenu()
        {
            Event.current.Use();

            GenericMenu menu = new GenericMenu();

            System.Reflection.Assembly fluxAssembly = typeof(FEvent).Assembly;

            Type[] types = typeof(FEvent).Assembly.GetTypes();

            if (fluxAssembly.GetName().Name != "Assembly-CSharp")
            {
                // if we are in the flux trial, basically allow to get the types in the project assembly
                ArrayUtility.AddRange<Type>(ref types, System.Reflection.Assembly.Load("Assembly-CSharp").GetTypes());
            }

            List<KeyValuePair<Type, FEventAttribute>> validTypeList = new List<KeyValuePair<Type, FEventAttribute>>();

            foreach (Type t in types)
            {
                if (!typeof(FEvent).IsAssignableFrom(t))
                    continue;

                object[] attributes = t.GetCustomAttributes(typeof(FEventAttribute), false);
                if (attributes.Length == 0 || ((FEventAttribute)attributes[0]).menu == null)
                    continue;

                validTypeList.Add(new KeyValuePair<Type, FEventAttribute>(t, (FEventAttribute)attributes[0]));
            }

            validTypeList.Sort(delegate (KeyValuePair<Type, FEventAttribute> x, KeyValuePair<Type, FEventAttribute> y)
            {
                return x.Value.menu.CompareTo(y.Value.menu);
            });

            foreach (KeyValuePair<Type, FEventAttribute> kvp in validTypeList)
            {
                menu.AddItem(new GUIContent(kvp.Value.menu), false, AddTrackMenu, kvp);
            }

            menu.ShowAsContext();
        }

        void AddTrackMenu(object param)
        {
            KeyValuePair<Type, FEventAttribute> kvp = (KeyValuePair<Type, FEventAttribute>)param;

            Undo.RecordObjects(new UnityEngine.Object[] { Container, this }, "add Track");

            FTrack track = (FTrack)typeof(FContainer).GetMethod("Add", new Type[] { typeof(FrameRange) }).MakeGenericMethod(kvp.Key).Invoke(Container, new object[] { SequenceEditor.ViewRange });

            //string evtName = track.gameObject.name;

            //int nameStart = 0;
            //int nameEnd = evtName.Length;
            //if (nameEnd > 2 && evtName[0] == 'F' && char.IsUpper(evtName[1]))
            //    nameStart = 1;
            //if (evtName.EndsWith("Event"))
            //    nameEnd = evtName.Length - "Event".Length;
            //evtName = evtName.Substring(nameStart, nameEnd - nameStart);

            string evtName = kvp.Value.menu;

            if (!string.IsNullOrEmpty(kvp.Value.menu))
            {
                string[] nodes = kvp.Value.menu.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                evtName = nodes[nodes.Length - 1];
            }

            track.gameObject.name = ObjectNames.NicifyVariableName(evtName);

            if (!Container.Sequence.IsStopped)
                track.Init();

            SequenceEditor.Refresh();

            Undo.RegisterCreatedObjectUndo(track.gameObject, string.Empty);

            SequenceEditor.SelectExclusive(SequenceEditor.GetEditor<FEventEditor>(track.GetEvent(0)));
        }

        public bool HasTrack(FTrackEditor trackEditor)
        {
            foreach (FTrackEditor t in Editors)
            {
                if (t == trackEditor)
                    return true;
            }

            return false;
        }

        private void StartDragSelecting(Vector2 mousePos)
        {
            _isDragSelecting = true;
            _dragSelectingStartPos = mousePos;
        }

        private void StopDragSelecting(Vector2 mousePos)
        {
            if (!_isDragSelecting)
                return;

            _isDragSelecting = false;

            FrameRange selectedRange = new FrameRange();
            bool isSelectingTimelines;

            Rect selectionRect = GetDragSelectionRect(_dragSelectingStartPos, mousePos, out selectedRange, out isSelectingTimelines);

            if (!Event.current.shift && !Event.current.control)
                SequenceEditor.DeselectAll();

            for (int i = 0; i != Editors.Count; ++i)
            {
                Rect trackRect = Editors[i].Rect;

                if (trackRect.yMin >= selectionRect.yMax)
                    break;

                if (trackRect.yMax <= selectionRect.yMin)
                    continue;

                if (Event.current.control)
                {
                    Editors[i].DeselectEvents(selectedRange);
                }
                else
                {
                    Editors[i].SelectEvents(selectedRange);
                }
            }
        }

        private void OnDragSelecting(Vector2 mousePos)
        {
            if (!_isDragSelecting)
                return;

            if (Event.current.shift)
            {
                EditorGUIUtility.AddCursorRect(Rect, MouseCursor.ArrowPlus);
            }
            else if (Event.current.control)
            {
                EditorGUIUtility.AddCursorRect(Rect, MouseCursor.ArrowMinus);
            }

            FrameRange selectedRange;
            bool isSelectingTracks;

            Rect selectionRect = GetDragSelectionRect(_dragSelectingStartPos, mousePos, out selectedRange, out isSelectingTracks);

            if (selectionRect.width == 0)
                selectionRect.width = 1;

            GUI.color = FGUI.GetSelectionColor();
            GUI.DrawTexture(selectionRect, EditorGUIUtility.whiteTexture);

            GUI.color = Color.white;
        }

        private Rect GetDragSelectionRect(Vector2 rawStartPos, Vector2 rawEndPos, out FrameRange selectedRange, out bool isSelectingTracks)
        {
            int startFrame = GetFrameForX(rawStartPos.x);
            int endFrame = GetFrameForX(rawEndPos.x);

            if (startFrame > endFrame)
            {
                int temp = startFrame;
                startFrame = endFrame;
                endFrame = temp;
            }

            selectedRange = new FrameRange(startFrame, endFrame);

            Rect rect = new Rect();

            Vector2 startPos = new Vector2(GetXForFrame(startFrame), rawStartPos.y);
            Vector2 endPos = new Vector2(GetXForFrame(endFrame), rawEndPos.y);

            bool isStartOnHeader;
            bool isEndOnHeader;

            FTrackEditor startTrack = GetTrackEditor(startPos, out isStartOnHeader);

            isSelectingTracks = isStartOnHeader;

            if (startTrack != null)
            {
                FTrackEditor endTrack = GetTrackEditor(endPos, out isEndOnHeader);
                if (endTrack == null)
                {
                    endTrack = startTrack;
                    isEndOnHeader = isStartOnHeader;
                }

                float xStart = Mathf.Min(startPos.x, endPos.x);
                float width = Mathf.Abs(startPos.x - endPos.x);
                float yStart;
                float height;


                if (startPos.y <= endPos.y)
                {
                    yStart = startTrack.Rect.yMin;
                    height = (isStartOnHeader ? endTrack.Rect.yMax : (isEndOnHeader ? endTrack.Rect.yMin + FTrackEditor.DEFAULT_TRACK_HEIGHT : endTrack.Rect.yMax)) - yStart;
                }
                else
                {
                    yStart = isStartOnHeader || isEndOnHeader ? endTrack.Rect.yMin : endTrack.Rect.yMin;
                    height = (isStartOnHeader ? startTrack.Rect.yMax : startTrack.Rect.yMax) - yStart;
                }

                rect.x = xStart;
                rect.y = yStart;
                rect.width = width;
                rect.height = height;
            }

            return rect;
        }

        public float GetXForFrame(int frame)
        {
            return _timelineHeaderWidth + frame * _pixelsPerFrame;
        }

        public int GetFrameForX(float x)
        {
            return Mathf.RoundToInt(((x - _timelineHeaderWidth) / _pixelsPerFrame));
        }

        private FTrackEditor GetTrackEditor(Vector2 pos, out bool isOnHeader)
        {
            for (int i = 0; i != Editors.Count; ++i)
            {
                if (Editors[i].Rect.Contains(pos))
                {
                    isOnHeader = Editors[i].Rect.yMin + FTrackEditor.DEFAULT_TRACK_HEIGHT > pos.y;

                    return Editors[i];
                }
            }

            isOnHeader = false;
            return null;
        }


    }
}
