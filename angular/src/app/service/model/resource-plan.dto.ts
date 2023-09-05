import { SocialAuthServiceConfig } from 'angularx-social-login';
export class ResourcePlanDto{
    constructor(_resourceRequestId, _projectUserId){
        this.projectUserId = _projectUserId;
        this.startTime = new Date();
        this.userId = 0;
        this.resourceRequestId = _resourceRequestId
    }
    public projectRole?: number
    public projectUserId?: number
    public resourceRequestId?: number
    public startTime?: any
    public userId?: number
}
export class RetroReviewInternHistoriesDto {
    emails: string[]
}