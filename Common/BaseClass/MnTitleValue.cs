namespace Mn.NewsCms.Common.BaseClass
{
    public class MnTitleValue
    {
        public MnTitleValue()
        {

        }      
        public MnTitleValue(string title, long value)
            : this()
        {
            this.Title = title;
            this.Value = value;
        }
        public string Title { get; set; }
        public long Value { get; set; }
    }

    public class MnTitleValue<T>
    {
        public MnTitleValue()
        {

        }
        public MnTitleValue(string title, T value)
            : this()
        {
            this.Title = title;
            this.Value = value;
        }
        public string Title { get; set; }
        public T Value { get; set; }
    }

}