using iTextSharp.text;
using iTextSharp.text.pdf;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Cinema.Account
{
    /// <summary>
    /// Сводное описание для Ticket
    /// </summary>
    public class Ticket : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("content-disposition", "inline; filename=ticket.pdf");

            List<string> items = MySQLServer.get_orders_information(int.Parse(context.Request["id"]));

            if (items.Count == 0 ||
                items[1] != context.Request.Cookies["login"].Value.ToUpper() ||
                items[4] != "confirmed")
            {
                context.Response.Redirect("~/Account/Profile");
            }

            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap QRImage = encoder.Encode(SignGenerator.GetSign(items[0] + items[1]));

            Document doc = new Document(iTextSharp.text.PageSize.A4, 40, 40, 42, 35);
            PdfWriter writer = PdfWriter.GetInstance(doc, context.Response.OutputStream);

            doc.Open();

            doc.Add(new Paragraph("User: " + items[1]));
            doc.Add(new Paragraph("Film: " + items[2]));
            doc.Add(new Paragraph("Date: " + items[3].Split(' ')[0]));
            doc.Add(new Paragraph("Time: " + items[3].Split(' ')[1]));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Cost: " + items[5]));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Places: " + items[6]));
            doc.Add(new Paragraph("\n"));
            iTextSharp.text.Image i = iTextSharp.text.Image.GetInstance(QRImage, ImageFormat.Jpeg);
            doc.Add(i);
            doc.Close();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}