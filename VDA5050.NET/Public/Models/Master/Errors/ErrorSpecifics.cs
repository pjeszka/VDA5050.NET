using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Public.Enums.Domain;
using VDA5050.NET.Public.Enums.Vda5050.State;

namespace VDA5050.NET.Public.Models.Errors;

public sealed record ErrorSpecifics(
    string ErrorType,
    ErrorLevel Level,
    ICollection<ErrorReference>? ErrorReferences,
    string? ErrorDescription,
    string? ErrorHint)
{
    internal static ErrorSpecifics FromMessage(ErrorItemMessage message)
    {
        return new ErrorSpecifics(
            message.ErrorType,
            message.ErrorLevel,
            message.ErrorReferences?.Select(ErrorReference.FromMessage).ToList(),
            message.ErrorDescription,
            message.ErrorHint);
    }
}

public sealed record ErrorReference(string Id, ErrorReferenceType Type)
{
    internal static ErrorReference FromMessage(ErrorReferenceMessage message)
    {
        return new ErrorReference(message.ReferenceValue, ErrorReferenceTypeHelper.FromMessage(message.ReferenceKey));
    }
}
    