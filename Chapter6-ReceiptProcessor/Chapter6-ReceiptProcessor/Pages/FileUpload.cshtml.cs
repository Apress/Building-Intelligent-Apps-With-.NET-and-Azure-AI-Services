using Chapter6_ReceiptProcessor.Model;
using Chapter6_ReceiptProcessor.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chapter6_ReceiptProcessor.Pages
{
    public class FileUploadModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IInvoiceProcessor _invoiceProcessor;
        public FileUploadModel(IWebHostEnvironment environment, IInvoiceProcessor invoiceProcessor)
        {
            _environment = environment;
            _invoiceProcessor = invoiceProcessor;
        }
        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                Invoice invoice = new Invoice();


                using (var stream = new FileStream(filePath, FileMode.Create))
                {

                    await file.CopyToAsync(stream);
                }

                Stream data = ConvertFormFileToStream(file);

                invoice = _invoiceProcessor.GetInvoice(data);
                invoice.FileName = fileName;

                return RedirectToPage("/DisplayFile", new { invoice.FileName, invoice.InvoiceNumber, invoice.InvoiceDate, invoice.Gstin, invoice.GstAmount, invoice.TotalAmount });
            }

            else
            {
                Message = "Please select a file to upload.";
            }

            return Page();
        }
        public Stream ConvertFormFileToStream(IFormFile file)
        {
            // Check if the file is null
            if (file == null)
            {
                return null;
            }

            // Open a stream to read the contents of the IFormFile
            var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Seek(0, SeekOrigin.Begin); // Rewind the stream to the beginning

            return stream;
        }
    }
}
