using VDA5050.NET.Public.Models.Robots;

namespace VDA5050.NET.Public.Services.Client;

public interface IVda5050Client
{
    void Connect(RobotSimulatorConfig robotConfig);
    void Disconnect();
    void UpdateRobotState(RobotState robotState);
    
    void AddNewOrderHandler(NewOrderHandler handler);
    void AddOrderUpdateHandler(OrderUpdateHandler handler);
    void AddOrderCanceledHandler(OrderCanceledHandler handler);
    void AddInstantActionRequestHandler(InstantActionHandler handler);
    void AddInitPositionRequestHandler(OrderCanceledHandler handler);
}