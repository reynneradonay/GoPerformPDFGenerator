namespace GoPerformPDFGenerator.Models
{
    public class Note
    {
        public Note()
        {
            Attachments = new List<NoteAttachment>();
        }

        public string AssociateID { get; set; }

        public string AssociateName { get; set; }

        public int NoteID { get; set; }

        public string NoteType { get; set; }

        public string NotesText { get; set; }

        public int DeletionAction { get; set; }

        public string NoteCreatedDate { get; set; }

        public string SortDate { get; set; }

        public string CommentorName { get; set; }

        public string ReplyIcon { get; set; }

        public int ReplyIconId { get; set; }

        public string ReplyText { get; set; }

        public int EditAction { get; set; }

        public int DeletionReply { get; set; }

        public int IsPrivate { get; set; }

        public int RowStatus { get; set; }

        public List<NoteAttachment> Attachments { get; set; }
    }
}
