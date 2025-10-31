namespace Domain.Extensions;

public static class ExceptionsExtensions
{
    extension(InvalidOperationException ex)
    {
        public static void ThrowIfFalse(bool condition)
        {
            if (!condition) throw new InvalidOperationException();
        }
    }
}