﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Framework.Editor
{
    public abstract class EditorView<TView, TPresenter> : EditorWindow 
        where TView : EditorView<TView, TPresenter>
        where TPresenter : EditorPresenter
    {
        protected static TView BaseInstance;
        protected TPresenter Presenter;

        protected Vector2 EditorSize = Vector2.zero;

        public static void FocusOrCreate()
        {
            FocusOrCreate<ScriptableObject>();
        }

        public static void FocusOrCreate<TArg>(TArg asset = null) where TArg : ScriptableObject
        {
            if (BaseInstance)
            {
                BaseInstance.Focus();
            }
            else
            {
                BaseInstance = GetWindow<TView>();

                if (BaseInstance == null)
                    throw new System.InvalidOperationException(string.Format("Unable to create instance of '{0}'!", typeof(TView)));

                BaseInstance.OnCreated();

                if (BaseInstance.Presenter == null)
                    throw new System.InvalidOperationException("Presenter was not instantiated in OnCreated!");
            }

            var assetEditor = BaseInstance as IAssetEditor<TArg>;
            if (assetEditor != null)
            {
                if (asset != null)
                {
                    assetEditor.OnLoadAsset(asset);
                }
                else
                {
                    assetEditor.ReloadAssetFromSelection();
                }
            }   
        }

        protected virtual void OnCreated()
        {
            
        }
        
        protected void OnFocus()
        {
            Presenter.OnFocus();
        }

        protected void OnProjectChange()
        {
            Presenter.OnProjectChange();
        }

        protected void OnSelectionChange()
        {
            Presenter.OnSelectionChange();
        }

        protected void OnGUI()
        {
            EditorSize = new Vector2(position.width, position.height);
            Presenter.OnDraw();
        }
    }
}

