namespace GoPerformPDFGenerator.Models
{
    public class KeyRoleOutcome
    {
        public KeyRoleOutcome()
        {
            KeyRoleOutcomeNotes = new List<Note>();
        }

        public int KeyRoleId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<Note> KeyRoleOutcomeNotes { get; set; }
    }
}
