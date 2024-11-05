export class CVStatusDto {
    color: string;
    name: string;
    id: number | undefined;
    triggerAction: number | null;
}

export class CvStatusCreateEditDto {
    cvStatus: CVStatusDto;
    command: string;
}
