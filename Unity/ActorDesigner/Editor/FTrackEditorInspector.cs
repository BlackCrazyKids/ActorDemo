using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

using Flux;

namespace FluxEditor
{
	[Serializable]
	public class FTrackEditorInspector : FEditorInspector<FTrackEditor, FTrack> {

		public override string Title {
			get {
				if( _editors.Count == 1 )
					return "轨道:";
				return "轨道列表:";
			}
		}

		public FTrackEditorInspector()
		{
		}

		protected override FMultiTypeInspector<FTrack> CreateMultiTypeInspector()
		{
			FMultiTypeInspector<FTrack> multiTypeInspector = FMultiTrackInspector.CreateInstance<FMultiTrackInspector>();
			multiTypeInspector.SetObjects( _objects.ToArray() );
			return multiTypeInspector;
		}

		public override void OnInspectorGUI( float contentWidth )
		{
			base.OnInspectorGUI(contentWidth);

			if( _editors.Count == 1 && _editors[0].HasTools() )
			{
				GUILayout.Space( 10 );

				EditorGUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(contentWidth));
				EditorGUILayout.LabelField("轨道工具:", EditorStyles.boldLabel);
				_editors[0].OnToolsGUI();
				EditorGUILayout.EndVertical();

			}
		}

		public override void Refresh()
		{
			base.Refresh();

			if( _inspector != null && _inspector is FTrackInspector )
			{
				((FTrackInspector)_inspector).ShowEvents = false;
			}
		}
	}
}
