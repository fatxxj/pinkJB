﻿using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using pinkJB_core.Utility;
using System.IO;

namespace pinkJB_core.Controllers
{
    public class PdfCreatorController : Controller
    {
        private readonly IConverter _converter;
        private readonly OrdersController _rdersController;
        public PdfCreatorController(IConverter converter)
        {
            _converter = converter;
        }

        [HttpGet]
        public IActionResult CreatePDF()
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                Out = @"C:\Users\fatha\Desktop\Generated1.pdf"  //USE THIS PROPERTY TO SAVE PDF TO A PROVIDED LOCATION
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
               // HtmlContent = TemplateGenerator.GetHTMLString(),
                Page = "https://localhost:44397/Orders/ShoppingCart", //USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            //_converter.Convert(pdf); IF WE USE Out PROPERTY IN THE GlobalSettings CLASS, THIS IS ENOUGH FOR CONVERSION

            var file = _converter.Convert(pdf);

            return Ok("Successfully created PDF document.");
            //return File(file, "application/pdf", "EmployeeReport.pdf");
            //return File(file, "application/pdf");
        }
    }
}
