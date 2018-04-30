﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Editor;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace Framework.AI.Editor
{
    [InitializeOnLoad]
    public class BehaviourTreeEditorView : EditorView<BehaviourTreeEditorView, BehaviourTreeEditorPresenter>, IAssetEditor<BehaviourTree>
    {
        protected bool bShowParameters;
        private readonly GraphNodeEditor Nodes = new GraphNodeEditor();

        #region Creation

        [MenuItem("Gameplay/Behaviour Tree Editor")]
        public static void MenuShowEditor()
        {
            FocusOrCreate();
        }

        protected BehaviourTreeEditorView()
        {
            Presenter = new BehaviourTreeEditorPresenter(this);
            Nodes.OnRightClick.Reassign(data =>
            {
                Presenter.OnRightClick(data.MousePos);
                return true;
            });
        }

        void OnEnable()
        {
            name = " Behaviour   ";
            titleContent.image = SpaceEditorStyles.BehaviourTreeIcon;
            titleContent.text = name;

            Presenter.OnEnable();
        }
        
        internal void OnNodeAdded(BehaviourTree asset, BehaviourTreeNode node)
        {
            Nodes.AddNode(new BehaviourTreeEditorNode(asset, node, Presenter));
        }

        internal void RecreateNodes(ref BehaviourTreeEditorPresenter.Model model)
        {
            Nodes.ClearNodes();
            Nodes.ScrollPos = model.TreeAsset.EditorPos;

            foreach (var node in model.TreeAsset.Nodes)
            {
                Nodes.AddNode(new BehaviourTreeEditorNode(model.TreeAsset, node, Presenter));
            }
        }

        #endregion

        #region IAssetEditor

        public void OnLoadAsset(BehaviourTree asset)
        {
            Presenter.OnLoadAsset(asset);
        }

        public void ReloadAssetFromSelection()
        {
            Presenter.OnReloadAssetFromSelection();
        }

        #endregion

        #region GUI

        public Vector2 GetScrollPos()
        {
            return Nodes.ScrollPos;
        }

        internal void DrawWorkspace(ref BehaviourTreeEditorPresenter.Model model)
        {
            GUILayout.BeginVertical();
            {
                DrawToolbar(model.TreeAsset);

                GUILayout.BeginHorizontal();
                {
                    DrawNodeGraph();

                    if (bShowParameters)
                    {
                        EditorGUILayout.BeginVertical(GUILayout.Width(300));
                        {
                            DrawParameters();
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
                GUILayout.EndHorizontal();

                Nodes.HandleEvents(this);

                DrawFooter(ref model);
            }
            EditorGUILayout.EndVertical();

            if (Nodes.WantsRepaint)
                Repaint();
        }

        private void DrawNodeGraph()
        {
            GUILayout.BeginVertical(SpaceEditorStyles.GraphNodeEditorBackground);
            {
                // Reserve space for graph
                var targetRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                var adjustedRect = new Rect(0, 16 + 21, targetRect.width, position.height - 21 - 16);

                Nodes.Draw(this, adjustedRect);
            }
            GUILayout.EndVertical();
        }

        private void DrawToolbar(BehaviourTree asset)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
            {
                GUILayout.Label(asset.name);

                /*if (ExecuteInRuntime())
                {
                    GUI.color = Color.yellow;

                    EditorGUILayout.Separator();

                    GUILayout.Label("PLAYING : " + RuntimeController.name);

                    GUI.color = Color.white;

                    if (GUILayout.Button("select", EditorStyles.toolbarButton))
                    {
                        Selection.activeGameObject = RuntimeController.gameObject;
                    }
                }*/

                GUILayout.FlexibleSpace();

                bShowParameters = GUILayout.Toggle(bShowParameters, "Parameters", EditorStyles.toolbarButton);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawParameters()
        {
            
        }

        private void DrawFooter(ref BehaviourTreeEditorPresenter.Model model)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)); // GUI.skin.box, 
            {
                GUILayout.Label(model.AssetPath);
                GUILayout.FlexibleSpace();
                GUILayout.Label($"<<{(Nodes.CurrentMouseMode != null ? Nodes.CurrentMouseMode.GetType().Name : "null")}>>");
                //GUILayout.Label($"{Nodes.ScrollPos} :: {Event.current.mousePosition} :: ");
                //GUILayout.Label($"{Nodes.ZoomLevel * 100:##.##}%");
                Nodes.ZoomLevel = GUILayout.HorizontalSlider(Nodes.ZoomLevel, 0.25f, 1, GUILayout.Width(64));
            }
            EditorGUILayout.EndHorizontal();
        }

        internal void DrawCreationButton()
        {
            GUI.Label(new Rect(EditorSize.x * 0.5f - 175, EditorSize.y * 0.5f - 15, 350, 30), "Select Behaviour Tree in project tab to edit, or create new ");
            if (GUI.Button(new Rect(EditorSize.x * 0.5f - 50, EditorSize.y * 0.5f + 15, 100, 20), "Create"))
            {
                Presenter.OnCreateNewAsset();
            }
        }

        #endregion

        public void TryBeginConnection(BehaviourTreeEditorNode source, Vector2 position)
        {
            Nodes.StartConnection(source, position);
        }
    }
}