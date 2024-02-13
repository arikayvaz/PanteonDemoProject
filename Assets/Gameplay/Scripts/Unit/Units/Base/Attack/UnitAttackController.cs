using Common;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class UnitAttackController : MonoBehaviour, IController
    {
        private IDamageable attackTarget = null;
        private int attackDamage = 0;
        private IEnumerator currentAttack = null;
        private UnityAction onTargetDied = null;
        WaitForSeconds waitForSeconds = null;

        public void InitController() 
        {
            ResetVariables();
        }

        public void StartAttacking(IDamageable attackTarget, int attackDamage, float attackDelay, UnityAction onTargetDied) 
        {
            this.attackTarget = attackTarget;
            this.attackDamage = attackDamage;
            this.onTargetDied = onTargetDied;

            waitForSeconds = new WaitForSeconds(attackDelay);

            currentAttack = AttackSequence();

            StartAttack(currentAttack);
        }

        private void StartAttack(IEnumerator attack) 
        {
            if (attack == null)
                return;

            StartCoroutine(attack);
        }

        private void StopAttack() 
        {
            if (currentAttack == null)
                return;

            StopCoroutine(currentAttack);
        }

        private IEnumerator AttackSequence() 
        {
            while (attackTarget.IsAlive())
            {
                attackTarget.GetDamage(attackDamage);

                if (attackTarget.IsAlive())
                    yield return waitForSeconds;

                OnTargetDied();
                yield break;
            }
        }

        private void OnTargetDied() 
        {
            StopAttack();

            onTargetDied?.Invoke();

            ResetVariables();
        }

        private void ResetVariables() 
        {
            attackTarget = null;
            attackDamage = 0;
            currentAttack = null;
            onTargetDied = null;
            waitForSeconds = null;
        }
    }
}