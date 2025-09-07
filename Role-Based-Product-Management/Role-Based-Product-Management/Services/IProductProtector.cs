namespace Role_Based_Product_Management.Services
{
    public interface IProductProtector
    {
        string Protect(decimal price);
        bool TryUnprotect(string protectedValue, out decimal price);
    }
}
