export interface GetProjectDailyMeetingsDto {
    id?: number | null;
    pmReportId: number;
    projectId: number;
    criterias: MeetingReportCriteriaDetailDto[];
    editMode?: boolean;

}
export interface MeetingReportCriteriaDetailDto {
    id: number;
    criteriaName: string;
    originalContent?: string;
    content: string;
    status: number;
    editMode?: boolean;
}