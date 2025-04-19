public class StatikShiv : IPoolable
{
    public bool PoweredVersion;

    public override string getReference()
    {
        return PoweredVersion ? "PoweredStatik" : "DefaultStatik";
    }
}
