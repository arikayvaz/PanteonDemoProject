using UnityEngine;

namespace Gameplay
{
    public interface IAttacker
    {
        public void Attack(BoardCoordinate attackCoordinate, IDamageable target) 
        {

        }
    }
}