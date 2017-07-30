using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Misc
{
    public sealed class InvalidValueException
        : InvalidOperationException
    {
        public object Value { get; }

        static string CreateMessage(object value)
        {
            return
                value == null
                    ? "Invalid null."
                    : $"Invalid value: {value}.";
        }

        public InvalidValueException(object value)
            : base(CreateMessage(value))
        {
            Value = value;
        }

        public InvalidValueException(object value, Exception innerException)
            : base(CreateMessage(value), innerException)
        {
            Value = value;
        }
    }
}
