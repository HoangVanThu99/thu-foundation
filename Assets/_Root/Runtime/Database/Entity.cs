﻿using UnityEngine;

namespace Pancake.Database
{
    public abstract class Entity : ScriptableObject
    {
#if PANCAKE_RUNTIME_UNSAFE
        [System.Ulid]
#endif
        [SerializeField] private string id;
        [SerializeField] private string title = "Blank";
        [SerializeField, TextArea] private string description;

        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public string ID { get => id; set => id = value; }

        public Sprite GetIcon => GetIconInternal();

        protected virtual void Reset()
        {
#if PANCAKE_RUNTIME_UNSAFE
            Title = $"UNASSIGNED.{System.Ulid.NewUlid()}";
#else 
            Title = $"UNASSIGNED";
#endif
            Description = "";
        }

        /// <summary>
        /// Typically used in the Editor to display an icon for the Asset List. Can be used for other things at runtime if desired.
        /// </summary>
        protected virtual Sprite GetIconInternal() { return null; }
    }
}