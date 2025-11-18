using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SwapService.Application.DTOs;
using SwapService.Application.Interfaces;
using SwapService.Extensions;

namespace SwapService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "staff")]
public class SwapController : ControllerBase
{
    private readonly ISwapService _swapService;

    public SwapController(ISwapService swapService)
    {
        _swapService = swapService;
    }

    [HttpPost("start")]
    public async Task<ActionResult<ApiResponse<SwapStartResponse>>> StartSwap([FromBody] SwapStartRequest request)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _swapService.StartSwapAsync(request, userId);
            return Ok(ApiResponse<SwapStartResponse>.SuccessResponse(result, "Swap started successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<SwapStartResponse>.FailureResponse(ex.Message));
        }
    }

    [HttpPost("complete")]
    public async Task<ActionResult<ApiResponse<SwapCompleteResponse>>> CompleteSwap([FromBody] SwapCompleteRequest request)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _swapService.CompleteSwapAsync(request, userId);
            return Ok(ApiResponse<SwapCompleteResponse>.SuccessResponse(result, "Swap completed successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<SwapCompleteResponse>.FailureResponse(ex.Message));
        }
    }

    [HttpPost("payment")]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> ProcessPayment([FromBody] SwapPaymentRequest request)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _swapService.ProcessPaymentAsync(request, userId);
            return Ok(ApiResponse<PaymentResponse>.SuccessResponse(result, "Payment processed successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PaymentResponse>.FailureResponse(ex.Message));
        }
    }

    [HttpGet("status/{swapId}")]
    public async Task<ActionResult<ApiResponse<SwapStatusResponse>>> GetSwapStatus(Guid swapId)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _swapService.GetSwapStatusAsync(swapId, userId);
            return Ok(ApiResponse<SwapStatusResponse>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<SwapStatusResponse>.FailureResponse(ex.Message));
        }
    }

    [HttpGet("history")]
    public async Task<ActionResult<ApiResponse<SwapHistoryResponse>>> GetSwapHistory([FromQuery] SwapHistoryRequest request)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _swapService.GetSwapHistoryAsync(request, userId);
            return Ok(ApiResponse<SwapHistoryResponse>.SuccessResponse(result, "Swap history retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<SwapHistoryResponse>.FailureResponse(ex.Message));
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<SwapListAllResponse>>> GetAllSwaps()
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _swapService.GetAllSwapsAsync(userId);
            return Ok(ApiResponse<SwapListAllResponse>.SuccessResponse(result, "All swaps retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<SwapListAllResponse>.FailureResponse(ex.Message));
        }
    }

    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<SwapListResponse>>> GetOngoingSwaps()
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _swapService.GetOngoingSwapsAsync(userId);
            return Ok(ApiResponse<SwapListResponse>.SuccessResponse(result, "Ongoing swaps retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<SwapListResponse>.FailureResponse(ex.Message));
        }
    }

    [HttpPut("cancel/{swapId}")]
    public async Task<ActionResult<ApiResponse<SwapCancelResponse>>> CancelSwap(Guid swapId)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _swapService.CancelSwapAsync(swapId, userId);
            return Ok(ApiResponse<SwapCancelResponse>.SuccessResponse(result, "Swap cancelled successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<SwapCancelResponse>.FailureResponse(ex.Message));
        }
    }
}
