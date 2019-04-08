using UnityEngine;
using UnityEditor;
using Flux;

namespace FluxEditor
{

	[CustomEditor(typeof(Flux.FPlayAudioEvent), true)]
	public class FPlayAudioEventInspector : FEventInspector {

		private FPlayAudioEvent _audioEvt = null;

		private SerializedProperty _audioClip = null;
		private SerializedProperty _volume = null;
		private SerializedProperty _loop = null;
		private SerializedProperty _speedDeterminesPitch = null;
		private SerializedProperty _startOffset = null;

        private GUIContent _audioClipUI;
        private GUIContent _volumeUI;
        private GUIContent _loopUI;
        private GUIContent _speedDeterminesPitchUI;
        private GUIContent _startOffsetUI;

		protected override void OnEnable ()
		{
			base.OnEnable();

			_audioEvt = (FPlayAudioEvent)target;

            _audioClip = serializedObject.FindProperty("_audioClip");
            _volume = serializedObject.FindProperty("_volume");
            _loop = serializedObject.FindProperty("_loop");
            _speedDeterminesPitch = serializedObject.FindProperty("_speedDeterminesPitch");
			_startOffset = serializedObject.FindProperty("_startOffset");

            _audioClipUI = new GUIContent("音频");
            _volumeUI = new GUIContent("音量");
            _loopUI = new GUIContent("是否循环");
            _speedDeterminesPitchUI = new GUIContent("速度决定音高");
            _startOffsetUI = new GUIContent("起点偏移");
            
        }

		public override void OnInspectorGUI ()
		{
			AudioClip currentClip = _audioEvt.AudioClip;
			base.OnInspectorGUI();

            if ( currentClip != _audioEvt.AudioClip && _audioEvt.AudioClip != null && !_audioEvt.Loop )
			{
				if( _audioEvt.LengthTime > _audioEvt.AudioClip.length )
				{
					Undo.RecordObject( _audioEvt, null );
					_audioEvt.Length = Mathf.RoundToInt( _audioEvt.AudioClip.length * _audioEvt.Sequence.FrameRate );
					FSequenceEditorWindow.RefreshIfOpen();
				}
			}

			serializedObject.Update();

            EditorGUILayout.PropertyField(_audioClip, _audioClipUI);
            EditorGUILayout.PropertyField(_volume, _volumeUI);
            EditorGUILayout.PropertyField(_loop, _loopUI);
            EditorGUILayout.PropertyField(_speedDeterminesPitch, _speedDeterminesPitchUI);

            _startOffset.intValue = Mathf.Clamp( _startOffset.intValue, 0, _audioEvt.GetMaxStartOffset() );
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.IntSlider( _startOffset, 0, _audioEvt.GetMaxStartOffset() , _startOffsetUI);
			if( EditorGUI.EndChangeCheck() )
			{
				if( FSequenceEditorWindow.instance != null )
					FSequenceEditorWindow.instance.Repaint();
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}
