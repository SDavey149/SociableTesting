using System;

namespace AutofacClassicalTesting
{
    public class ContainerAlreadyBuiltException : Exception
    {
        public ContainerAlreadyBuiltException() : base("Container is built once Sut property has been accessed")
        {
        }
    }
}