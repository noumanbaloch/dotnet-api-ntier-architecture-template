using Breeze.Models.Dtos.Subject.SP;
using Breeze.Models.GenericResponses;

namespace Breeze.Services.Subject;
public interface ISubjectFacadeService
{
    Task<GenericResponse<IEnumerable<SubjectSummarySPDto>>> GetSubjectSummary(int subjectId);
}
