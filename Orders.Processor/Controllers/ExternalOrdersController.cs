using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/externalorders")]
public class ExternalOrdersController : ControllerBase
{
    private readonly ILogger<ExternalOrdersController> _logger;
    private readonly DaprClient _daprClient;
    public ExternalOrdersController(ILogger<ExternalOrdersController> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    [Topic("pubsub-servicebus", "orderreceivedtopic")]
    [HttpPost("orderreceived")]
    public async Task<IActionResult> OrderReceived([FromBody] OrderModel orderModel)
    {
        _logger.LogInformation("Received new order at: '{0}' Order Id: '{1}' Order reference: '{2}', Order quantity: '{3}'",
                                DateTime.UtcNow, orderModel.OrderId, orderModel.Reference, orderModel.Quantity);

        //Do your business logic with order received
        orderModel.CreatedOn = DateTime.UtcNow;

        ////ToDo: Your exercise :) Save the received message into CosmoDb using the SveStateAsync
        //await _daprClient.SaveStateAsync<OrderModel>("statestore-cosmosdb", orderModel.OrderId.ToString(), orderModel);

        //Return 200 ok to acknowledge order is processed successfully          
        return Ok($"Order Processing completed successfully");

        //Retunr 400 bad request to retry re-processing based on service broker configuration
        //return BadRequest($"Failed to process order due to: failure reason");
    }
}