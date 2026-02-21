using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Robots;

namespace VDA5050.NET.Public.Services.Client;

public interface IVda5050Client
{
    void AddRobot(RobotConfig robotConfig);
    void RemoveRobot(RobotSerialNumber robotSerialNumber);
    void UpdateRobotState(RobotState robotState);
    
    void AddNewOrderHandler(NewOrderHandler handler);
    void AddOrderUpdateHandler(OrderUpdateHandler handler);
    void AddOrderCanceledHandler(OrderCanceledHandler handler);
    void AddInstantActionRequestHandler(OrderCanceledHandler handler);
}