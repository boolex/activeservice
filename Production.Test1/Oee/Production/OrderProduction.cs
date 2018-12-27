using Production.Test1.Oee.Calc;
using System;
namespace Production.Test1.Oee
{
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
