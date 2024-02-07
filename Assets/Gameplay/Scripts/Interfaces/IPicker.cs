using UnityEngine;

namespace Gameplay
{
    public interface IPicker<T> where T : IPickable
    {
        void PickObject(T pickObject);
        void DropObject();
    }
}