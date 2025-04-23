using System;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace GridSystem
{
    public class GridTile : MonoBehaviour
    {
        [SerializeField] private Color obstacleColor;
        [SerializeField] private Gradient walkableColor;
        [SerializeField] protected SpriteRenderer tileSpriteRenderer;

        public ICoordinates Coords;
        public float GetDistance(GridTile other) => Coords.GetDistance(other.Coords);
        public bool Walkable { get; private set; }
        private bool occupied = false;

        private bool selected;
        private Color defaultColor;

        public bool Occupied { get; set; }
        public bool IsPreview { get; set; }

        public virtual void Init(bool walkable, bool isPreview, ICoordinates coords)
        {
            Walkable = walkable;
            IsPreview = isPreview;

            tileSpriteRenderer.color = walkable ? walkableColor.Evaluate(Random.Range(0f, 1f)) : obstacleColor;
            defaultColor = tileSpriteRenderer.color;

            OnHoverTile += OnOnHoverTile;

            Coords = coords;
            transform.position = Coords.Pos;
        }

        public static event Action<GridTile> OnHoverTile;
        private void OnEnable() => OnHoverTile += OnOnHoverTile;
        private void OnDisable() => OnHoverTile -= OnOnHoverTile;
        private void OnOnHoverTile(GridTile selected) => this.selected = selected == this;

        protected virtual void OnMouseDown()
        {
        }

        private void OnMouseEnter()
        {
            if (!Walkable || IsPreview) return;
            OnHoverTile?.Invoke(this);
        }

        #region Pathfinding
        public List<GridTile> Neighbors { get; set; }
        public GridTile Connection { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;

        public void SetConnection(GridTile nodeBase) => Connection = nodeBase;
        public void SetG(float g) { G = g; }
        public void SetH(float h) { H = h; }
        public void SetColor(Color color) => tileSpriteRenderer.color = color;
        public void RevertTile() => tileSpriteRenderer.color = defaultColor;
        #endregion
    }
}
