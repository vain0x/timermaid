using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Collections.ObjectModel
{
    public abstract class BindableViewCollection<TSource, TTarget>
        : BindableCollection<TTarget>
        where TTarget : INotifyPropertyChanged
    {
        protected abstract TSource GetSource(TTarget target);

        ICollection<TSource> Sources { get; }

        protected override void OnAdded(TTarget item)
        {
            if (Sources == null) return;
            Sources.Add(GetSource(item));
            base.OnAdded(item);
        }

        protected override void OnRemoved(TTarget item)
        {
            if (Sources == null) return;
            Sources.Remove(GetSource(item));
            base.OnRemoved(item);
        }

        public BindableViewCollection(IEnumerable<TTarget> targets, ICollection<TSource> sources)
            : base(targets)
        {
            Sources = sources ?? throw new ArgumentNullException(nameof(sources));
        }
    }
}
