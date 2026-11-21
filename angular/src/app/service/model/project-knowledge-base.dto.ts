export interface AbpResponse<T> {
  result: T;
}

export interface SavedKnowledgeBaseDto {
  projectId: string;
  content: string | null;
  updatedAt: string | null;
}

export interface KnowledgeBaseDto extends SavedKnowledgeBaseDto {
  referenceFiles: KnowledgeBaseReferenceFileDto[];
}

export interface KnowledgeBaseReferenceFileDto {
  projectFileId: string;
  name: string;
}

export interface KnowledgeBaseFileOptionDto extends KnowledgeBaseReferenceFileDto {
  isSelected: boolean;
}

export interface GeneratedKnowledgeBaseDto {
  content: string | null;
}
