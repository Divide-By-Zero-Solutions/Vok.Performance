namespace Vok.Performance;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class PerfCritcalAttribute : Attribute {
    public PerfCritcalAttribute(int threshold) {
        Threshold = threshold;
    }

    public int Threshold { get; }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class PerfIgnoreAttribute : Attribute {
}
