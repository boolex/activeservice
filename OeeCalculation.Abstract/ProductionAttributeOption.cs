namespace Production.Abstract
{
    public class ProductionAttributeOption
    {
        private string name;
        private string value;
        public ProductionAttributeOption(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
        public string Name
        {
            get { return name; }
        }
        public string Value
        {
            get { return value; }
        }
    }
}
