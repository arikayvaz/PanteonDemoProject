using UnityEngine;

namespace Gameplay
{
    public interface ISelectable 
    {
        void Select();
        void Deselect();
        bool CanSelect();
        bool IsEqual(ISelectable selectable);
    }
}