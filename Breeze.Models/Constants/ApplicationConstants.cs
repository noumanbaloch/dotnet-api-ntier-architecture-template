namespace Breeze.Models.Constants;

public struct ApiStatusCodes
{
    // 100 series for failures
    public const short RECORD_ALREADY_EXIST = 100;
    public const short INVALID_USERNAME_OR_PASSWORD = 101;
    public const short INVALID_OTP = 102;
    public const short UNABLE_TO_COMPLETE_PROCESS = 103;
    public const short USER_ALREADY_EXIST = 104;
    public const short PASSWORD_NOT_CHANGED = 105;
    public const short SOMETHING_WENT_WRONG = 106;
    public const short RECORD_NOT_FOUND = 107;
    public const short USER_DOES_NOT_EXIST = 108;
    public const short PASSWORD_IS_INVALID = 110;

    // 200 series for success
    public const short RECORD_SAVED_SUCCESSFULLY = 200;
    public const short USER_LOGIN_SUCCESSFULLY = 201;
    public const short VERIFICATION_CODE_SENT = 202;
    public const short OTP_VERIFIED_SUCCESSFULLY = 203;
    public const short PASSWORD_CHANGED = 204;
    public const short RECORD_FOUND = 205;
    public const short REQUEST_COMPLETED_SUCCESSFULLY = 206;
    public const short EMAIL_VERIFIED = 207;
    public const short PROFILE_UPDATED_SUCCESSFULLY = 208;
    public const short WELCOME_MESSAGE = 209;
    public const short WELCOME_BACK_MESSAGE = 210;
}

public struct ApiResponseMessages
{
    public const string RECORD_ALREADY_EXIST = "Record already exists.";
    public const string RECORD_SAVED_SUCCESSFULLY = "Record saved successfully.";
    public const string INVALID_USERNAME_OR_PASSWORD = "Invalid username or password.";
    public const string USER_LOGIN_SUCCESSFULLY = "User logged in successfully.";
    public const string INVALID_OTP = "Invalid OTP code.";
    public const string USER_ALREADY_EXIST = "User already exists.";
    public const string UNABLE_TO_COMPLETE_PROCESS = "Unable to complete the requested process.";
    public const string VERIFICATION_CODE_SENT = "Verification Code Sent! Please check your inbox and spam folder.";
    public const string OTP_VERIFIED_SUCCESSFULLY = "OTP verified successfully.";
    public const string PASSWORD_CHANGED = "Password changed successfully.";
    public const string PASSWORD_NOT_CHANGED = "Password not changed.";
    public const string SOMETHING_WENT_WRONG = "Something went wrong.";
    public const string RECORD_FOUND = "Record found.";
    public const string RECORD_NOT_FOUND = "Record not found.";
    public const string USER_DOES_NOT_EXIST = "User does not exist.";
    public const string REQUEST_COMPLETED_SUCCESSFULLY = "Request completed successfully.";
    public const string EMAIL_VERIFIED = "Email verified.";
    public const string PROFILE_UPDATED_SUCCESSFULLY = "Profile updated successfully.";
    public const string WELCOME_MESSAGE = "Welcome, ";
    public const string WELCOME_BACK_MESSAGE = "Welcome back, ";
    public const string PASSWORD_IS_INVALID = "Password is invalid.";
}

public struct ExceptionMessages
{
    public const string RECORD_MODIFIED_BY_OTHER_USER = "Record has been modified by another user.";
    public const string ERROR_UPDATING_ROW_VERSION = "Error updating RowVersion property.";
    public const string FILE_NOT_FOUND = "File not found";
    public const string FAILED_TO_START_API = "Error while creating and building generic host builder object";
}

public struct RequestValidationMessages
{
    public const string USERNAME_REQUIRED_MSG = "Please enter a username.";
    public const string CURRENT_PASS_REQUIRED_MSG = "Please enter your current password.";
    public const string NEW_PASS_REQUIRED_MSG = "Please enter a new password.";
    public const string FIRST_NAME_REQUIRED_MSG = "Please enter your first name.";
    public const string LAST_NAME_REQUIRED_MSG = "Please enter your last name.";
    public const string PASSWORD_REQUIRED_MSG = "Please enter a password.";
    public const string PHONE_NUMBER_REQUIRED_MSG = "Please enter your phone number.";
    public const string ACCEPTED_TERMS_AND_CONDITIONS_REQUIRED_MSG = "Please accept the terms and conditions.";
    public const string OTP_CODE_REQUIRED_MSG = "Please enter the OTP code.";
    public const string VALID_EMAIL_ADDRESS_REQUIRED_MSG = "Please enter a valid email address.";
    public const string OTP_USE_CASE_REQUIRED_MSG = "The OTP use case is required.";
}

public struct JWTClaimNames
{
    public const string USER_ID = "UserId";
    public const string USER_NAME = "UserName";
    public const string EMAIL = "Email";
    public const string FIRST_NAME = "FirstName";
    public const string LAST_NAME = "LastName";
    public const string PHONE_NUMBER = "PhoneNumber";
    public const string FULL_NAME = "FullName";
    public const string JTI = "JWT ID";
    public const string IAT = "Issued At";
}

public struct Characters
{
    public const string SPACE = "\u0020";
    public const string LEFT_BRACE = "\u007B";
    public const string RIGHT_BRACE = "\u007D";
    public const string DOT = "\u002E";
}

public struct MagicNumbers
{
    public const short OTP_LENGTH = 4;
    public const short OTP_EXPIRY_MINUTES = 3;
    public const short TOKEN_EXPIRY_DAYS = 7;
    public const short DEFAULT_SUBSCRIPTION_LEVEL = 1;
}

public struct FileExtensions
{
    public const string HTML = ".html";
    public const string TEXT = ".text";
    public const string IMAGE = ".image";
    public const string PDF = ".pdf";
    public const string MP4 = ".mp4";
}

public struct CacheKeys
{
    public const string TRUSTED_DEVICE = "TDev_";
}

public struct EmailTemplates
{
    public const string REGISTER_TEMPLATE = "register";
    public const string LOGIN_TEMPLATE = "login";
    public const string FORGOT_PASSWORD_TEMPLATE = "forgot";
    public const string VERIFY_EMAIL_TEMPLATE = "verify_email";
}

public struct EmailSubjects
{
    public const string VERIFICATION_CODE_EMAIL = "Your Breeze account verification code";
}

public struct CompanyEmailAddresses
{
    public const string SALES_EMAIL_ADDRESS = "breeze.sales@gmail.com";
}

public struct PropertyNames
{
    public const string DEVICE_ID = "DeviceId";
    public const string ROW_VERSION = "RowVersion";
}

public struct ContainerNames
{
    public const string APPLICATION_LOGS_CONTAINER = "applicationlogs";
}

public struct EnvironmentNames
{
    public const string QA_ENV = "qa";
}