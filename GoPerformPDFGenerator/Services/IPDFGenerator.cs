using GoPerformPDFGenerator.Models;

namespace GoPerformPDFGenerator.Services
{
    public interface IPDFGenerator
    {
        byte[] Generate(List<Deliverable> deliverables, List<KeyRoleOutcome> keyRoleOutcomes, AssociateInfo associateInfo);
    }
}
