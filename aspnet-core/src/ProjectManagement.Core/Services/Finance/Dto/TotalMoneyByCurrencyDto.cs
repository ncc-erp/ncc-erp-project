using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.Finance.Dto
{
    public class TotalMoneyByCurrencyDto
    {
        public string CurrencyName { get; set; }
        public double Amount { get; set; }
        public double RoundAmount => Math.Round(Amount);
    }
}
