using Microsoft.AspNetCore.DataProtection;
using System.Globalization;

namespace Role_Based_Product_Management.Services
{
    public class ProductProtector : IProductProtector
    {
        private readonly IDataProtector _protector;
        public ProductProtector(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("ProductPriceProtector.v1");
        }

        public string Protect(decimal price)
        {
            var s = price.ToString(CultureInfo.InvariantCulture);
            return _protector.Protect(s);
        }

        public bool TryUnprotect(string protectedValue, out decimal price)
        {
            price = 0m;
            try
            {
                var raw = _protector.Unprotect(protectedValue);
                if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var p))
                {
                    price = p; return true;
                }
            }
            catch { }
            return false;
        }
    }
}
