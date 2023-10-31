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
        public string Exception { get; set; } = string.Empty;

        [Column(DbColumnNames.MESSAGE)]
        public string Message { get; set; } = string.Empty;

        [Column(DbColumnNames.STATUS_CODE)]
        public int StatusCode { get; set; }

        [Column(DbColumnNames.STACK_TRACE)]
        public string StackTrace { get; set; } = string.Empty;

        [Column(DbColumnNames.USER_DESCRIPTION)]
        public string? UserDescription { get; set; }

        [Column(DbColumnNames.REQUEST_METHOD)]
        public string RequestMethod { get; set; } = string.Empty;

        [Column(DbColumnNames.REQUEST_PATH)]
        public string RequestPath { get; set; } = string.Empty;

        [Column(DbColumnNames.REQUEST_HEADERS)]
        public string RequestHeaders { get; set; } = string.Empty;

        [Column(DbColumnNames.SOURCE)]
        public string Source { get; set; } = string.Empty;
    }
}
