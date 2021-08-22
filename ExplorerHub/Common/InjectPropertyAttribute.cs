using System;

namespace ExplorerHub
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InjectPropertyAttribute : Attribute
    {
        public Type ResolvedType { get; set; }
    }
}