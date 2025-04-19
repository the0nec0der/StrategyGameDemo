using System;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace GridSystem
{
    public abstract class NodeBase : MonoBehaviour
    {
        [SerializeField] private Color obstacleColor;
        [SerializeField] private Gradient walkableColor;

        [SerializeField] protected SpriteRenderer tileSpriteRenderer;

        public ICoords Coords;
        public float GetDistance(NodeBase other) => Coords.GetDistance(other.Coords);
        public bool Walkable { get; private set; }

        private bool selected;
        private Color defaultColor;

        public virtual void Init(bool walkable, ICoords coords)
        {
            Walkable = walkable;

            tileSpriteRenderer.color = walkable ? walkableColor.Evaluate(Random.Range(0f, 1f)) : obstacleColor;
            defaultColor = tileSpriteRenderer.color;

            OnHoverTile += OnOnHoverTile;

            Coords = coords;
            transform.position = Coords.Pos;
        }

        public static event Action<NodeBase> OnHoverTile;
        private void OnEnable() => OnHoverTile += OnOnHoverTile;
        private void OnDisable() => OnHoverTile -= OnOnHoverTile;
        private void OnOnHoverTile(NodeBase selected) => this.selected = selected == this;

        protected virtual void OnMouseDown()
        {
            if (!Walkable) return;
            OnHoverTile?.Invoke(this);
        }

        #region Pathfinding

        [Header("Pathfinding")]
        public List<NodeBase> Neighbors { get; protected set; }
        public NodeBase Connection { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;

        public abstract void CacheNeighbors();
        public void SetConnection(NodeBase nodeBase) => Connection = nodeBase;
        public void SetG(float g) { G = g; }
        public void SetH(float h) { H = h; }
        public void SetColor(Color color) => tileSpriteRenderer.color = color;
        public void RevertTile() => tileSpriteRenderer.color = defaultColor;

        #endregion
    }
}

public interface ICoords
{
    public float GetDistance(ICoords other);
    public Vector2 Pos { get; set; }
}