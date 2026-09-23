namespace Performance;

/// <summary>Marks a class or benchmark method with a performance threshold.</summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class PerfCritcalAttribute : Attribute {
    /// <summary>Initializes the attribute with a threshold in the benchmark's unit.</summary>
    /// <param name="threshold">The maximum accepted threshold.</param>
    public PerfCritcalAttribute(int threshold) {
        Threshold = threshold;
    }

    /// <summary>Gets the configured threshold.</summary>
    public int Threshold { get; }
}

/// <summary>Marks a class or benchmark method as excluded from performance gating.</summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class PerfIgnoreAttribute : Attribute {
}
