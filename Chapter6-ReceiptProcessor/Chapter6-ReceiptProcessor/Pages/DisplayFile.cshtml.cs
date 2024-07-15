using Chapter6_ReceiptProcessor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;

namespace Chapter6_ReceiptProcessor.Pages
{
    public class DisplayFileModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public long FileSize { get; private set; }
        public Invoice Invoice = new Invoice();
        public DisplayFileModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult OnGet(string FileName, string InvoiceNumber, string InvoiceDate, string Gstin, string GstAmount, string TotalAmount)
        {



            if (!string.IsNullOrEmpty(FileName))
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploadsFolder, FileName);

                var fileProvider = new PhysicalFileProvider(uploadsFolder);
                var fileInfo = fileProvider.GetFileInfo(FileName);

                if (fileInfo.Exists)
                {
                    FilePath = filePath;
                    FileSize = fileInfo.Length;
                    Invoice.FileName = FileName;
                    Invoice.InvoiceNumber = InvoiceNumber;
                    Invoice.InvoiceDate = InvoiceDate;
                    Invoice.TotalAmount = TotalAmount;
                    Invoice.GstAmount = GstAmount;
                    Invoice.Gstin = Gstin;

                    return Page();
                }
            }

            return RedirectToPage("/Index");
        }
    }
}
