using Breeze.Models.Dtos.Subject.SP;

namespace Breeze.Services.Subject;
public interface ISubjectService
{
    Task<IEnumerable<SubjectSummarySPDto>> GetSubjectsSummary(int subjectId);
}
