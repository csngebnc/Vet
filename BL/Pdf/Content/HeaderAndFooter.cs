using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL.Pdf.Style;

namespace Vet.BL.Pdf.Content
{
    internal class HeaderAndFooter
    {
        public void Add(Section section)
        {
            AddHeader(section);
            AddFooter(section);
        }

        private void AddHeader(Section section)
        {
            var header = section.Headers.Primary.AddParagraph();
            header.Format.AddTabStop(Size.GetWidth(section), TabAlignment.Right);

            header.AddText("BunnyV állatkórház - kórlap");
            header.AddTab();
            header.AddText($"Generálva: {DateTime.Now:g}");
        }

        private void AddFooter(Section section)
        {
            var footer = section.Footers.Primary.AddParagraph();
            footer.Format.AddTabStop(Size.GetWidth(section), TabAlignment.Right);

            footer.AddText("BunnyV állatkórház - a legfontosabb az egészség");
            footer.AddTab();
            footer.AddText("Oldal: ");
            footer.AddPageField();
            footer.AddText(" / ");
            footer.AddNumPagesField();
        }
    }
}
