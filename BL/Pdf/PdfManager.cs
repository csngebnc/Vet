using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using MigraDoc.Rendering;
using Vet.Models;
using Vet.BL.Pdf.Style;
using Vet.BL.Pdf.Content;
using System.IO;

namespace Vet.BL
{
    public class PdfManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        public PdfManager(IUserRepository userRepository, IMedicalRecordRepository medicalRecordRepository)
        {
            _userRepository = userRepository;
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<string> GeneratePdf(string path, MedicalRecord record)
        {
            var user = await _userRepository.GetUserByIdAsync(record.OwnerId);

            var document = CreateDocument(user, record);

            var pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save(path + ".pdf");

            return path;
        }

        public Document CreateDocument(VetUser user, MedicalRecord record)
        {
            var doc = new Document();
            //CustomStyles.Define(doc);
            doc.Add(CreateMainSection(user, record));
            return doc;
        }
        
        private Section CreateMainSection(VetUser user, MedicalRecord record)
        {
            var section = new Section();
            SetUpPage(section);
            AddHeaderAndFooter(section);
            AddContents(section, user, record);
            return section;
        }

        private void SetUpPage(Section section)
        {
            section.PageSetup.PageFormat = PageFormat.Letter;

            section.PageSetup.LeftMargin = Size.LeftRightPageMargin;
            section.PageSetup.TopMargin = Size.TopBottomPageMargin;
            section.PageSetup.RightMargin = Size.LeftRightPageMargin;
            section.PageSetup.BottomMargin = Size.TopBottomPageMargin;

            section.PageSetup.HeaderDistance = Size.HeaderFooterMargin;
            section.PageSetup.FooterDistance = Size.HeaderFooterMargin;
        }
        private void AddHeaderAndFooter(Section section)
        {
            new HeaderAndFooter().Add(section);
        }

        private void AddContents(Section section, VetUser user, MedicalRecord record)
        {
            AddPatientInfo(section, user, record);
            AddStructureSet(section, record);
        }

        private void AddPatientInfo(Section section, VetUser user, MedicalRecord record)
        {
            new PatientInfo().Add(section, user, record);
        }

        private void AddStructureSet(Section section, MedicalRecord record)
        {
            new StructureSetContent().Add(section, record);
        }

    }
}
