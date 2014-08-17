using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    /*
    abstract class IObjectStorage
    {
        public IObjectStorage()
        {
        }

        public abstract void SetObject(IObject obj);

        public abstract IObject GetObject();

        public virtual void ResetVersionList()
        {
        }

        public virtual void StepToNextVersion()
        {
        }
    }

    class IObjectStorageSingle : IObjectStorage
    {
        private IObject theObject;

        public IObjectStorageSingle(IObject obj)
        {
            theObject = obj;
        }

        public override void SetObject(IObject obj)
        {
            theObject = obj;
        }

        public override IObject GetObject()
        {
            return theObject;
        }
    }

    /*
    class UserDefinedElement : IObjectStorage
    {
        private List<IObject> objects;
        private int currentIndex;

        public UserDefinedElement(IObject obj)
        {
            objects = new List<IObject>();
            objects.Add(obj);
            currentIndex = 0;
        }

        public override void SetObject(IObject obj)
        {
            ++currentIndex;
            if (objects.Count > currentIndex)
                objects[currentIndex] = obj;
            else
                objects.Add(obj);
        }

        public override IObject GetObject()
        {
            return objects[currentIndex];
        }

        public override void ResetVersionList()
        {
            currentIndex = 0;
        }

        public override void StepToNextVersion()
        {
            ++currentIndex;
        }
    }
     * */
}
