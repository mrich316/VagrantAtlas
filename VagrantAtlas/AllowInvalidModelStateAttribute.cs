using System;

namespace VagrantAtlas
{
    /// <summary>
    /// Actions and controllers marked with this attribute are skipped by <see cref="ValidateAttribute"/>
    /// during model state validation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class AllowInvalidModelStateAttribute : Attribute
    {
    }
}
