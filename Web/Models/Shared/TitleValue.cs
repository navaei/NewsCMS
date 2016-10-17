using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tazeyab.Web.Models
{
    public class TitleValue
    {
        public TitleValue()
        {

        }
        public TitleValue(string title, int value)
            : this()
        {
            this.Title = title;
            this.Value = value;
        }
        public string Title { get; set; }
        public int Value { get; set; }
    }

    public class TitleValue<T>
    {
        public TitleValue()
        {

        }
        public TitleValue(string title, T value)
            : this()
        {
            this.Title = title;
            this.Value = value;
        }
        public string Title { get; set; }
        public T Value { get; set; }
    }
}