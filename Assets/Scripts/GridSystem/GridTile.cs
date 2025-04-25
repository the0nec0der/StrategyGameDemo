using System;
using System.Collections.Generic;
using Gameplay.Product;
using Gameplay.SoldierUnits;
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

        private bool selected;
        private Color defaultColor;

        public Color ChangedColor { get; set; }
        public bool Occupied { get; set; }
        public bool IsPreview { get; set; }
        public IProduct Product { get; set; }
        public SoldierController RuntimeSoldier { get; set; }

        public virtual void Init(bool walkable, bool isPreview, ICoordinates coords)
        {
            Walkable = walkable;
            IsPreview = isPreview;

            GetComponent<Collider>().enabled = !isPreview;

            tileSpriteRenderer.color = walkable ? walkableColor.Evaluate(Random.Range(0f, 1f)) : obstacleColor;
            defaultColor = tileSpriteRenderer.color;

            OnTileHovered += TileHover;

            Coords = coords;
            transform.position = new Vector3(coords.Pos.x, 0f, coords.Pos.y);
        }

        public static event Action<GridTile> OnTileHovered;
        public static event Action<GridTile> OnTileSelected;
        private void OnEnable()
        {
            OnTileHovered += TileHover;
            OnTileSelected += TileSelect;
        }
        private void OnDisable()
        {
            OnTileHovered -= TileHover;
            OnTileSelected -= TileSelect;
        }
        private void TileHover(GridTile selected) { }

        private void TileSelect(GridTile selected)
        {
            this.selected = selected == this;
        }

        protected virtual void OnMouseDown()
        {
            if (!Walkable || IsPreview) return;
            OnTileSelected?.Invoke(this);
        }

        private void OnMouseEnter()
        {
            if (!Walkable || IsPreview) return;
            OnTileHovered?.Invoke(this);
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
        public void RevertTile()
        {
            tileSpriteRenderer.color = !Occupied ? defaultColor : ChangedColor;
        }
        #endregion
    }
}
