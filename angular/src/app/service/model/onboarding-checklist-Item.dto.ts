export interface OnboardingChecklistItemDto {
    key: string;
    label: string;
    isChecked: boolean;
    details: string[];
} 

export class AddOnboardingDto {
    projectUserId: number;
    checklist: OnboardingChecklistItemDto[];
}