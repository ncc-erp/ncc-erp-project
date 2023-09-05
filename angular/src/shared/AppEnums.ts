import { TenantAvailabilityState } from '@shared/service-proxies/service-proxies';


export class AppTenantAvailabilityState {
    static Available: number = TenantAvailabilityState._1;
    static InActive: number = TenantAvailabilityState._2;
    static NotFound: number = TenantAvailabilityState._3;
}
export const DATE_FORMATS = {
    parse: {
        dateInput: 'DD/MM/YYYY',
    },
    display: {
        dateInput: 'DD/MM/YYYY',
        monthYearLabel: 'DD MM YYYY',
        dateA11yLabel: 'DD MM YYYY',
        monthYearA11yLabel: 'DD MM YYYY',
    },
};
export const APP_ENUMS = {
    // ProjectType: {
    //     ODC: 0,
    //     'T&M': 1,
    //     FIXPRICE: 2,
    //     PRODUCT: 3
    // },
    // ProjectStatus: {
    //     POTENTIAL: 0,
    //     'IN PROGRESS': 1,
    //     MAINTAIN: 2,
    //     CLOSED: 3
    // },
    IssueCritical:
    {
        Low: 0,
        Medium: 1,
        High: 2
    },
    Currency:
    {
        VND: 0,
        USD: 1,
        EUR: 2,
    },
    MilestoneFlag:
    {
        Green: 0,
        Red: 1

    },
    PMReportProjectIssueStatus:
    {
        InProgress: 0,
        Done: 1
    },
    PMReportProjectRiskStatus:
    {
        InProgress: 0,
        Done: 1
    },
    PMReportProjectIssue:
    {
        InProgress: "InProgress",
        Done: "Done"
    },
    
    PMReportProjectStatus:
    {
        Draft: 0,
        Sent: 1
    },
    PMReportStatus:
    {
        Active: 0,
        Done: 1
    },
    PMReportType:
    {
        Weekly: 0,
        Monthly: 1
    },
    ProjectHealth:
    {
        Green: 1,
        Yellow: 2,
        Red: 3
    },
    ProjectMilestoneStatus:
    {
        Paid: 2,
        UAT: 1,
        Upcoming: 0,
        Fail: 3
    },
    ProjectStatus:
    {
        Potential: 0,
        InProgress: 1,
        Closed: 2
    },
    ProjectType:
    {
        ODC: 0,
        TAM: 1,
        FIXPRICE: 2,
        PRODUCT: 3,
        NOCHARGE: 4,
        TRAINING: 5
    },
    SubProjectType: {
        ODC: 0,
        TAM: 1,
        FIXPRICE: 2,
        NoBill: 4,
    },

    CurrencyChargeProjectType: {
        ODC: 0,
        TAM: 1,
        FIXPRICE: 2,
    },

    ProjectUserStatus:
    {
        Present: 0,
        Past: 1,
        Future: 2
    },
    ResourceRequestStatus:
    {
        PENDING: 0,
        DONE: 1,
        CANCELLED: 2,
        APPROVED: 3,
    },
    ProjectIssueSource:
    {
        Internal: 0,
        External: 1,
        Others: 2
    },
    TimesheetStatus:
    {
        Active: 0,
        Done: 1
    },
    ProjectUserRole:
    {
        PM: 0,
        DEV: 1,
        TESTER: 2,
        BA: 3,
        Artist: 4,
    },
    SaodoStatus:
    {
        New: 0,
        InProcess: 1,
        Done: 2
    },
    Branch:
    {
        "Hà Nội": 0,
        "Đà Nẵng": 1,
        "Hồ Chí Minh": 2,
        "Vinh": 3,
        Other: 4
    },
    UserBranch: {
        "HN": 0,
        "DN": 1,
        "HCM": 2,
        "Vinh": 3,
    },
    UserLevel:
    {
        Intern_0: 0,
        Intern_1: 1,
        Intern_2: 2,
        Intern_3: 3,
        FresherMinus: 4,
        Fresher: 5,
        FresherPlus: 6,
        JuniorMinus: 7,
        Junior: 8,
        JuniorPlus: 9,
        MiddleMinus: 10,
        Middle: 11,
        MiddlePlus: 12,
        SeniorMinus: 13,
        Senior: 14,
        Principal: 15,
    },
    UserType: {
        TTS: 0,
        CTV: 1,
        Staff: 2,
        "T.Việc": 3,
        FakeUser: 4,
        Vendor: 5

    },
    UserTypeTabAllResource: {
        TTS: 0,
        CTV: 1,
        Staff: 2,
        "T.Việc": 3,

    },
    WeeklySent: {
        Unsent: 0,
        Sent: 1
    },
    TypePhase: {
        Main: 0,
        Sub: 1
    },
    CheckPointUserType:
    {
        PM: 0,
        Team: 1,
        Client: 2,
        Self: 3,
        Exam: 4
    },
    CheckPointUserStatus:
    {
        Draft: 0,
        Reviewed: 1
    },
    CheckpointUserResult: {
        Draft: 0,
        UserDone: 1,
        PMDone: 2,
        FinalDone: 3,

    },
    checkpointUserTags: {
        Up: 1,

    },
    ChargeType:
    {
        Daily : 0,
        Monthly : 1,
        Hour : 2
    },
    Priority:{
        Low: 0,
        Medium: 1,
        High: 2,
        Critical: 3
    },
    Sortable:{
        ASC: 0,
        DESC: 1
    },
    ExportInvoiceMode:
    {
        Normal: 0,
        MontlyToDaily: 1
    },
    InvoiceSetting: {
        Main: true,
        Sub: false
    },
    AuditStatus:{
        Green:1,
        Amber: 2,
        Red: 3
    }
}

