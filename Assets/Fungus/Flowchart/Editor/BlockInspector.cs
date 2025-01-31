using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fungus
{
	/**
	 * Temp hidden object which lets us use the entire inspector window to inspect
	 * the block command list.
	 */
	public class BlockInspector : ScriptableObject 
	{
		[FormerlySerializedAs("sequence")]
		public Block block;
	}

	/**
	 * Custom editor for the temp hidden object.
	 */
	[CustomEditor (typeof(BlockInspector), true)]
	public class BlockInspectorEditor : Editor
	{
		protected Vector2 blockScrollPos;
		protected Vector2 commandScrollPos;
		protected bool resize = false;
		protected bool clamp = false;
		protected float topPanelHeight = 50;

		// Cache the block and command editors so we only create and destroy them
		// when a different block / command is selected.
		protected BlockEditor activeBlockEditor;
		protected CommandEditor activeCommandEditor;
		protected Command activeCommand; // Command currently being inspected

		// Cached command editors to avoid creating / destroying editors more than necessary
		// This list is static so persists between 
		protected static List<CommandEditor> cachedCommandEditors = new List<CommandEditor>();

		protected void OnDestroy()
		{
			ClearEditors();
		}

		protected void OnEnable()
		{
			ClearEditors();
		}

		protected void OnDisable()
		{
			ClearEditors();
		}

		protected void ClearEditors()
		{
			foreach (CommandEditor commandEditor in cachedCommandEditors)
			{
				DestroyImmediate(commandEditor);
			}

			cachedCommandEditors.Clear();
			activeCommandEditor = null;
		}

		public override void OnInspectorGUI () 
		{
			BlockInspector blockInspector = target as BlockInspector;
			Block block = blockInspector.block;

			if (block == null)
			{
				return;
			}

			Flowchart flowchart = block.GetFlowchart();

			if (activeBlockEditor == null ||
			    block != activeBlockEditor.target)
			{
				DestroyImmediate(activeBlockEditor);
				activeBlockEditor = Editor.CreateEditor(block) as BlockEditor;
			}

			activeBlockEditor.DrawBlockName(flowchart);

			// Using a custom rect area to get the correct 5px indent for the scroll views
			Rect blockRect = new Rect(5, topPanelHeight, Screen.width - 6, Screen.height - 70);
			GUILayout.BeginArea(blockRect);

			blockScrollPos = GUILayout.BeginScrollView(blockScrollPos, GUILayout.Height(flowchart.blockViewHeight));
			activeBlockEditor.DrawBlockGUI(flowchart);
			GUILayout.EndScrollView();

			Command inspectCommand = null;
			if (flowchart.selectedCommands.Count == 1)
			{
				inspectCommand = flowchart.selectedCommands[0];
			}

			if (Application.isPlaying &&
			    inspectCommand != null &&
			    inspectCommand.parentBlock != block)
			{
				GUILayout.EndArea();
				Repaint();
				return;
			}

			// Only change the activeCommand at the start of the GUI call sequence
			if (Event.current.type == EventType.Layout)
			{
				activeCommand = inspectCommand;
			}

			DrawCommandUI(flowchart, inspectCommand);
		}

		public void DrawCommandUI(Flowchart flowchart, Command inspectCommand)
		{
			ResizeScrollView(flowchart);

			GUILayout.Space(7);

			activeBlockEditor.DrawButtonToolbar();

			commandScrollPos = GUILayout.BeginScrollView(commandScrollPos);

			if (inspectCommand != null)
			{
				if (activeCommandEditor == null || 
				    inspectCommand != activeCommandEditor.target)
				{
					// See if we have a cached version of the command editor already,
					var editors = (from e in cachedCommandEditors where (e != null && e.target == inspectCommand) select e);

					if (editors.Count() > 0)
					{
						// Use cached editor
						activeCommandEditor = editors.First();
					}
					else
					{
						// No cached editor, so create a new one.
						activeCommandEditor = Editor.CreateEditor(inspectCommand) as CommandEditor;
						cachedCommandEditors.Add(activeCommandEditor);
					}
				}
				if (activeCommandEditor != null)
				{
					activeCommandEditor.DrawCommandInspectorGUI();
				}
			}

			GUILayout.EndScrollView();

			GUILayout.EndArea();

			// Draw the resize bar after everything else has finished drawing
			// This is mainly to avoid incorrect indenting.
			Rect resizeRect = new Rect(0, topPanelHeight + flowchart.blockViewHeight + 1, Screen.width, 4f);
			GUI.color = new Color(0.64f, 0.64f, 0.64f);
			GUI.DrawTexture(resizeRect, EditorGUIUtility.whiteTexture);
			resizeRect.height = 1;
			GUI.color = new Color32(132, 132, 132, 255);
			GUI.DrawTexture(resizeRect, EditorGUIUtility.whiteTexture);
			resizeRect.y += 3;
			GUI.DrawTexture(resizeRect, EditorGUIUtility.whiteTexture);
			GUI.color = Color.white;

			Repaint();
		}

		private void ResizeScrollView(Flowchart flowchart)
		{
			Rect cursorChangeRect = new Rect(0, flowchart.blockViewHeight + 1, Screen.width, 4f);

			EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.ResizeVertical);
			
			if (Event.current.type == EventType.MouseDown && cursorChangeRect.Contains(Event.current.mousePosition))
			{
				resize = true;
			}

			if (resize)
			{
				Undo.RecordObject(flowchart, "Resize view");
				flowchart.blockViewHeight = Event.current.mousePosition.y;
			}
			
			ClampBlockViewHeight(flowchart);
			
			// Stop resizing if mouse is outside inspector window.
			// This isn't standard Unity UI behavior but it is robust and safe.
			if (resize && Event.current.type == EventType.MouseDrag)
			{
				Rect windowRect = new Rect(0, 0, Screen.width, Screen.height);
				if (!windowRect.Contains(Event.current.mousePosition))
				{
					resize = false;
				}
			}

			if (Event.current.type == EventType.MouseUp)
			{
				resize = false;
			}
		}
		
		protected virtual void ClampBlockViewHeight(Flowchart flowchart)
		{
			// Screen.height seems to temporarily reset to 480 for a single frame whenever a command like 
			// Copy, Paste, etc. happens. Only clamp the block view height when one of these operations is not occuring.

			if (Event.current.commandName != "")
			{
				clamp = false;
			}
			
			if (clamp)
			{
				// Make sure block view is always clamped to visible area
				float height = flowchart.blockViewHeight;
				height = Mathf.Max(200, height);
				height = Mathf.Min(Screen.height - 200,height);
				flowchart.blockViewHeight = height;
			}
			
			if (Event.current.type == EventType.Repaint)
			{
				clamp = true;
			}
		}
	}

}
