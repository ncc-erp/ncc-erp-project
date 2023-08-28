export class ReviewYourselfDto{
    id: number;
    note: string;
    score: number;
    listDetail: listDetail[]
}
export class listDetail 
    {
      id: number;
      criteriaId: number;
      note: string;
      checkPointUserId: number;
      score: number;
    }