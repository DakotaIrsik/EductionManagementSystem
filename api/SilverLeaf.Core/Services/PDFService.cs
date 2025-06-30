using Microsoft.Extensions.Options;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using SilverLeaf.Common;
using SilverLeaf.Core.Objects;
using SilverLeaf.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SilverLeaf.Core.Services
{
    public interface IPDFService
    {
        string GenerateScreenerEvaluationForm(CompletionScreenerDTO completionScreenerDTO);
    }

    public class PDFService : IPDFService
    {
        private readonly AppSettings _settings;

        public PDFService(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateScreenerEvaluationForm(CompletionScreenerDTO completionScreenerDTO)
        {
            const string EvaluationForm = "data/pdfs/ScreenerEvaluationForm11-17-2019.pdf";

            PdfSharp.Pdf.PdfDocument PDFDoc = PdfReader.Open(EvaluationForm, PdfDocumentOpenMode.Import);
            PdfSharp.Pdf.PdfDocument PDFNewDoc = new PdfSharp.Pdf.PdfDocument();
            for (int Pg = 0; Pg < PDFDoc.Pages.Count; Pg++)
            {
                PDFNewDoc.AddPage(PDFDoc.Pages[Pg]);
            }
            var pp = PDFNewDoc.Pages[0];
            XGraphics gfx = XGraphics.FromPdfPage(pp);
            var renderables = GetDrawableObjects(completionScreenerDTO);
            foreach (var r in renderables)
            {
                gfx.DrawString(r.Text, r.Font, XBrushes.Crimson, r.Rect);
            }
            var fileName = $"{completionScreenerDTO.Student.EnglishName}-{ DateTime.UtcNow.AddHours(8).ToString("mmddyyyyss")}.pdf";
            var fullImageNameAndFilePath = $"{_settings.StaticFilePath}screeners/reports/{fileName}";
            PDFNewDoc.Save(fullImageNameAndFilePath);
            CreateImage(fullImageNameAndFilePath);

            return $"{_settings.Url}{_settings.StaticFileAlias}/screeners/reports/{fileName.Replace(".pdf", ".png")}";
        }

        static void CreateImage(string pdfFileName)
        {
            pdfFileName = "\"" + pdfFileName + "\"";
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "C:\\Program Files\\poppler\\pdftoppm.exe";
            p.StartInfo.Arguments = $@"-png -r 300 -singlefile {pdfFileName} {pdfFileName.Replace(".pdf", "")}";
            p.Start();
        }

        private static double GetCoordinateFromInch(double inches)
        {
            return inches * 72;
        }

        private List<PDFDrawableBox> GetDrawableObjects(CompletionScreenerDTO model)
        {
            var list = new List<PDFDrawableBox>();
            //var options = new XPdfFontOptions(PdfFontEmbedding.Always);

            #region Header
            list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(1.05), Y = GetCoordinateFromInch(1.125), Font = new XFont("Droid Sans Fallback", 10, XFontStyle.Regular), Text = model.Student.NativeName });
            list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(1.60), Y = GetCoordinateFromInch(1.125), Text = $"({model.Student.EnglishName})" });
            list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(4.3), Y = GetCoordinateFromInch(1.125), Text = model.StarReaderId });
            list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(6.5), Y = GetCoordinateFromInch(1.125), Text = model.Student.Grade });
            list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(7.5), Y = GetCoordinateFromInch(1.125), Text = model.Student.Age.ToString() });
            list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(.85), Y = GetCoordinateFromInch(1.34), Text = model.GeneratedOn.ToString("MM/dd/yyyy") });
            list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(1.2), Y = GetCoordinateFromInch(1.54), Text = model.Assessor });
            #endregion

            #region Initial Interview
            list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(.47), Y = GetCoordinateFromInch(2.45), Text = model.AreasOfStrength });
            list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(.47), Y = GetCoordinateFromInch(2.95), Text = model.AreasForImprovement });
            list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(.47), Y = GetCoordinateFromInch(3.5), Text = model.ExtraInformationGained });
            #endregion

            #region Phonics Screener

                #region Phonics 1
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(1.75), Y = GetCoordinateFromInch(4.67), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 1 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(3.2), Y = GetCoordinateFromInch(4.67), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 1 && t.Task == 2).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(4.77), Y = GetCoordinateFromInch(4.67), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 1 && t.Task == 3).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(6.4), Y = GetCoordinateFromInch(4.67), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 1 && t.Task == 4).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(7.4), Y = GetCoordinateFromInch(4.67), Text = model.Phonics.TotalCorrect(1).ToString() });
                #endregion

                #region Phonics 2
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(1.75), Y = GetCoordinateFromInch(5.32), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 2 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(3.2), Y = GetCoordinateFromInch(5.32), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 2 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(4.77), Y = GetCoordinateFromInch(5.32), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 2 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(6.4), Y = GetCoordinateFromInch(5.32), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 2 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(7.4), Y = GetCoordinateFromInch(5.32), Text = model.Phonics.TotalCorrect(2).ToString() });
                #endregion

                #region Phonics 3
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(1.75), Y = GetCoordinateFromInch(5.95), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 3 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(3.2), Y = GetCoordinateFromInch(5.95), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 3 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(4.77), Y = GetCoordinateFromInch(5.95), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 3 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(6.4), Y = GetCoordinateFromInch(5.95), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 3 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(7.4), Y = GetCoordinateFromInch(5.95), Text = model.Phonics.TotalCorrect(3).ToString() });
                #endregion

                #region Phonics 4
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(1.75), Y = GetCoordinateFromInch(6.625), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 4 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(3.2), Y = GetCoordinateFromInch(6.625), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 4 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(4.77), Y = GetCoordinateFromInch(6.625), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 4 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 100, X = GetCoordinateFromInch(6.4), Y = GetCoordinateFromInch(6.625), Text = model.Phonics.PhonicsTasks.FirstOrDefault(t => t.CourseId == 4 && t.Task == 1).Correct.ToString() });
                list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(7.4), Y = GetCoordinateFromInch(6.625), Text = model.Phonics.TotalCorrect(4).ToString() });
            #endregion

            #endregion

            #region STAR Reading
            list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(4.2), Y = GetCoordinateFromInch(8.1), Text = model.StarReading.GradeEquivalentLevel.ToString() });
            list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(4.2), Y = GetCoordinateFromInch(8.29), Text = model.StarReading.InstructionalReadingLevel.ToString() });
            list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(4.2), Y = GetCoordinateFromInch(8.48), Text = model.StarReading.ZoneOfProximalDevelopment.ToString() });
            list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(5.7), Y = GetCoordinateFromInch(8.1), Text = model.StarReading.TimeTaken });
            list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(6.77), Y = GetCoordinateFromInch(8.3), Text = model.StarReading.PracticeQuestionsAnswered.ToString() });
            #endregion

            #region Course Recommendation
            list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(2.25), Y = GetCoordinateFromInch(9.22), Text = model.PrimaryRecommendation });
            list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(.47), Y = GetCoordinateFromInch(9.65), Text = model.ReasonsForPrimaryRecommendation });
            list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(3.4), Y = GetCoordinateFromInch(10.2), Text = model.SecondaryRecommendation });
            list.Add(new PDFDrawableBox { Width = 80, X = GetCoordinateFromInch(.47), Y = GetCoordinateFromInch(10.62), Text = model.ReasonsForSecondaryRecommendation });
            #endregion

            return list;
        }
    }
}
