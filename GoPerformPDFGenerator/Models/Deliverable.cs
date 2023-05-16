namespace GoPerformPDFGenerator.Models
{
    public class Deliverable
    {
        public Deliverable()
        {
            DeliverableNotes = new List<DeliverableNote>();
        }

        public int DeliverableId { get; set; }

        public int AssociateId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string MeasuredBy { get; set; }

        public string CompleteBy { get; set; }

        public string Status { get; set; }

        public string Date { get; set; }

        public string CreatedDetails { get; set; }

        public string ModifiedDetails { get; set; }

        public List<DeliverableNote> DeliverableNotes { get; set; }
    }
}
