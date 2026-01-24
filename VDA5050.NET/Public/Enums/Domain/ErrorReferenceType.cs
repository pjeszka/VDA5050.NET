namespace VDA5050.NET.Public.Enums.Domain;

public enum ErrorReferenceType
{
    OrderId,
    NodeId,
    EdgeId,
    ActionId,
    Other,
}

public static class ErrorReferenceTypeHelper
{
    public static string ToKey(this ErrorReferenceType errorReferenceType) => errorReferenceType.ToString().ToCamelCase();
    
    private static string ToCamelCase(this string pascalCase)
    {
        if (string.IsNullOrEmpty(pascalCase))
            return pascalCase;

        if (pascalCase.Length == 1)
            return pascalCase.ToLowerInvariant();

        return char.ToLowerInvariant(pascalCase[0]) + pascalCase[1..];
    }

    public static ErrorReferenceType FromMessage(string referenceKey)
    {
        if (Enum.TryParse<ErrorReferenceType>(referenceKey, out var referenceType))
        {
            return referenceType;
        }
        
        return ErrorReferenceType.Other;
    }
}