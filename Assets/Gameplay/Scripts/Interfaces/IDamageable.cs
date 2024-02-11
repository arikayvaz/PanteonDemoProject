using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public interface IDamageable
    {
        int GetHealth();
        void SetHealth(int health);
        void AddHealth(int healthDelta);
        void GetDamage(int damage);
        void OnDied();
        BoardCoordinate GetCoordinate();
        BoardCoordinate GetAttackableCoordinate();
        virtual bool IsAlive() 
        {
            return GetHealth() >= 0;
        }
    }
}