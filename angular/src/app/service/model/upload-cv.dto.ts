export class GetCvBillAccountDto {
    id: number;
    nameCv: string;
    linkCV: string;
}

export class UploadCvBillAccountDto {
    id: number;
    nameCv: string;
    selectedFile: File;
}