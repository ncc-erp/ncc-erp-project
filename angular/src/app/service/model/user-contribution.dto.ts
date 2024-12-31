import { PagedRequestDto } from "../../../shared/paged-listing-component-base";

export class FilterMonthlyUserContributionDto extends PagedRequestDto
{
    branchIds: number[];
    userTypes: number[];
    userLevels: number[];
    monthTime: number;
    yearTime: number;

    constructor(branchIds: number[], userTypes: number[], userLevels: number[], monthTime: number, yearTime: number) {
        super();
        this.branchIds = branchIds;
        this.userTypes = userTypes;
        this.userLevels = userLevels;
        this.monthTime = monthTime;
        this.yearTime = yearTime;
    }
}