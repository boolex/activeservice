namespace OeeCalculation.Computing.Production
{
    using OeeCalculation.Computing.Calculator;
    public class OrderProduction
    {
        private readonly OsProductionPeriodCalculator calculator;
        public OrderProduction(OsProductionPeriodCalculator calculator)
        {
            this.calculator = calculator;
        }
        //public void StartUnit() { calculator.Handle(); }
        //public void EndUnit() { calculator.Handle(); }
        //public void ScrapUnit() { calculator.Handle(); }
    }
}
