    using Chapter6_ReceiptProcessor.Services;
    using Microsoft.Extensions.FileProviders;

    namespace Chapter6_ReceiptProcessor
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddRazorPages();

                var uploadDirectory = builder.Configuration.GetSection("UploadDirectory").Value;

                var fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), uploadDirectory));


                builder.Services.AddSingleton<IFileProvider>(fileProvider);

                builder.Services.AddSingleton<IInvoiceProcessor, InvoiceProcessor>();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthorization();

                app.MapRazorPages();

                app.Run();
            }
        }
    }
