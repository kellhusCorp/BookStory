using System;
using System.ComponentModel.DataAnnotations;

namespace BookStory.Attributes
{
    public class OnlyPastDateTimeAttribute : RangeAttribute
    {
        private static string format = "dd.MM.yyyy HH:mm:ss";

        public OnlyPastDateTimeAttribute()
            : base(typeof(DateTime),
                DateTime.MinValue.ToString(format),
                DateTime.Now.ToString(format))
        {
        }
    }
}