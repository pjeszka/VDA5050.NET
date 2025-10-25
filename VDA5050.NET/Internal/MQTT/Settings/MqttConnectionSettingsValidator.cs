using FluentValidation;

namespace VDA5050.NET.Internal.MQTT.Settings;

public sealed class MqttConnectionSettingsValidator
    : AbstractValidator<MqttConnectionSettings>
{
    public MqttConnectionSettingsValidator()
    {
        RuleFor(x => x.ReconnectionPeriodInMs)
            .GreaterThanOrEqualTo(100);

        RuleFor(x => x.BrokerAddress)
            .NotEmpty();

        RuleFor(x => x.Port)
            .GreaterThan(0);

        RuleFor(x => x.Username)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
