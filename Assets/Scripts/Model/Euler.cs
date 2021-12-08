using UnityEngine;

public class Euler
{
    private float _x;
    private float _y;
    private float _z;

    public float x
    {
        get => _x;
        set => _x = clampValue(value);
    }
    public float y
    {
        get => _y;
        set => _y = clampValue(value);
    }
    public float z
    {
        get => _z;
        set => _z = clampValue(value);
    }

    public Euler(float x = 0, float y = 0, float z = 0)
    {
        this._x = x;
        this._y = y;
        this._z = z;
    }
    private float clampValue(float value)
    {
        if (value > 180)
            return -180 + (value - 180);
        else if (value < -180)
            return 180 + (value + 180);
        return value;
    }


    public static implicit operator Vector3(Euler data)
    {
        return new Vector3(data.x, data.y, data.z);
    }
}

