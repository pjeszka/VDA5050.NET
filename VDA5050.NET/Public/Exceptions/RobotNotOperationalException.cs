using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Public.Exceptions;

public sealed class RobotNotOperationalException : Exception
{
    private const string MessageFormat = "Robot with serial number {0} is not operational";
    public RobotNotOperationalException(RobotSerialNumber RobotSerialNumber) 
        : base(string.Format(MessageFormat, RobotSerialNumber))
    {
    }
}