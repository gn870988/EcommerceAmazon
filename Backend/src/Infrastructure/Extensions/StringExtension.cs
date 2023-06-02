namespace Ecommerce.Infrastructure.Extensions;

public static class StringExtension
{
    public static bool IsNullOrEmpty(this string @this)
    {
        return string.IsNullOrEmpty(@this);
    }
}