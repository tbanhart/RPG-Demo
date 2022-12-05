using UnityEngine;

public struct RPGStat
{
    float _total;

    float _current;

    public RPGStat(float total){
        _total = total;
        _current = 0;
    }

    public float Add(float value)
    {
        _current = Mathf.Clamp(_current + value, 0f, _total);
        return GetPercent();
    }

    public float GetPercent()
    {
        return _current / _total;
    }

    public float SetMax()
    {
        _current = _total;
        return GetPercent();
    }
    
    public float SetMin()
    {
        _current = 0;
        return 0;
    }
}