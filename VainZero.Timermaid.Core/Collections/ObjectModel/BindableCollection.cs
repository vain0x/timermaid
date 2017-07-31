using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Collections.ObjectModel
{
    /// <summary>
    /// Represents a collection of bindable objects
    /// which notifies changes as collection and all changes of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BindableCollection<T>
        : ReactiveCollection<T>
        where T : INotifyPropertyChanged
    {
        public event EventHandler<T> ItemChanged;

        protected virtual void OnItemChanged(T item)
        {
            ItemChanged?.Invoke(this, item);
        }

        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is T) OnItemChanged((T)sender);
        }

        protected override void OnAdded(T item)
        {
            item.PropertyChanged += OnItemPropertyChanged;
            base.OnAdded(item);
        }

        protected override void OnRemoved(T item)
        {
            item.PropertyChanged -= OnItemPropertyChanged;
            base.OnRemoved(item);
        }

        public BindableCollection()
        {
        }

        public BindableCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }
    }
}
