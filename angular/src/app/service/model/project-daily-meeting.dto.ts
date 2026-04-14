export interface ProjectDailyMeetingDto {
    id: number;
    summary: string;
    pmReportId: number;
    projectId: number;
    dailyReports: ProjectDailyReportDto[];
    criterias: MeetingReportCriteriaDto[];
    editMode: boolean;
}

export interface ProjectDailyReportDto {
    id: number;
    date: Date | string;
    content: string;
    weeklySummaryId: number;
    projectId: number;
    editMode: boolean;
}
export interface MeetingReportCriteriaDto {
    id: number;
    criteriaName: string;
    content: string;
    status: number;
    editMode: boolean;
    originalStatus?: number;
    originalContent?: string;
}