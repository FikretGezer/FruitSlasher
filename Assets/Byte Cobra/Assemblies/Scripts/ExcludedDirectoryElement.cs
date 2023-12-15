using System;
using UnityEngine.UIElements;

namespace ByteCobra.Assemblies
{
    public class ExcludedDirectoryElement
    {
        public Action<string> OnEditCompplete;
        public TextField TextField { get; protected set; }
        public Button DeleteButton { get; protected set; }
        public VisualElement Root { get; protected set; }

        public ExcludedDirectoryElement(VisualTreeAsset tree)
        {
            var clone = tree.Instantiate();
            Root = clone.Q<VisualElement>("root-element");
            DeleteButton = clone.Query<Button>("delete-button");
            TextField = clone.Query<TextField>();
        }
    }
}