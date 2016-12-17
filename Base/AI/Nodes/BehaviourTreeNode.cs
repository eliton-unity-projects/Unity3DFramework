﻿using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.AI
{
    [System.Serializable]
    public abstract class BehaviourTreeNode : ScriptableObject
    {
        protected static readonly List<BehaviourTreeNode> EmptyList = new List<BehaviourTreeNode>();

        public abstract string Name { get; }
        public abstract string Description { get; }
        
        [HideInInspector]
        [SerializeField]
        public ParentNode Parent;

        [SerializeField]
        public Dictionary<string, GenericParameter> BlackboardRequired = new Dictionary<string, GenericParameter>();
        
        public AIController CurrentController { get; private set; }
        public Blackboard CurrentBlackboard { get; private set; }

        public NodeResult LastResult { get; private set; }
        
        public bool IsRootNode()
        {
            return Parent == null;
        }

        public virtual bool IsParentNode()
        {
            return false;
        }

        public ParentNode AsParentNode()
        {
#if UNITY_EDITOR
            if(!IsParentNode())
                throw new InvalidOperationException(string.Format("Cannot Node {0} is not a Parent Node!", GetType().Name));
#endif
            return (ParentNode) this;
        }

        public virtual void OnInit() { }

        public virtual void OnStart(AIController controller) { }

        public virtual void OnEnd(AIController controller) { }
        
        private void GetFromBlackboard()
        {
            foreach (var requirement in BlackboardRequired)
            {
                // CurrentBlackboard.SetToParameter(requirement.Value);
                // requirement.Value.SetParameter.Invoke(CurrentBlackboard.GetValue(parameter.Type, parameter.Name));
            }
        }

        private void SetToBlackboard()
        {
            foreach (var requirement in BlackboardRequired)
            {
                // CurrentBlackboard.GetFromParameter(requirement.Value);
                // Maybe blackboard set parameter? And throw exception?
                // CurrentBlackboard.SetValue(parameter.Value.Name);
            }
        }

        public NodeResult CallUpdate(AIController controller, Blackboard blackboard)
        {
            CurrentController = controller;
            CurrentBlackboard = blackboard;

            // update blackboard

            LastResult = OnUpdate();

            // update blackboard

            return LastResult;
        }

        protected abstract NodeResult OnUpdate();

        public static string GetNodeTypeName(System.Type type)
        {
            if (type.IsSubclassOf(typeof(CompositeNode)))
                return "Composite";
            if (type.IsSubclassOf(typeof(DecoratorNode)))
                return "Decorator";
            if (type.IsSubclassOf(typeof(TaskNode)))
                return "Task";

            return "Unknown";
        }

#if UNITY_EDITOR
        [HideInInspector] public Vector2 EditorPosition;
#endif
    }
}
