namespace Breeze.Models.Constants;

public struct Usernames
{
    public const string SYSTEM_USERNAME = "BreezeFramework";
}

public struct StoreProcedureNames
{
    public const string GET_SUBJECT_SUMMARY = "procGetSubjectSummary";
}

public struct DapperSPParams
{
    public const string STUDENT_ID = "@studentId";
    public const string SUBJECT_ID = "@subjectId";
    public const string CHAPTER_ID = "@chapterId";
    public const string CARD_TYPE_ID = "@cardTypeId";
    public const string DISCIPLINE_ID = "@disciplineId";
}

public struct TableNames
{
    public const string USERS_TABLE = "users";
    public const string COLLEGES_TABLE = "colleges";
    public const string OTP_CODES_TABLE = "otp_codes";
    public const string BOARD_DETAILS_TABLE = "board_details";
}

public struct DbColumnNames
{
    public const string ID = "id";
    public const string FIRST_NAME = "first_name";
    public const string LAST_NAME = "last_name";
    public const string USER_ID = "user_id";
    public const string OTP_CODE = "otp_code";
    public const string EXPIRATION_TIME = "expiration_time";
    public const string OTP_USE_CASE = "otp_use_case";
    public const string GENDER = "gender";
    public const string USER_HANDLE = "user_handle";
    public const string ROW_VERSION = "row_version";
    public const string CREATED_BY = "created_by";
    public const string CREATED_DATE = "created_date";
    public const string MODIFIED_BY = "modified_by";
    public const string MODIFIED_DATE = "modified_date";
    public const string DELETED = "deleted";
    public const string TRUSTED_DEVICE_ID = "trusted_device_id";
    public const string ACCEPTED_TERMS_AND_CONDITIONS = "accepted_terms_and_conditions";
    public const string USER_NAME = "user_name";
    public const string COLLEGE_NAME = "college_name";
    public const string BOARD_ID = "board_id";
    public const string BOARD_NAME = "board_name";
    public const string COLLEGE_ID = "college_id";
}

public struct UserRoles
{
    public const string ADMIN_ROLE = "Admin";
}

public struct CommandConstants
{
    public const string EXEC_COMMAND = "exec";
}

public struct OTPUseCases
{
    public const string REGISTER_OTP = "ROTP";
    public const string LOGIN_OTP = "LOTP";
    public const string FORGOT_OTP = "FOTP";
    public const string VERIFY_EMAIL_OTP = "VOTP";
}