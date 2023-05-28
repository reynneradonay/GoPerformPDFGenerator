using GoPerformPDFClassLibrary;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Web;

namespace GoPerformPDFGenerator.Services
{
    public sealed class PDFGenerator : IPDFGenerator
    {
        public readonly IWebHostEnvironment _environment;

        public PDFGenerator(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public byte[] Generate(List<Deliverable> deliverables, List<KeyRoleOutcome> keyRoleOutcomes, AssociateInfo associateInfo)
        {
            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            using (var doc = new Document(pdf, pageSize: PageSize.A4, false))
            {
                Text headerText = new Text("GoPerform")
                    .SetFontColor(ColorConstants.WHITE)
                    .SetBold();

                Text headerGoalPeriodText = new Text(HttpUtility.HtmlDecode($"\nGoals for {associateInfo.Period}"))
                    .SetFontColor(ColorConstants.WHITE)
                    .SetFontSize(10);

                Text associateInfoNameText = new Text($"{associateInfo.AssociateName} ({associateInfo.AssociateId})")
                    .SetFontColor(ColorConstants.BLUE);

                Text associateInfoDescText = new Text($"{associateInfo.JobDesc}, {associateInfo.DeptDesc}")
                    .SetFontColor(ColorConstants.BLACK);

                Text associateInfoManagerText = new Text($"Home Manager: {associateInfo.SupervisorName} ({associateInfo.SupervisorId})")
                    .SetFontColor(ColorConstants.BLACK);

                Text keyRoleOutcomesText = new Text("Key Role Outcomes")
                    .SetFontColor(ColorConstants.WHITE)
                    .SetBold();

                Text keyRoleOutcomesSubText = new Text("\n(Key outcomes expected from the role for the year, as defined by the Home manager)")
                    .SetFontColor(ColorConstants.WHITE)
                    .SetFontSize(8);

                Text deliverablesText = new Text("Deliverables")
                    .SetFontColor(ColorConstants.WHITE)
                    .SetBold();

                Text deliverablesSubText = new Text("\n(Key deliverables agreed mutually between associate, HM and BM as applicable)")
                    .SetFontColor(ColorConstants.WHITE)
                    .SetFontSize(8);

                Paragraph headerParagraph = new Paragraph()
                    .Add(headerText)
                    .Add(headerGoalPeriodText)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                    .SetTextAlignment(TextAlignment.CENTER);

                Div headerDiv = new Div()
                    .SetPadding(7)
                    .SetBackgroundColor(WebColors.GetRGBColor("#000048"));

                headerDiv.Add(headerParagraph);

                int offset = associateInfo.ImageData.IndexOf(',') + 1;
                byte[] imageInBytes = Convert.FromBase64String(associateInfo.ImageData[offset..^0]);
                ImageData rawImage = ImageDataFactory.Create(imageInBytes);
                Image associateImage = new Image(rawImage)
                    .ScaleAbsolute(20, 20);

                Table associateInfoTable = new Table(new float[] { 1, 1 })
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetFontSize(10)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMinWidth(UnitValue.CreatePercentValue(100));
                Cell associateInfoNameCell = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.RIGHT)
                   .SetBorder(Border.NO_BORDER)
                   .Add(new Paragraph(associateInfoNameText));
                Cell associateInfoDescCell = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.RIGHT)
                   .SetBorder(Border.NO_BORDER)
                   .Add(new Paragraph(associateInfoDescText));
                Cell associateInfoManagerCell = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.RIGHT)
                   .SetBorder(Border.NO_BORDER)
                   .Add(new Paragraph(associateInfoManagerText));
                Cell associateInfoImageCell = new Cell(3, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetBorder(Border.NO_BORDER)
                   .Add(associateImage.SetAutoScale(true));

                associateInfoTable.AddHeaderCell(associateInfoNameCell);
                associateInfoTable.AddHeaderCell(associateInfoImageCell);
                associateInfoTable.AddHeaderCell(associateInfoDescCell);
                associateInfoTable.AddHeaderCell(associateInfoManagerCell);

                Paragraph associateInfoNameParagraph = new Paragraph(associateInfoNameText)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10);

                Paragraph associateInfoDescParagraph = new Paragraph(associateInfoDescText)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10);

                Paragraph associateInfoManagerParagraph = new Paragraph(associateInfoManagerText)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10);

                Paragraph keyRoleOutcomesParagraph = new Paragraph()
                    .Add(keyRoleOutcomesText)
                    .Add(keyRoleOutcomesSubText)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(12)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                    .SetTextAlignment(TextAlignment.LEFT);

                Div keyRoleOutcomesDiv = new Div()
                    .SetPadding(10)
                    .SetBackgroundColor(WebColors.GetRGBColor("#2E308E"));

                keyRoleOutcomesDiv.Add(keyRoleOutcomesParagraph);

                Paragraph deliverablesParagraph = new Paragraph()
                    .Add(deliverablesText)
                    .Add(deliverablesSubText)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(12)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                    .SetTextAlignment(TextAlignment.LEFT);

                Div deliverablesDiv = new Div()
                    .SetPadding(10)
                    .SetBackgroundColor(WebColors.GetRGBColor("#2E308E"));

                deliverablesDiv.Add(deliverablesParagraph);

                LineSeparator ls = new LineSeparator(new SolidLine());

                doc.Add(headerDiv);
                doc.Add(new Paragraph());
                doc.Add(associateInfoTable);
                doc.Add(new Paragraph());
                doc.Add(ls);
                doc.Add(new Paragraph());
                doc.Add(keyRoleOutcomesDiv);
                doc.Add(new Paragraph());

                Table keyRoleOutcomesTable = new Table(1, false)
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetFontSize(10)
                    .SetHorizontalAlignment(HorizontalAlignment.LEFT)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetMinWidth(UnitValue.CreatePercentValue(100));

                foreach (var item in keyRoleOutcomes)
                {
                    Text keyRoleOutcomeTitleText = new Text(HttpUtility.HtmlDecode(item.Title))
                        .SetFontColor(WebColors.GetRGBColor("#2E308E"))
                        .SetBold();

                    Text keyRoleOutcomeDescText = new Text(HttpUtility.HtmlDecode(ReplaceText(item.Description)))
                        .SetFontColor(ColorConstants.BLACK);

                    Cell keyRoleOutcomeTitleCell = new Cell()
                       .SetBorder(Border.NO_BORDER)
                       .Add(new Paragraph(keyRoleOutcomeTitleText))
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetPadding(5)
                       .SetMargin(5)
                       .SetBorderTop(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f))
                       .SetBorderLeft(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f))
                       .SetBorderRight(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f));

                    Cell keyRoleOutcomeDescCell = new Cell()
                       .SetBorder(Border.NO_BORDER)
                       .Add(new Paragraph(keyRoleOutcomeDescText).SetFontSize(10))
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetPadding(5)
                       .SetMargin(5)
                       .SetBorderLeft(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f))
                       .SetBorderRight(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f));

                    Cell blankCell = new Cell()
                       .SetBorder(Border.NO_BORDER)
                       .Add(new Paragraph("\n").SetFontSize(8))
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetPadding(5)
                       .SetMargin(5);

                    keyRoleOutcomesTable.AddCell(keyRoleOutcomeTitleCell);
                    keyRoleOutcomesTable.AddCell(keyRoleOutcomeDescCell);

                    // TODO: refactor this with the deliverable notes
                    if (item.KeyRoleOutcomeNotes.Count > 0)
                    {
                        Table notesTable = new Table(new float[] { 1, 1, 1, 1, 1 })
                           .SetWidth(UnitValue.CreatePercentValue(100))
                           .SetFontSize(10)
                           .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                           .SetTextAlignment(TextAlignment.CENTER)
                           .SetMinWidth(UnitValue.CreatePercentValue(100))
                           .SetBackgroundColor(WebColors.GetRGBColor("#E8E8E6"));

                        keyRoleOutcomesTable.AddCell(notesTable);

                        int totalNoteCount = item.KeyRoleOutcomeNotes.Count;
                        int totalNoteAppreciationCount = item.KeyRoleOutcomeNotes.Where(i => i.NoteType == "Appreciation").Count();
                        int totalNoteNeedImprovementCount = item.KeyRoleOutcomeNotes.Where(i => i.NoteType == "Need for Improvement").Count();
                        int totalNoteSelfCount = item.KeyRoleOutcomeNotes.Where(i => i.NoteType == "Self").Count();
                        int totalNoteOtherCount = item.KeyRoleOutcomeNotes.Where(i => i.NoteType == "Others").Count();

                        Cell noteTotal = new Cell(1, 1)
                            .SetBorder(Border.NO_BORDER)
                            .Add(new Paragraph($"Total Notes: ({totalNoteCount})").SetFontSize(9))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetPadding(3)
                            .SetMargin(5)
                            .SetBorderBottom(new DashedBorder(ColorConstants.BLACK, 0.2f))
                            .SetWidth(UnitValue.CreatePercentValue(15));

                        Cell noteAppreciationTotal = new Cell(1, 1)
                            .SetBorder(Border.NO_BORDER)
                            .Add(GetNoteImage(GetNoteTypeImagePathByStatus("Appreciation"), true, 12))
                            .Add(new Paragraph(HttpUtility.HtmlDecode($"&nbsp;Appreciation: ({totalNoteAppreciationCount})")).SetFontSize(9))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetPadding(3)
                            .SetMargin(5)
                            .SetBorderBottom(new DashedBorder(ColorConstants.BLACK, 0.2f))
                            .SetWidth(UnitValue.CreatePercentValue(20));

                        Cell noteNeedImprovementTotal = new Cell(1, 1)
                            .SetBorder(Border.NO_BORDER)
                            .Add(GetNoteImage(GetNoteTypeImagePathByStatus("Need for Improvement"), true, 12))
                            .Add(new Paragraph(HttpUtility.HtmlDecode($"&nbsp;Need for Improvement: ({totalNoteNeedImprovementCount})")).SetFontSize(9))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetPadding(3)
                            .SetMargin(5)
                            .SetBorderBottom(new DashedBorder(ColorConstants.BLACK, 0.2f))
                            .SetWidth(UnitValue.CreatePercentValue(27));

                        Cell noteSelfTotal = new Cell(1, 1)
                            .SetBorder(Border.NO_BORDER)
                            .Add(GetNoteImage(GetNoteTypeImagePathByStatus("Self"), true, 12))
                            .Add(new Paragraph(HttpUtility.HtmlDecode($"&nbsp;Self Notes: ({totalNoteSelfCount})")).SetFontSize(9))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetPadding(3)
                            .SetMargin(5)
                            .SetBorderBottom(new DashedBorder(ColorConstants.BLACK, 0.2f))
                            .SetWidth(UnitValue.CreatePercentValue(19));

                        Cell noteOtherTotal = new Cell(1, 1)
                            .SetBorder(Border.NO_BORDER)
                            .Add(GetNoteImage(GetNoteTypeImagePathByStatus("Others"), true, 12))
                            .Add(new Paragraph(HttpUtility.HtmlDecode($"&nbsp;Others: ({totalNoteOtherCount})")).SetFontSize(9))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetPadding(3)
                            .SetMargin(5)
                            .SetBorderBottom(new DashedBorder(ColorConstants.BLACK, 0.2f))
                            .SetWidth(UnitValue.CreatePercentValue(19));

                        notesTable.AddCell(noteTotal);
                        notesTable.AddCell(noteAppreciationTotal);
                        notesTable.AddCell(noteNeedImprovementTotal);
                        notesTable.AddCell(noteSelfTotal);
                        notesTable.AddCell(noteOtherTotal);

                        var groupedNotes = item.KeyRoleOutcomeNotes.GroupBy(g => g.AssociateName)
                            .Select(group => new { AssociateName = group.Key, Notes = group.ToList() })
                            .ToList();

                        foreach (var note in groupedNotes)
                        {
                            Cell noteItem = new Cell(1, 5)
                               .SetBorder(Border.NO_BORDER)
                               .Add(new Paragraph(note.AssociateName).SetFontSize(9))
                               .SetTextAlignment(TextAlignment.LEFT)
                               .SetPadding(3)
                               .SetMargin(5)
                               .SetUnderline();

                            List noteList = new List();

                            foreach (var noteObj in note.Notes)
                            {
                                //noteList.SetListSymbol("\u2022");
                                ListItem listItem = new ListItem(HttpUtility.HtmlDecode($"\t&nbsp;{noteObj.NotesText}"))
                                    .SetListSymbol(GetNoteImage(GetNoteTypeImagePathByStatus(noteObj.NoteType), false, 10));
                                noteList.Add(listItem);
                            }

                            Cell noteItemList = new Cell(1, 5)
                                .SetBorder(Border.NO_BORDER)
                                .Add(noteList).SetFontSize(9)
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                                .SetPadding(10)
                                .SetMarginLeft(10)
                                .SetMarginRight(10);

                            notesTable.AddCell(noteItem);
                            notesTable.AddCell(noteItemList);
                        }
                    }

                    keyRoleOutcomesTable.AddCell(blankCell);
                }

                doc.Add(keyRoleOutcomesTable);

                Table deliverablesTable = new Table(1, false)
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetFontSize(10)
                .SetHorizontalAlignment(HorizontalAlignment.LEFT)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetMinWidth(UnitValue.CreatePercentValue(100));

                Text measuredByText = new Text("Measured by: ")
                    .SetFontColor(ColorConstants.BLACK)
                    .SetBold();

                Text completeByText = new Text("Complete by: ")
                    .SetFontColor(ColorConstants.BLACK)
                    .SetBold();

                Text statusText = new Text("Status: ")
                    .SetFontColor(ColorConstants.BLACK)
                    .SetBold();

                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                doc.Add(deliverablesDiv);
                doc.Add(new Paragraph());

                foreach (var item in deliverables)
                {
                    Text deliverableTitleText = new Text(HttpUtility.HtmlDecode(item.Title))
                        .SetFontColor(WebColors.GetRGBColor("#2E308E"))
                        .SetBold();

                    Text deliverableDescText = new Text(HttpUtility.HtmlDecode(item.Description))
                        .SetFontColor(ColorConstants.BLACK);

                    Text deliverableMeasuredByText = new Text(item.MeasuredBy)
                        .SetFontColor(ColorConstants.BLACK);

                    Text deliverableCompleteByText = new Text(item.CompleteBy)
                        .SetFontColor(ColorConstants.BLACK);

                    Text deliverableStatusText = new Text(item.Status)
                        .SetFontColor(ColorConstants.WHITE)
                        .SetBackgroundColor(GetColorByStatus(item.Status));

                    Text deliverableCreatedByText = new Text($"Created By: {item.CreatedDetails}")
                        .SetFontColor(WebColors.GetRGBColor("#2D70B9"))
                        .SetItalic();

                    Cell deliverableTitleCell = new Cell()
                       .SetBorder(Border.NO_BORDER)
                       .Add(new Paragraph(deliverableTitleText)
                            .Add(new Tab())
                            .AddTabStops(new TabStop(1000, TabAlignment.RIGHT))
                            .Add(deliverableStatusText)
                            .SetFontSize(10))
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetPadding(5)
                       .SetMargin(5)
                       .SetBorderTop(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f))
                       .SetBorderLeft(GetBorderByStatus(item.Status))
                       .SetBorderRight(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f));

                    Cell deliverableDescCell = new Cell()
                       .SetBorder(Border.NO_BORDER)
                       .Add(new Paragraph(deliverableDescText).SetFontSize(10))
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetPadding(5)
                       .SetMargin(5)
                       .SetBorderLeft(GetBorderByStatus(item.Status))
                       .SetBorderRight(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f));

                    Cell deliverableMeasuredByCell = new Cell()
                       .SetBorder(Border.NO_BORDER)
                       .Add(new Paragraph().Add(measuredByText).Add(deliverableMeasuredByText).SetFontSize(10))
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetPadding(5)
                       .SetMargin(5)
                       .SetBorderLeft(GetBorderByStatus(item.Status))
                       .SetBorderRight(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f));

                    Cell deliverableCompleteByCell = new Cell()
                       .SetBorder(Border.NO_BORDER)
                       .Add(new Paragraph().Add(completeByText).Add(deliverableCompleteByText).SetFontSize(10))
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetPadding(5)
                       .SetMargin(5)
                       .SetBorderLeft(GetBorderByStatus(item.Status))
                       .SetBorderRight(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f));

                    Cell deliverableStatusCell = new Cell()
                       .SetBorder(Border.NO_BORDER)
                       .Add(new Paragraph().Add(statusText).Add(deliverableStatusText).SetFontSize(10))
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetPadding(5)
                       .SetMargin(5)
                       .SetBorderLeft(GetBorderByStatus(item.Status))
                       .SetBorderRight(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f));

                    Cell deliverableCreatedByCell = new Cell()
                       .SetBorder(Border.NO_BORDER)
                       .Add(new Paragraph(deliverableCreatedByText).SetFontSize(8))
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetPadding(5)
                       .SetMargin(5)
                       .SetBorderBottom(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f))
                       .SetBorderLeft(GetBorderByStatus(item.Status))
                       .SetBorderRight(new SolidBorder(WebColors.GetRGBColor("#2E308E"), 0.2f));

                    Cell blankCell = new Cell()
                       .SetBorder(Border.NO_BORDER)
                       .Add(new Paragraph("\n").SetFontSize(8))
                       .SetTextAlignment(TextAlignment.LEFT)
                       .SetPadding(5)
                       .SetMargin(5);

                    deliverablesTable.AddCell(deliverableTitleCell);
                    deliverablesTable.AddCell(deliverableDescCell);
                    deliverablesTable.AddCell(deliverableMeasuredByCell);
                    deliverablesTable.AddCell(deliverableCompleteByCell);
                    deliverablesTable.AddCell(deliverableCreatedByCell);

                    if (item.DeliverableNotes.Count > 0)
                    {
                        Table notesTable = new Table(new float[] { 1, 1, 1, 1, 1 })
                           .SetWidth(UnitValue.CreatePercentValue(100))
                           .SetFontSize(10)
                           .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                           .SetTextAlignment(TextAlignment.CENTER)
                           .SetMinWidth(UnitValue.CreatePercentValue(100))
                           .SetBackgroundColor(WebColors.GetRGBColor("#E8E8E6"));

                        deliverablesTable.AddCell(notesTable);

                        int totalNoteCount = item.DeliverableNotes.Count;
                        int totalNoteAppreciationCount = item.DeliverableNotes.Where(i => i.NoteType == "Appreciation").Count();
                        int totalNoteNeedImprovementCount = item.DeliverableNotes.Where(i => i.NoteType == "Need for Improvement").Count();
                        int totalNoteSelfCount = item.DeliverableNotes.Where(i => i.NoteType == "Self").Count();
                        int totalNoteOtherCount = item.DeliverableNotes.Where(i => i.NoteType == "Others").Count();

                        Cell noteTotal = new Cell(1, 1)
                            .SetBorder(Border.NO_BORDER)
                            .Add(new Paragraph($"Total Notes: ({totalNoteCount})").SetFontSize(9))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetPadding(3)
                            .SetMargin(5)
                            .SetBorderBottom(new DashedBorder(ColorConstants.BLACK, 0.2f))
                            .SetWidth(UnitValue.CreatePercentValue(15));

                        Cell noteAppreciationTotal = new Cell(1, 1)
                            .SetBorder(Border.NO_BORDER)
                            .Add(GetNoteImage(GetNoteTypeImagePathByStatus("Appreciation"), true, 12))
                            .Add(new Paragraph(HttpUtility.HtmlDecode($"&nbsp;Appreciation: ({totalNoteAppreciationCount})")).SetFontSize(9))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetPadding(3)
                            .SetMargin(5)
                            .SetBorderBottom(new DashedBorder(ColorConstants.BLACK, 0.2f))
                            .SetWidth(UnitValue.CreatePercentValue(20));

                        Cell noteNeedImprovementTotal = new Cell(1, 1)
                            .SetBorder(Border.NO_BORDER)
                            .Add(GetNoteImage(GetNoteTypeImagePathByStatus("Need for Improvement"), true, 12))
                            .Add(new Paragraph(HttpUtility.HtmlDecode($"&nbsp;Need for Improvement: ({totalNoteNeedImprovementCount})")).SetFontSize(9))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetPadding(3)
                            .SetMargin(5)
                            .SetBorderBottom(new DashedBorder(ColorConstants.BLACK, 0.2f))
                            .SetWidth(UnitValue.CreatePercentValue(27));

                        Cell noteSelfTotal = new Cell(1, 1)
                            .SetBorder(Border.NO_BORDER)
                            .Add(GetNoteImage(GetNoteTypeImagePathByStatus("Self"), true, 12))
                            .Add(new Paragraph(HttpUtility.HtmlDecode($"&nbsp;Self Notes: ({totalNoteSelfCount})")).SetFontSize(9))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetPadding(3)
                            .SetMargin(5)
                            .SetBorderBottom(new DashedBorder(ColorConstants.BLACK, 0.2f))
                            .SetWidth(UnitValue.CreatePercentValue(19));

                        Cell noteOtherTotal = new Cell(1, 1)
                            .SetBorder(Border.NO_BORDER)
                            .Add(GetNoteImage(GetNoteTypeImagePathByStatus("Others"), true, 12))
                            .Add(new Paragraph(HttpUtility.HtmlDecode($"&nbsp;Others: ({totalNoteOtherCount})")).SetFontSize(9))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetPadding(3)
                            .SetMargin(5)
                            .SetBorderBottom(new DashedBorder(ColorConstants.BLACK, 0.2f))
                            .SetWidth(UnitValue.CreatePercentValue(19));

                        notesTable.AddCell(noteTotal);
                        notesTable.AddCell(noteAppreciationTotal);
                        notesTable.AddCell(noteNeedImprovementTotal);
                        notesTable.AddCell(noteSelfTotal);
                        notesTable.AddCell(noteOtherTotal);

                        var groupedNotes = item.DeliverableNotes.GroupBy(g => g.AssociateName)
                            .Select(group => new { AssociateName = group.Key, Notes = group.ToList() })
                            .ToList();

                        foreach (var note in groupedNotes)
                        {
                            Cell noteItem = new Cell(1, 5)
                               .SetBorder(Border.NO_BORDER)
                               .Add(new Paragraph(note.AssociateName).SetFontSize(9))
                               .SetTextAlignment(TextAlignment.LEFT)
                               .SetPadding(3)
                               .SetMargin(5)
                               .SetUnderline();

                            List noteList = new List();

                            foreach (var noteObj in note.Notes)
                            {
                                //noteList.SetListSymbol("\u2022");
                                ListItem listItem = new ListItem(HttpUtility.HtmlDecode($"\t&nbsp;{noteObj.NotesText}"))
                                    .SetListSymbol(GetNoteImage(GetNoteTypeImagePathByStatus(noteObj.NoteType), false, 10));
                                noteList.Add(listItem);
                            }

                            Cell noteItemList = new Cell(1, 5)
                                .SetBorder(Border.NO_BORDER)
                                .Add(noteList).SetFontSize(9)
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                                .SetPadding(10)
                                .SetMarginLeft(10)
                                .SetMarginRight(10);

                            notesTable.AddCell(noteItem);
                            notesTable.AddCell(noteItemList);
                        }
                    }

                    deliverablesTable.AddCell(blankCell);
                }

                doc.Add(deliverablesTable);

                SetPageFooter(pdf, doc);

                doc.Close();
                doc.Flush();
                pdfBytes = stream.ToArray();
            }

            return pdfBytes;
        }

        private void SetPageFooter(PdfDocument pdf, Document doc)
        {
            int n = pdf.GetNumberOfPages();
            Paragraph footer;
            for (int page = 1; page <= n; page++)
            {
                footer = new Paragraph($"Page {page} of {n}")
                    .SetFontSize(9)
                    .SetFontColor(ColorConstants.GRAY);
                doc.ShowTextAligned(footer, 297.5f, 20, page, TextAlignment.CENTER, VerticalAlignment.MIDDLE, 0);
            }
        }

        private Border GetBorderByStatus(string status)
        {
            return status switch
            {
                "Not Started" => new SolidBorder(WebColors.GetRGBColor("#B81F2D"), 3.0f),
                "In Progress" => new SolidBorder(WebColors.GetRGBColor("#E9C71D"), 3.0f),
                "Completed" => new SolidBorder(WebColors.GetRGBColor("#2DB81F"), 3.0f),
                _ => new SolidBorder(WebColors.GetRGBColor("#2E308E"), 3.0f),
            };
        }

        private Color GetColorByStatus(string status)
        {
            return status switch
            {
                "Not Started" => WebColors.GetRGBColor("#B81F2D"),
                "In Progress" => WebColors.GetRGBColor("#E9C71D"),
                "Completed" => WebColors.GetRGBColor("#2DB81F"),
                _ => WebColors.GetRGBColor("#2E308E"),
            };
        }

        private string GetNoteTypeImagePathByStatus(string status)
        {
            return status switch
            {
                "Appreciation" => $"{_environment.WebRootPath}/images/appreciation.png",
                "Need for Improvement" => $"{_environment.WebRootPath}/images/needimprovement.png",
                "Self" => $"{_environment.WebRootPath}/images/self.png",
                "Others" => $"{_environment.WebRootPath}/images/other.png",
                _ => "",
            };
        }

        private Image GetNoteImage(string imagePath, bool isFloatLeft, int scale)
        {
            ImageData imageData = ImageDataFactory.Create(imagePath);
            Image image = new Image(imageData)
                .ScaleToFit(scale, scale);
            if (isFloatLeft)
            {
                image.SetProperty(Property.FLOAT, FloatPropertyValue.LEFT);
            }

            return image;
        }

        private string ReplaceText(string text)
        {
            return text.Replace("&amp;bull;&amp;nbsp;", "").Replace("&lt;br&gt;", "\n").Replace("<br>", "\n");
        }
    }
}
