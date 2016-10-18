using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeatureTest
{
    [CallTrace(true)]
    abstract class ObjectWithAspects : ContextBoundObject
    {
    }

    abstract class SecondaryObjectWithAspects : ObjectWithAspects
    {
        
    }

    abstract class ThirdObjectWithAspects : SecondaryObjectWithAspects
    {
        
    }
}
