using Microsoft.EntityFrameworkCore;
using MySqlConnector;

public static class ExceptionExtensions
{
    public static string ToClientMessage(this Exception exception)
    {
        if (exception is DbUpdateException dbUpdateException)
        {
            if (dbUpdateException.InnerException is MySqlException mySqlException)
            {
                return mySqlException.Number switch
                {
                    1452 => "Invalid reference id: FK constraint violation. Please verify related ids exist.",
                    1451 => "Cannot modify or delete this record because it is referenced by other records.",
                    1062 => "Duplicate value detected. Please use unique values.",
                    _ => "Database update failed. Please verify your input data and related ids."
                };
            }

            return "Database update failed. Please verify your input data and try again.";
        }

        return exception.Message;
    }
}