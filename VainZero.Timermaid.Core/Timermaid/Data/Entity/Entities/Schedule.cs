using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using VainZero.Misc;

namespace VainZero.Timermaid.Data.Entity
{
    /// <summary>
    /// Represents a schedule to be executed in due time.
    /// </summary>
    [Table("schedules")]
    public class Schedule
        : BindableBase
    {
        long id;

        [Key]
        [Column("schedule_id")]
        public long Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        string name;

        [Column("name")]
        [StringLength(1024)]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        DateTime dueTime;

        [Column("due_time")]
        public DateTime DueTime
        {
            get => dueTime;
            set => SetProperty(ref dueTime, value);
        }

        string filePath;

        [Column("file_path")]
        [StringLength(1024)]
        public string FilePath
        {
            get => filePath;
            set => SetProperty(ref filePath, value);
        }

        string argument;

        [Column("argument")]
        [StringLength(1024)]
        public string Argument
        {
            get => argument;
            set => SetProperty(ref argument, value);
        }

        ScheduleStatus status;

        [Column("status")]
        public ScheduleStatus Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        public void Disable()
        {
            switch (Status)
            {
                case ScheduleStatus.Enabled:
                    Status = ScheduleStatus.Disabled;
                    break;
                case ScheduleStatus.Disabled:
                case ScheduleStatus.Hidden:
                    break;
                default:
                    throw new InvalidValueException(Status);
            }
        }

        public Schedule()
        {
            DueTime = DateTime.Now;
        }
    }

    public enum ScheduleStatus
    {
        Enabled,
        Disabled,
        Hidden,
    }
}
