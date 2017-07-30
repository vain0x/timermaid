using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using VainZero.Misc;
using VainZero.Timermaid.Data.Entity;

namespace VainZero.Timermaid.ScheduleLists
{
    public sealed class ScheduleView
        : BindableBase
    {
        public Schedule Source { get; }

        void SetProperty<X>(X original, Action<X> set, X value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<X>.Default.Equals(original, value)) return;
            set(value);
            RaisePropertyChanged(propertyName);
        }

        public string Name
        {
            get => Source.Name;
            set => SetProperty(Name, x => Source.Name = x, value);
        }

        public DateTime DueTime
        {
            get => Source.DueTime;
            set
            {
                SetProperty(DueTime, x => Source.DueTime = x, value);
                RaisePropertyChanged(nameof(Status));
            }
        }

        public string FilePath
        {
            get => Source.FilePath;
            set => SetProperty(FilePath, x => Source.FilePath = x, value);
        }

        public string Argument
        {
            get => Source.Argument;
            set => SetProperty(Argument, x => Source.Argument = x, value);
        }

        bool IsActive => DueTime > DateTime.Now;

        public ScheduleViewStatus Status
        {
            get => ScheduleViewStatusModule.ToView(Source.Status, IsActive);
            set => SetProperty(Status, x => Source.Status = ScheduleViewStatusModule.FromView(x), value);
        }

        public ScheduleView()
            : this(new Schedule())
        {
        }

        public ScheduleView(Schedule source)
        {
            Source = source;
        }
    }
}
