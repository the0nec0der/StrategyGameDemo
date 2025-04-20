using UnityEngine;

public interface ICoordinates
{
    public float GetDistance(ICoordinates other);
    public Vector2 Pos { get; set; }
}