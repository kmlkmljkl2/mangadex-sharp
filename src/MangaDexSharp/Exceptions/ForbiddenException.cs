namespace MangaDexSharp.Exceptions
{
    public class ForbiddenException : MangaDexException
    {
        internal ForbiddenException(Error error)
            : base(403, error)
        {
        }
    }
}