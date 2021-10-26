using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TF.FocusPay.AuditLogger.BusinessObjects
{

    public enum SystemInfo
    {
        None=0,
        FocusPayPortal=1,
        FocusPayPAM=2,
        FocusPayConnect=3,
        FocusPayEmailServer=4
    }

    public enum EventInfo
    {
        None = 0,
        AuthenticateUser = 1,
        CreatePassword = 2,
        GenerateSession = 3,
        ValidatePassword = 4,
        SendingEmail = 5,
        FileUpload = 6,
        FileDownload = 7,
        LoadListofCompanies = 8,
        SearchForACompanies = 9,
        DeleteCompany = 10,
        ExportCompanyList = 11,
        ViewCompanyDetails = 12,
        EditCompanyDetails = 13,
        CreateCompany = 14,
        CreateCompanyAdmin = 15,
        CreateCompanyAdminRole = 16,
        CreateCompanyPAMRules = 17,
        CreateDefaultBinForCompany = 18,
        WelcomeEmail = 19,
        MailLoginCredentials = 20,
        UpdateUserDetails = 21,
        //New Events added by Ketan
        AssignGateway = 22,
        DEAssignGateway = 23,
        RemoveGatewayForCompany = 24,
        RemoveGatewayForMerchant = 25,
        RemoveBinForMerchant = 26,
        URLCreationAfterAddingACompany = 27,
        SearchForAMerchant = 28,
        DeleteMerchant = 29,
        ExportMerchantList = 30,
        ViewMerchantDetails = 31,
        EditMerchantDetails = 32,
        CreateMerchant = 33,
        CreateMerchantAdmin = 34,
        CreateMerchantAdminRole = 35,
        AssignBinRangeForMerchant = 36,
        EditBinRangeForMerchant = 37,
        DeleteBinRangeForMerchant = 38,
        CreateDefaultMerchantAttendant = 39,
        SearchForAAttendant = 40,
        DeleteAttendant = 41,
        ExportAttendantList = 42,
        ViewAttendantDetails = 43,
        EditAttendantDetails = 44,
        CreateAttendant = 45,
        CreateAttendantAdmin = 46,
        CreateAttendantAdminRole = 47,
        AssignPOSToAttendant = 48,
        DeAssignPOSToAttendant = 49,
        LoadListOfRole = 50,
        SearchForARole = 51,
        DeleteRoles = 52,
        ViewRolesDetails = 53,
        EditRolesDetails = 54,
        CreateRoles = 55,
        LoadListOfUsers = 56,
        SearchForAUsers = 57,
        DeleteUsers = 58,
        ViewUsersDetails = 59,
        EditUsersDetails = 60,
        CreateUsers = 61,
        LockStatus = 62,
        ResetPassword = 63,
        EditUserRoles = 64,
        UserDefaultedToPrimitiveRole = 65,
        LoadListOfGateways = 66,
        DeleteGateway = 67,
        ViewGatewayDetails = 68,
        EditGatewayDetails = 69,
        CreateGateway = 70,
        ViewMyPOSLicenses = 71,
        AssignLicenses = 72,
        EmailNotificationWhenWeAssignPOS = 73,
        ExportPOSLicenseList = 74,
        SearchReceipts = 75,
        LoadListOfReceipts = 76,
        DeleteReceipts = 77,
        ViewReceiptDetails = 78,
        EditReceiptDetails = 79,
        CreateMyReceipts = 80,
        ViewMyTransactionSummary = 81,
        SearchMyTransactionSummary = 82,
        ExportMyTransactionSummary = 83,
        ViewMyTransactionHistory = 84,
        SearchMyTransactionHistory = 85,
        ExportMyTransactionHistory = 86,
        ViewTransactionDetails = 87,
        PrintTransactionDetails = 88,
        EmailTransactionDetails = 89,
        MerchantDeleted = 90,
        AttendantDeleted = 91,
        DeAssigningPOSToAttendant = 92,
        DeAssignGatewaySuperAdminLevel = 93,
        DeAssignGatewayCompanyAdminLevel = 94,
        ForgotPassword = 95,
        ChangePassword = 96,
        SwapAdmin = 97,
        Signinasotheruser = 98, //Adde new event by Nazreen on 30 Dec,2010 for MP25 requirement.
        AddNewPrivilegeItem = 99,
        OnlinePOS = 100, //Adde new event by Nazreen on 17 Feb,2011 for MP28 requirement Shelby release.
        ChangeUserID = 101,  //Adde new event by Nazreen on 1 Dec,2011 for MP44 requirement Beat release.
        PostProcessingthroughTransactionHistory=102, //Added new event by Nazreen on 19 Dec,2011 for MP38 requrement Sunny release.
        MerchantActivityStatement=103

    }

    public enum EventTypeInfo
    {
        None = 0,
        IdentificationAndAuthentication	=	1	,
        DatabaseOperation	=	2	,
        BusinessOperation	=	3	,
        IOOperation	=	4	,
        GatewayOperation	=	5	,
        EmailOperation	=	6	,
        BackgroundProcess	=	7	,
        ExceptionHandling	=	8	,
        DataValidation	=	9	
    }

    public enum EntityInfo
    {
        None = 0,
        Company = 1,
        Merchant = 2,
        Attendant = 3,
        User = 4,
        Role = 5,
        Gateway = 6,
        Login = 7,
        Email = 8,
        License = 9,
        Transaction = 10,
        Privilege = 11,
        Receipt = 12,
        BinRange = 13,
        Currency = 14
    }


    public enum StateInfo
    {
        None = 0,
        Success=1,
        Error=2,
        Exception=3,
        Warning=4,
        Alert=5,
        Message=6,
        Prompt=7
    }

    public enum TableInfo
    {
        None = 0,
        Attendants = 1,
        AuditTrailsInfo = 2,
        AuthTransactions = 3,
        BatchHeader = 4,
        CCTransactionAdditionalInfo = 5,
        CCTransactions = 6,
        ChanakyaTransactions = 7,
        CompanyBin = 8,
        CompanyGatewayMaster = 9,
        CompanyPAMRules = 10,
        CompanyPOSLicenseDetails = 11,
        CompanyPOSLicenseSummary = 12,
        CompanyRegistration = 13,
        CurrencyMaster = 14,
        EmailPriority = 15,
        EmailQueue = 16,
        EmailType = 17,
        EventInfo = 18,
        GatewayMaster = 19,
        GatewayTransactions = 20,
        LevelMaster = 21,
        ManageLogos = 22,
        MerchantBinGateways = 23,
        MerchantGatewayInfo = 24,
        MerchantPOSLicenseDetails = 25,
        MerchantPOSLicenseSummary = 26,
        MerchantRegistration = 27,
        PAMRules = 28,
        PasswordHistory = 29,
        POSApplication = 30,
        POSType = 31,
        Prospect = 32,
        ProspectInformation = 33,
        ReceiptInfo = 34,
        RoleMaster = 35,
        RolePrivileges = 36,
        ScreenMaster = 37,
        SystemInfo = 38,
        TransactionTypes = 39,
        UserActivity = 40,
        UserRoles = 41,
        Users = 42,
        UserSession = 43,
        UserStatus = 44
    }
}
