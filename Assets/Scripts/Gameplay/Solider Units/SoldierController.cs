using System;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Buildings;
using GridSystem;
using UnityEngine;
using Utilities.Pooling;

namespace Gameplay.SoldierUnits
{
    public class SoldierController : HealthEntity, ISoliderUnit, IPoolable
    {
        private ISoliderUnitData data;
        private Sequence movementSequence;

        public float Damage => data.Damage;
        public Action OnMovementComplete { get; set; }

        public ISoliderUnitData Data => data;

        public void Initialize(ISoliderUnitData soldierData)
        {
            data = soldierData;
            InitializeMaxHP(data.MaxHealth);
        }

        public void MoveAlongPath(List<GridTile> path)
        {
            if (movementSequence != null && movementSequence.IsPlaying())
                movementSequence.Kill();

            movementSequence = DOTween.Sequence();

            foreach (var tile in path)
            {
                Vector3 targetPos = tile.transform.position;
                targetPos.y = transform.position.y;

                movementSequence.AppendCallback(() =>
                {
                    Vector3 dir = (targetPos - transform.position).normalized;
                    if (dir != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(dir);
                        transform.DORotateQuaternion(targetRotation, 0.2f);
                    }
                });

                movementSequence.Append(transform.DOMove(targetPos, 0.3f).SetEase(Ease.Linear));
            }

            movementSequence.OnComplete(() =>
            {
                OnMovementComplete?.Invoke();
                OnMovementComplete = null;
            });
        }

        public void Attack(IBuilding target)
        {
            if (target is HealthEntity healthEntity)
            {
                healthEntity.TakeDamage(Damage);
            }

            Debug.Log($"Soldier attacked {target.Name} for {Damage} damage.");
        }

        protected override void Elimination()
        {
            base.Elimination();
            movementSequence?.Kill();

            var tile = GridManager.Instance.GetTileAtPosition(transform.position);
            if (tile != null && tile.RuntimeSoldier == this)
            {
                tile.RuntimeSoldier = null;
                tile.Product = null;
                tile.Occupied = false;
            }

            PoolManager.Instance.Return(this);
        }

        public void OnSpawnFromPool() { }

        public void OnReturnToPool() => movementSequence?.Kill();
    }
}
