namespace ApiNetCore.Models
{
    public class EnumValueAttribute : System.Attribute
    {
        private string _value;

        public EnumValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }
}