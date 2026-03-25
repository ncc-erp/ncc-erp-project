export interface ProjectDailyMeetingDto {
    id: number;
    summary: string;
    pmReportId: number;
    projectId: number;
    dailyReports: ProjectDailyReportDto[];
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