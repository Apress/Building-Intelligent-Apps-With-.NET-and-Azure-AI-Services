using Chapter6_ReceiptProcessor.Model;

namespace Chapter6_ReceiptProcessor.Services
{
    public interface IInvoiceProcessor
    {
        public Invoice GetInvoice(Stream fileContent);
    }
}
