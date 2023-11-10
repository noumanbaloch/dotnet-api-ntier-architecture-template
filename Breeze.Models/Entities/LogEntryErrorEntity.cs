using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Breeze.Models.Entities
{
    [Table(TableNames.LOG_ENTRY_ERRORS_TABLE)]
    public class LogEntryErrorEntity : BaseEntity
    {
        [Key]
        [Column(DbColumnNames.ID)]
        public int Id { get; set; }

        [Column(DbColumnNames.EXCEPTION)]
        public string Exception { get; set; } = null!;

        [Column(DbColumnNames.MESSAGE)]
        public string Message { get; set; } =  null!;

        [Column(DbColumnNames.STATUS_CODE)]
        public int StatusCode { get; set; }

        [Column(DbColumnNames.STACK_TRACE)]
        public string StackTrace { get; set; } = null!;

        [Column(DbColumnNames.USER_DESCRIPTION)]
        public string? UserDescription { get; set; }

        [Column(DbColumnNames.REQUEST_METHOD)]
        public string RequestMethod { get; set; } = null!;

        [Column(DbColumnNames.REQUEST_PATH)]
        public string RequestPath { get; set; } = null!;

        [Column(DbColumnNames.REQUEST_HEADERS)]
        public string RequestHeaders { get; set; } = null!;

        [Column(DbColumnNames.SOURCE)]
        public string Source { get; set; } = null!;
    }
}
