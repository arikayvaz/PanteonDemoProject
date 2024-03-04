using UnityEngine;

namespace Gameplay
{
    public interface IPicker
    {
        void PickObject(IPickable pickObject);
        void DropObject();
    }
}