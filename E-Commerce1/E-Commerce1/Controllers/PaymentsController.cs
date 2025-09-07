using E_Commerce1.Data;
using E_Commerce1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly ECommerceDb _db;
        public PaymentsController(ECommerceDb db) { _db = db; }

        public class ProcessReq { public int OrderId { get; set; } public int UserId { get; set; } public int Amount { get; set; } public string? Provider { get; set; } = "Mock"; }

        [HttpPost("process")]
        public async Task<IActionResult> Process([FromBody] ProcessReq req)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == req.OrderId && o.UserId == req.UserId);
            if (order == null) return NotFound("Order not found for user.");
            if (order.Total != req.Amount) return BadRequest("Amount mismatch.");

            var payReq = new PaymentRequest
            {
                OrderId = order.Id,
                UserId = req.UserId,
                Amount = req.Amount,
                Provider = string.IsNullOrWhiteSpace(req.Provider) ? "Mock" : req.Provider!
            };
            _db.PaymentRequests.Add(payReq);
            await _db.SaveChangesAsync();

            var transactionId = Guid.NewGuid().ToString("N");

            var payResp = new PaymentResponse
            {
                PaymentRequestId = payReq.Id,
                Success = true,
                TransactionId = transactionId
            };
            _db.PaymentResponses.Add(payResp);

            order.Status = "Paid";
            await _db.SaveChangesAsync();

            return Ok(new { success = payResp.Success, transactionId });
        }
    }
}
