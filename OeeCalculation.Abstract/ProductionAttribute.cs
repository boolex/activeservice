namespace Production.Abstract
{
    public class ProductionAttribute
    {
        private readonly string name;
        private readonly ProductionAttributeOption[] options;
        public ProductionAttribute(string name, ProductionAttributeOption[] options)
        {
            this.name = name;
            this.options = options;
        }
        public string Name
        {
            get { return name; }
        }
        public ProductionAttributeOption[] Options
        {
            get { return options; }
        }
    }
}
