using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

using Flux;

namespace FluxEditor
{
	[Serializable]
	public class FEventEditorInspector : FEditorInspector<FEventEditor,FEvent> {

		public override string Title {
			get {
				if( _editors.Count == 1 )
					return "事件:";
				return "事件列表:";
			}
		}

		protected override FMultiTypeInspector<FEvent> CreateMultiTypeInspector()
		{
			FMultiTypeInspector<FEvent> multiTypeInspector = FMultiEventInspector.CreateInstance<FMultiEventInspector>();
			multiTypeInspector.SetObjects( _objects.ToArray() );
			return multiTypeInspector;
		}
	}
}
