internal class RPGStat
{
    public float Total;

    public float Current;

    public RPGStat(float total){
        Total = total;
        Reset();
    }

    public void Reset(){
        Current = Total;
    }
}