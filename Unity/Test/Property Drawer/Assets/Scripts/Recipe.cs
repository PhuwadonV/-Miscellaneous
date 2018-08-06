using UnityEngine;
using System;

public enum IngredientUnit { Spoon, Cup, Bowl, Piece }

[Serializable]
public class Ingredient
{
    public uint amount;
    public IngredientUnit unit;
}

public class Recipe : MonoBehaviour {
    public Ingredient result;
    public Ingredient[] ingredient;
}
