public class PaymentDto
{
    public string BookingCode { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionRef { get; set; }
}
