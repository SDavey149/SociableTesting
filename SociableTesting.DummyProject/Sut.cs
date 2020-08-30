﻿using System;

namespace DummyProject
{
    public class ClassDependingOnIMockedDependency
    {
        public ClassDependingOnIMockedDependency(IDependency dependency,
            IMockedDependency mockThisDependency)
        {
        }
    }
}