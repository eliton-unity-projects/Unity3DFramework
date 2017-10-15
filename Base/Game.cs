﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework
{
    public abstract class Game<TGame> : Singleton<TGame> where TGame : Game<TGame>
    {
        public bool InitOnStart;

        public SceneLoader Loader;

        public DebugConsole Console;

        public ControllSystem Controllers;

        public GameState StartState;

        public GameState CurrentState { get; protected set; }
        public GameState PreviousState { get; protected set; }

        public List<GameState> AllStates { get; protected set; }

        void Start()
        {
            if (!InitOnStart)
                return;

            Init();
        }

        private void RegisterConsoleCommands()
        {
            Console.RegisterCommand("exit", "closes console", (_) => 
            {
                Console.Close(); return true;
            });

            Console.RegisterCommand("quit", "quits game", (_) =>
            {
                QuitGame(); return true;
            });

            Console.RegisterCommand("restart", "restarts game", (_) =>
            {
                RestartGame(); return true;
            });
        }

        public void Init()
        {
            RegisterConsoleCommands();
            
            if (AllStates != null)
                AllStates.Clear();
            else
                AllStates = new List<GameState>();

            AllStates.AddRange(GetComponentsInChildren<GameState>());
            
            Loader.OnSceneLoad -= SceneLoaded;
            Loader.OnSceneLoad += SceneLoaded;

            Loader.StartLoadScene(Loader.BaseScene);
        }

        public abstract bool IsPlaying();

        protected virtual void OnSceneLoad()
        { }

        private void SceneLoaded()
        {
            SwitchState(StartState);
            OnSceneLoad();
            
            gameObject.BroadcastToAll("OnLevelLoaded");
        }

        public void SwitchState(GameState state)
        {
            if (state != CurrentState)
                PreviousState = CurrentState;

            if (CurrentState != null) CurrentState.DoEnd();
            CurrentState = state;
            if (CurrentState != null) CurrentState.DoStart();
        }

        public void SwitchState<TState>() where TState : GameState
        {
            SwitchState(AllStates.FirstOrDefault(s => s is TState));
        }

        public void QuitGame()
        {
            gameObject.BroadcastToAll("OnLevelCleanUp");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
        }

        public void RestartGame()
        {
            StopAllCoroutines();

            gameObject.BroadcastToAll("OnLevelCleanUp");

            Loader.StartLoadScene(Loader.CurrentScene);
        }

        public void DestroyPersistent()
        {
            foreach (var pers in FindObjectsOfType<Persistent>())
            {
                pers.DestroyOnExit();
            }
        }

        void Update()
        {
            if (!Loader.IsReady)
                return;

            if (CurrentState != null)
            {
                CurrentState.Tick();
            }

            Controllers.Tick();
        }

        void FixedUpdate()
        {
            if (!Loader.IsReady)
                return;

            if (CurrentState != null)
            {
                CurrentState.FixedTick();
            }

            Controllers.FixedTick();
        }

        void LateUpdate()
        {
            if (!Loader.IsReady)
                return;

            if (CurrentState != null)
            {
                CurrentState.LateTick();
            }
            
            Controllers.LateTick();
        }
    }
}
