namespace Chapter6_ReceiptProcessor.Model
{
    public class Invoice
    {
        public string FileName { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string Gstin { get; set; }
        public string InvoiceAmount { get; set; }
        public string GstAmount { get; set; }
        public string TotalAmount { get; set; }
    }
}
