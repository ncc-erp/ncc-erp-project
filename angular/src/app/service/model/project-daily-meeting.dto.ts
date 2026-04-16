export interface GetProjectDailyMeetingsDto {
    id: number;
    pmReportId: number;
    projectId: number;
    criterias: MeetingReportCriteriaDetailDto[];
}
export interface MeetingReportCriteriaDetailDto {
    id: number;
    criteriaName: string;
    content: string;
    status: number;
}
export interface ProjectDailyMeetingDto extends GetProjectDailyMeetingsDto {
    summary?: string;
    editMode?: boolean;
}