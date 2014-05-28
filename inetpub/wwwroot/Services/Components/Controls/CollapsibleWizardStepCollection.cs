using System;
using System.Collections.Generic;
using System.Web.UI;

namespace CKG.Components.Controls
{
    public sealed class CollapsibleWizardStepCollection : StateManagedCollection, IList<CollapsibleWizardStep>, ICollection<CollapsibleWizardStep>, IEnumerable<CollapsibleWizardStep>
    {
        private static readonly Type[] KnownTypes = new Type[] { typeof(CollapsibleWizardStep) };

        protected override void SetDirtyObject(object o)
        {
            ((CollapsibleWizardStep)o).SetDirty();
        }

        protected override void OnValidate(object o)
        {
            base.OnValidate(o);
            if (!(o is CollapsibleWizardStep))
            {
                throw new ArgumentException("Invalid type.");
            }
        }

        protected override object CreateKnownType(int index)
        {
            return new CollapsibleWizardStep();
        }

        protected override Type[] GetKnownTypes()
        {
            return CollapsibleWizardStepCollection.KnownTypes;
        }

        public CollapsibleWizardStep this[int index]
        {
            get { return (CollapsibleWizardStep)((System.Collections.IList)this)[index];}
            set { ((System.Collections.IList)this)[index] = value; }
        }

        public void Add(CollapsibleWizardStep item)
        {
            ((System.Collections.IList)this).Add(item);
        }

        public void Insert(int index, CollapsibleWizardStep item)
        {
            ((System.Collections.IList)this).Insert(index, item);
        }

        public bool Remove(CollapsibleWizardStep item)
        {
            var that = (System.Collections.IList)this;
            bool ret = that.Contains(item);
            that.Remove(item);
            return ret;
        }

        public void RemoveAt(int index)
        {
            ((System.Collections.IList)this).RemoveAt(index);
        }

        public new IEnumerator<CollapsibleWizardStep> GetEnumerator()
        {
            foreach (CollapsibleWizardStep step in (System.Collections.IEnumerable)this)
            {
                yield return step;
            }
        }

        IEnumerator<CollapsibleWizardStep> IEnumerable<CollapsibleWizardStep>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void ICollection<CollapsibleWizardStep>.Add(CollapsibleWizardStep item)
        {
            this.Add(item);
        }

        void ICollection<CollapsibleWizardStep>.Clear()
        {
            this.Clear();
        }

        bool ICollection<CollapsibleWizardStep>.Contains(CollapsibleWizardStep item)
        {
            return ((System.Collections.IList)this).Contains(item);
        }

        void ICollection<CollapsibleWizardStep>.CopyTo(CollapsibleWizardStep[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        int ICollection<CollapsibleWizardStep>.Count { get { return this.Count; } }

        bool ICollection<CollapsibleWizardStep>.IsReadOnly
        {
            get { return ((System.Collections.IList)this).IsReadOnly; }
        }

        bool ICollection<CollapsibleWizardStep>.Remove(CollapsibleWizardStep item)
        {
            return this.Remove(item);
        }

        int IList<CollapsibleWizardStep>.IndexOf(CollapsibleWizardStep item)
        {
            return ((System.Collections.IList)this).IndexOf(item);
        }

        void IList<CollapsibleWizardStep>.Insert(int index, CollapsibleWizardStep item)
        {
            this.Insert(index, item);
        }

        void IList<CollapsibleWizardStep>.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        CollapsibleWizardStep IList<CollapsibleWizardStep>.this[int index]
        {
            get { return this[index]; }
            set { this[index] = value; }
        }
    }
}
